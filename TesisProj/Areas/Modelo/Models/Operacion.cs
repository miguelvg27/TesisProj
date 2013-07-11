using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Plantilla.Models;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelo.Models
{
    [Table("Operacion")]
    public class Operacion : DbObject, IValidatableObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Operación")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Período inicial")]
        public string PeriodoInicial { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Período final")]
        public string PeriodoFinal { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Secuencia")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que 0")]
        public int Secuencia { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Referencia")]
        [RegularExpression("[A-Za-z]+[A-Za-z1-9]*", ErrorMessage = "El campo solo puede contener alfanuméricos y debe comenzar con una letra.")]
        public string Referencia { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Proyecto")]
        public int IdProyecto { get; set; }

        [ForeignKey("IdProyecto")]
        public Proyecto Proyecto { get; set; }

        [DisplayName("Indicador")]
        public bool Indicador { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Cadena")]
        public string Cadena { get; set; }

        public List<double> Valores;

        public Operacion()
        {
        }

        public Operacion(PlantillaOperacion plantilla, int IdProyecto)
        {
            this.IdProyecto = IdProyecto;
            this.Indicador = plantilla.Indicador;
            this.Nombre = plantilla.Nombre;
            this.PeriodoInicial = plantilla.PeriodoInicial;
            this.PeriodoFinal = plantilla.PeriodoFinal;
            this.Referencia = plantilla.Referencia;
            this.Secuencia = plantilla.Secuencia;
            this.Cadena = plantilla.Cadena;
        }

        public List<double> Evaluar(int horizonte, List<Operacion> operaciones, List<TipoFormula> tipoformulas, List<Formula> formulas, List<Parametro> parametros)
        {
            List<double> resultado = new List<double>();

            MathParserNet.Parser parser = new MathParserNet.Parser();
            double valor, pinicial, pfinal;

            try
            {
                for (int i = 1; i <= horizonte; i++)
                {
                    parser.RemoveAllVariables();

                    parser.AddVariable("Periodo", i);
                    parser.AddVariable("Horizonte", horizonte);

                    foreach (TipoFormula tipoformula in tipoformulas)
                    {
                        parser.AddVariable(tipoformula.Referencia, tipoformula.Valores[i - 1]);
                    }

                    foreach (Operacion operacion in operaciones)
                    {
                        parser.AddVariable(operacion.Referencia, operacion.Valores[i - 1]);
                    }

                    pinicial = parser.SimplifyInt(this.PeriodoInicial, MathParserNet.Parser.RoundingMethods.Round);
                    pfinal = parser.SimplifyInt(this.PeriodoFinal, MathParserNet.Parser.RoundingMethods.Round);

                    bool siCalcular = (i >= pinicial && i <= pfinal);

                    if (siCalcular)
                    {
                        if ((this.Cadena.StartsWith("Tir(") || (this.Cadena.StartsWith("Van(")) && this.Cadena.EndsWith(")")))
                        {
                            valor = Generics.ComplexParse(this.Cadena, operaciones);
                        }
                        else
                        {
                            valor = (i >= pinicial && i <= pfinal) ? parser.SimplifyDouble(this.Cadena) : 0;
                            valor = double.IsNaN(valor) ? 0 : valor;
                        }
                    }
                    else
                    {
                        valor = 0;
                    }

                    resultado.Add(valor);
                }
            }

            catch (Exception)
            {
            }

            return resultado;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.Operaciones.Any(f => f.Nombre == this.Nombre && f.IdProyecto == this.IdProyecto && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre en el mismo proyecto.", new string[] { "Nombre" });
                }

                if (context.Operaciones.Any(f => f.Referencia == this.Referencia && f.IdProyecto == this.IdProyecto && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre de referencia en el mismo proyecto.", new string[] { "Referencia" });
                }

                if (context.Operaciones.Any(f => f.Secuencia == this.Secuencia && f.IdProyecto == this.IdProyecto && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo número de secuencia en el mismo proyecto.", new string[] { "Secuencia" });
                }

                MathParserNet.Parser parser = new MathParserNet.Parser();
                var tipoformulas = context.TipoFormulas;
                var operaciones = context.Operaciones.Where(o => o.IdProyecto == this.IdProyecto && o.Secuencia < this.Secuencia);

                parser.AddVariable("Horizonte", 10);

                foreach (TipoFormula tipoformula in tipoformulas)
                {
                    parser.AddVariable(tipoformula.Referencia, 2);
                }

                foreach (Operacion operacion in operaciones)
                {
                    parser.AddVariable(operacion.Referencia, 2);
                }

            //
            //  Valida si es Tir o Van
                if (!Generics.Validar(this.Cadena, parser))
                {
                    if (!Generics.TestComplexParse(this.Cadena, operaciones.ToList()))
                    {
                        yield return new ValidationResult("Cadena inválida. La operación solo puede contener referencias a tipos de fórmula.", new string[] { "Cadena" });
                    }
                }

            //
            //  Valida períodos
                if (!Generics.Validar(this.PeriodoInicial, parser))
                {
                    yield return new ValidationResult("Cadena inválida. Solo puede contener Horizonte o números.", new string[] { "PeriodoInicial" });
                }

                if (!Generics.Validar(this.PeriodoFinal, parser))
                {
                    yield return new ValidationResult("Cadena inválida. Solo puede contener Horizonte o números.", new string[] { "PeriodoFinal" });
                }
            }
        }
    }
}
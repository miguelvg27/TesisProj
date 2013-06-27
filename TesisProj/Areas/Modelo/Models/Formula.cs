using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Areas.Plantilla.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Modelo.Models
{
    [Table("Formula")]
    public class Formula : DbObject, IValidatableObject
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Fórmula")]
        public string Nombre { get; set; }

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
        [DisplayName("Elemento")]
        public int IdElemento { get; set; }

        [ForeignKey("IdElemento")]
        public Elemento Elemento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Tipo")]
        public int IdTipoFormula { get; set; }

        [ForeignKey("IdTipoFormula")]
        public TipoFormula TipoFormula { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Período inicial")]
        public string PeriodoInicial { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Período final")]
        public string PeriodoFinal { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Cadena")]
        public string Cadena { get; set; }

        public Formula()
        {
        }

        public Formula(PlantillaFormula plantilla, int IdElemento)
        {
            this.IdElemento = IdElemento;
            this.Referencia = plantilla.Referencia;
            this.Secuencia = plantilla.Secuencia;
            this.Nombre = plantilla.Nombre;
            this.IdTipoFormula = plantilla.IdTipoFormula;
            this.Cadena = plantilla.Cadena;
            this.PeriodoInicial = plantilla.PeriodoInicial;
            this.PeriodoFinal = plantilla.PeriodoFinal;
        }

        public List<double> Evaluar(int horizonte, List<Formula> formulas, List<Parametro> parametros)
        {
            List<double> resultado = new List<double>();
            MathParserNet.Parser parser = new MathParserNet.Parser();
            formulas.OrderBy(f => f.Secuencia);
            double valor, pinicial, pfinal;

            try
            {
                for (int i = 1; i <= horizonte; i++)
                {
                    parser.Reset();
                    foreach (Parametro parametro in parametros)
                    {
                        parser.AddVariable(parametro.Referencia, (double) parametro.Celdas.First(c => c.Periodo == (parametro.Constante ? 1 : i)).Valor);
                    }

                    foreach (Formula formula in formulas)
                    {
                        valor = parser.SimplifyDouble(formula.Cadena);
                        parser.AddVariable(formula.Referencia, valor);
                    }

                    pinicial = parser.SimplifyDouble(this.PeriodoInicial);
                    pfinal = parser.SimplifyDouble(this.PeriodoFinal);

                    valor = (i >= pinicial && i <= pfinal) ? valor = parser.SimplifyDouble(this.Cadena) : 0;
                    
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
                if (context.Formulas.Any(f => f.Nombre == this.Nombre && f.IdElemento == this.IdElemento && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre en el mismo elemento.", new string[] { "Nombre" });
                }

                if (context.Formulas.Any(f => f.Referencia == this.Referencia && f.IdElemento == this.IdElemento && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe una fórmula con el mismo nombre de referencia en el mismo elemento.", new string[] { "Referencia" });
                }

                if (context.Parametros.Any(p => p.Referencia == this.Referencia && p.IdElemento == this.IdElemento && (this.Id > 0 ? p.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un parámetro con el mismo nombre de referencia en el mismo elemento.", new string[] { "Referencia" });
                }

                if (context.Formulas.Any(f => f.Secuencia == this.Secuencia && f.IdElemento == this.IdElemento && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo número de secuencia en el mismo elemento.", new string[] { "Secuencia" });
                }

                //  Valida cadena de la fórmula

                bool cadenavalida = true;
                double testvalue = 0;
                MathParserNet.Parser parser = new MathParserNet.Parser();
                var parametros = context.Parametros.Where(p => p.IdElemento == this.IdElemento);
                var formulas = context.Formulas.Where(p => p.IdElemento == this.IdElemento && p.Secuencia < this.Secuencia);

                foreach (Parametro parametro in parametros)
                {
                    parser.AddVariable(parametro.Referencia, Math.PI);
                }

                foreach (Formula formula in formulas)
                {
                    parser.AddVariable(formula.Referencia, Math.PI);
                }

                try
                {
                    testvalue = parser.SimplifyDouble(this.Cadena);
                }
                catch (Exception)
                {
                    cadenavalida = false;
                }

                if (!cadenavalida)
                {
                    yield return new ValidationResult("Cadena inválida. La fórmula solo puede contener los parámetros y las fórmulas con menor secuencia del elemento.", new string[] { "Cadena" });
                }

                //  Valida las fórmulas del período inicial y final

                cadenavalida = true;
                parser.Reset();
                parametros = context.Parametros.Where(p => p.IdElemento == this.IdElemento && p.IdTipoParametro == 2);

                foreach (Parametro parametro in parametros)
                {
                    parser.AddVariable(parametro.Referencia, 5);
                }

                try
                {
                    testvalue = parser.SimplifyInt(this.PeriodoInicial);
                }
                catch (Exception)
                {
                    cadenavalida = false;
                }

                if (!cadenavalida || this.PeriodoInicial.Contains('/'))
                {
                    yield return new ValidationResult("Cadena inválida. La fórmula solo puede contener los parámetros enteros del elemento y no puede contener divisiones.", new string[] { "PeriodoInicial" });
                }

                cadenavalida = true;

                try
                {
                    testvalue = parser.SimplifyInt(this.PeriodoFinal);
                }
                catch (Exception)
                {
                    cadenavalida = false;
                }

                if (!cadenavalida || this.PeriodoFinal.Contains('/'))
                {
                    yield return new ValidationResult("Cadena inválida. La fórmula solo puede contener los parámetros enteros del elemento y no puede contener divisiones.", new string[] { "PeriodoFinal" });
                }
            }
        }
    }
}
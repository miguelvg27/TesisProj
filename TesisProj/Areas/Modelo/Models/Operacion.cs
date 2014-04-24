using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
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
        [StringLength(30, MinimumLength = 2, ErrorMessage = "El campo {0} debe tener un mínimo de {2} y un máximo de {1} carácteres.")]
        [DisplayName("Referencia")]
        [RegularExpression("[A-Za-z]+[A-Za-z1-9]*", ErrorMessage = "El campo solo puede contener alfanuméricos y debe comenzar con una letra.")]
        public string Referencia { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Proyecto")]
        public int IdProyecto { get; set; }

        [XmlIgnore]
        [ForeignKey("IdProyecto")]
        public virtual Proyecto Proyecto { get; set; }

        [DisplayName("Indicador")]
        public bool Indicador { get; set; }

        [DisplayName("Subrayar")]
        public bool Subrayar { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Cadena")]
        public string Cadena { get; set; }

        [DisplayName("Sensible")]
        public bool Sensible { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Tipo de dato")]
        public int IdTipoDato { get; set; }

        [XmlIgnore]
        [ForeignKey("IdTipoDato")]
        public virtual TipoDato TipoDato { get; set; }

        [StringLength(2048)]
        [DisplayName("Valores")]
        public string strValores { get; set; }

        [StringLength(2048)]
        public string strValoresInvariante { get; set; }

        [InverseProperty("Operacion")]
        public List<SalidaOperacion> Salidas { get; set; }

        [XmlIgnore]
        public List<double> Valores;

        [XmlIgnore]
        [NotMapped]
        public int valPeriodoInicial { get; set; }

        [XmlIgnore]
        [NotMapped]
        public int valPeriodoFinal { get; set; }

        [XmlIgnore]
        public String ListName { get { return Nombre + " (" + Referencia + ")"; } }

        public Operacion() { }

        public Operacion(PlantillaOperacion plantilla, int idProyecto)
        {
            this.IdProyecto = idProyecto;
            this.Indicador = plantilla.Indicador;
            this.Nombre = plantilla.Nombre;
            this.PeriodoInicial = plantilla.PeriodoInicial;
            this.PeriodoFinal = plantilla.PeriodoFinal;
            this.Referencia = plantilla.Referencia;
            this.Secuencia = plantilla.Secuencia;
            this.Cadena = plantilla.Cadena;
            this.Subrayar = plantilla.Subrayar;
            this.IdTipoDato = plantilla.IdTipoDato;
        }

        public override string LogValues()
        {
            return "Nombre = " + this.Nombre + Environment.NewLine +
                "Referencia = " + this.Referencia + Environment.NewLine +
                "Secuencia = " + this.Secuencia + Environment.NewLine +
                "Indicador = " + this.Indicador + Environment.NewLine +
                "Tipo dato = " + this.TipoDato.Nombre + Environment.NewLine +
                "Período inicial = " + this.PeriodoInicial + Environment.NewLine +
                "Periodo final = " + this.PeriodoFinal + Environment.NewLine +
                "Cadena = " + this.Cadena;
        }

        public List<double> Evaluar(int horizonte, int preoperativos, int cierre, List<Operacion> operaciones, List<TipoFormula> tipoformulas)
        {
            List<double> resultado = new List<double>();

            MathParserNet.Parser parser = new MathParserNet.Parser();
            double valor;

            try
            {
                for (int i = 1; i <= horizonte; i++)
                {
                    parser.RemoveAllVariables();

                    parser.AddVariable("Periodo", i);
                    parser.AddVariable("Horizonte", horizonte);
                    parser.AddVariable("PeriodosCierre", cierre);
                    parser.AddVariable("PeriodosPreOperativos", preoperativos);

                    foreach (TipoFormula tipoformula in tipoformulas)
                    {
                        parser.AddVariable(tipoformula.Referencia, tipoformula.Valores[i - 1]);
                    }

                    foreach (Operacion operacion in operaciones)
                    {
                        parser.AddVariable(operacion.Referencia, (i >= operacion.valPeriodoInicial && i <= operacion.valPeriodoFinal) ? operacion.Valores[i - 1] : 0);
                    }

                    this.valPeriodoInicial = parser.SimplifyInt(this.PeriodoInicial, MathParserNet.Parser.RoundingMethods.Round);
                    this.valPeriodoFinal = parser.SimplifyInt(this.PeriodoFinal, MathParserNet.Parser.RoundingMethods.Round);

                    valor = 0;

                    if (i >= this.valPeriodoInicial && i <= this.valPeriodoFinal)
                    {
                        if ((this.Cadena.StartsWith("Tir(") || this.Cadena.StartsWith("Van(") || this.Cadena.StartsWith("Tri(")) && this.Cadena.EndsWith(")"))
                        {
                            valor = Generics.ComplexParse(this.Cadena, operaciones);
                        }
                        else
                        {
                            valor = (i >= this.valPeriodoInicial && i <= this.valPeriodoFinal) ? parser.SimplifyDouble(this.Cadena) : 0;
                            valor = double.IsNaN(valor) ? 0 : valor;
                        }
                    }

                    resultado.Add(valor);
                }
            }

            catch (Exception)
            {
            }

            this.Valores = resultado;
            return resultado;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (TProjContext context = new TProjContext())
            {
                if (context.Operaciones.Any(f => f.Referencia == this.Referencia && f.IdProyecto == this.IdProyecto && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo nombre de referencia en el mismo proyecto.", new string[] { "Referencia" });
                }

                if (context.TipoFormulas.Any(f => f.Referencia == this.Referencia))
                {
                    yield return new ValidationResult("Ya existe un tipo de fórmula con el mismo nombre de referencia.", new string[] { "Referencia" });
                }

                if (context.Operaciones.Any(f => f.Secuencia == this.Secuencia && f.IdProyecto == this.IdProyecto && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe un registro con el mismo número de secuencia en el mismo proyecto.", new string[] { "Secuencia" });
                }

                if (Generics.Reservadas.Contains(this.Referencia))
                {
                    yield return new ValidationResult("Ya existe una palabra reservada con el mismo nombre.", new string[] { "Referencia" });
                }

                MathParserNet.Parser parser = new MathParserNet.Parser();

                parser.AddVariable("Periodo", 5);
                parser.AddVariable("Horizonte", 10);
                parser.AddVariable("PeriodosCierre", 1);
                parser.AddVariable("PeriodosPreOperativos", 1);

                var tipoformulas = context.TipoFormulas;
                foreach (TipoFormula tipoformula in tipoformulas)
                {
                    parser.AddVariable(tipoformula.Referencia, 2);
                }

                var operaciones = context.Operaciones.Where(o => o.IdProyecto == this.IdProyecto && o.Secuencia < this.Secuencia);
                foreach (Operacion operacion in operaciones)
                {
                    parser.AddVariable(operacion.Referencia, 2);
                }

            //  Valida si es Tir o Van
                if (!Generics.Validar(this.Cadena, parser))
                {
                    if (!Generics.TestComplexParse(this.Cadena, operaciones.ToList()))
                    {
                        yield return new ValidationResult("Cadena inválida. La operación solo puede contener referencias a tipos de fórmula.", new string[] { "Cadena" });
                    }
                }

                parser.RemoveVariable("Periodo");

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

        public bool EsSensible(List<Operacion> operacionesInvariantes, List<TipoFormula> tipoformulasInvariantes)
        {
            bool resultado = false;

            MathParserNet.Parser parser = new MathParserNet.Parser();
            parser.AddVariable("Periodo", 5);
            parser.AddVariable("Horizonte", 10);
            parser.AddVariable("PeriodosCierre", 1);
            parser.AddVariable("PeriodosPreOperativos", 1);

            foreach (TipoFormula tipoformula in tipoformulasInvariantes)
            {
                parser.AddVariable(tipoformula.Referencia, 2);
            }

            foreach (Operacion operacion in operacionesInvariantes)
            {
                parser.AddVariable(operacion.Referencia, 2);
            }

            //  Valida si es Tir o Van
            if (!Generics.Validar(this.Cadena, parser))
            {
                if (!Generics.TestComplexParse(this.Cadena, operacionesInvariantes.ToList()))
                {
                    resultado = true;
                }
            }

            return resultado;
        }
    }
}
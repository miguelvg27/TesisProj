using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Areas.Plantilla.Models;
using TesisProj.Models;
using TesisProj.Models.Storage;
using System.Data.Entity;
using System.Xml.Serialization;

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

        [XmlIgnore]
        [ForeignKey("IdElemento")]
        public virtual Elemento Elemento { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Tipo")]
        public int IdTipoFormula { get; set; }

        [XmlIgnore]
        [ForeignKey("IdTipoFormula")]
        public virtual TipoFormula TipoFormula { get; set; }

        [DisplayName("Visible")]
        public bool Visible { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Período inicial")]
        public string PeriodoInicial { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Período final")]
        public string PeriodoFinal { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayName("Tipo de dato")]
        public int IdTipoDato { get; set; }

        [XmlIgnore]
        [ForeignKey("IdTipoDato")]
        public virtual TipoDato TipoDato { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Cadena")]
        public string Cadena { get; set; }

        [DisplayName("Sensible")]
        public bool Sensible { get; set; }

        [DisplayName("Simular")]
        public bool Simular { get; set; }

        [StringLength(2048)]
        public string strValores { get; set; }

        [XmlIgnore]
        public string ListName { get { return Nombre + " (" + Referencia + ")"; } }

        [XmlIgnore]
        public List<double> Valores;

        [XmlIgnore]
        public int valPeriodoInicial { get; set; }

        [XmlIgnore]
        public int valPeriodoFinal { get; set; }

        public Formula() { }

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
            this.IdTipoDato = plantilla.IdTipoDato;
            this.Visible = plantilla.Visible;
        }

        public override string LogValues()
        {
            return "Elemento = " + this.Elemento.Nombre + Environment.NewLine +
                "Nombre = " + this.Nombre + Environment.NewLine +
                "Referencia = " + this.Referencia + Environment.NewLine +
                "Secuencia = " + this.Secuencia + Environment.NewLine +
                "Tipo fórmula = " + this.TipoFormula.Nombre + Environment.NewLine +
                "Tipo dato = " + this.TipoDato.Nombre + Environment.NewLine +
                "Visible = " + this.Visible + Environment.NewLine +
                "Período inicial = " + this.PeriodoInicial + Environment.NewLine +
                "Periodo final = " + this.PeriodoFinal + Environment.NewLine +
                "Cadena = " + this.Cadena;
        }

        public List<double> Evaluar(int horizonte, int preoperativos, int cierre, List<Formula> formulas, List<Parametro> parametros, bool simular = false)
        {
            List<double> resultado = new List<double>();
            MathParserNet.Parser parser = new MathParserNet.Parser();
            formulas.OrderBy(f => f.Secuencia);
            double valor;

            //  Agrego al parser las funciones

            parser.RegisterCustomDoubleFunction("Amortizacion", Generics.Ppmt);
            parser.RegisterCustomDoubleFunction("Intereses", Generics.IPmt);
            parser.RegisterCustomDoubleFunction("Cuota", Generics.Pmt);
            parser.RegisterCustomDoubleFunction("DepreciacionLineal", Generics.Sln);
            parser.RegisterCustomDoubleFunction("DepreciacionAcelerada", Generics.Syn);
            parser.RegisterCustomDoubleFunction("ValorResidual", Generics.ResSln);

            try
            {
                for (int i = 1; i <= horizonte; i++)
                {
                    parser.RemoveAllVariables();

                    parser.AddVariable("Periodo", i);
                    parser.AddVariable("Horizonte", horizonte);
                    parser.AddVariable("PeriodosCierre", cierre);
                    parser.AddVariable("PeriodosPreOperativos", preoperativos);
                    
                    //
                    //  Agrego al parser el Horizonte, Período, los parámetros y las fórmulas de referencia

                    foreach (Parametro parametro in parametros)
                    {
                        var celdas = (simular && parametro.Sensible) ? parametro.CeldasSensibles : parametro.Celdas;
                        parser.AddVariable(parametro.Referencia, (double) celdas.First(c => c.Periodo == (parametro.Constante ? 1 : i)).Valor);
                    }

                    foreach (Formula formula in formulas)
                    {
                        parser.AddVariable(formula.Referencia, (i >= formula.valPeriodoInicial && i <= formula.valPeriodoFinal) ? formula.Valores[i - 1] : 0);
                    }

                    this.valPeriodoInicial = parser.SimplifyInt(this.PeriodoInicial, MathParserNet.Parser.RoundingMethods.Round);
                    this.valPeriodoFinal = parser.SimplifyInt(this.PeriodoFinal, MathParserNet.Parser.RoundingMethods.Round);

                    valor = (i >= this.valPeriodoInicial && i <= this.valPeriodoFinal) ? parser.SimplifyDouble(this.Cadena) : 0;
                    resultado.Add(double.IsNaN(valor) ? 0 : valor);
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

                if (Generics.Reservadas.Contains(this.Referencia))
                {
                    yield return new ValidationResult("Ya existe una palabra reservada con el mismo nombre.", new string[] { "Referencia" });
                }

                Elemento elemento = context.Elementos.Find(this.IdElemento);
                if (context.Formulas.Include(f => f.IdTipoFormula).Include(f => f.Elemento).Any(f => f.IdTipoFormula == this.IdTipoFormula && f.TipoFormula.Unico && f.Elemento.IdProyecto == elemento.IdProyecto && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe una fórmula de este tipo en el proyecto. Dicho tipo de fórmula solo permite una por proyecto.", new string[] { "IdTipoFormula" });
                }

                //  Valida cadena de la fórmula

                MathParserNet.Parser parser = new MathParserNet.Parser();
                parser.AddVariable("Periodo", 5);
                parser.AddVariable("Horizonte", 10);
                parser.AddVariable("PeriodosCierre", 1);
                parser.AddVariable("PeriodosPreOperativos", 1);
                parser.RegisterCustomDoubleFunction("Amortizacion", Generics.Ppmt);
                parser.RegisterCustomDoubleFunction("Intereses", Generics.IPmt);
                parser.RegisterCustomDoubleFunction("Cuota", Generics.Pmt);
                parser.RegisterCustomDoubleFunction("DepreciacionLineal", Generics.Sln);
                parser.RegisterCustomDoubleFunction("DepreciacionAcelerada", Generics.Syn);
                parser.RegisterCustomDoubleFunction("ValorResidual", Generics.ResSln);

                var parametros = context.Parametros.Where(p => p.IdElemento == this.IdElemento);
                foreach (Parametro parametro in parametros)
                {
                    parser.AddVariable(parametro.Referencia, 2);
                }

                var formulas = context.Formulas.Where(p => p.IdElemento == this.IdElemento && p.Secuencia < this.Secuencia);
                foreach (Formula formula in formulas)
                {
                    parser.AddVariable(formula.Referencia, 2);
                }

                if (!Generics.Validar(this.Cadena, parser))
                {
                    yield return new ValidationResult("Cadena inválida. La fórmula solo puede contener los parámetros y las fórmulas con menor secuencia del elemento.", new string[] { "Cadena" });
                }

                //  Validar períodos
                parser.RemoveVariable("Periodo");
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

        public bool EsSensible(List<Formula> formulasInvariantes, List<Parametro> parametrosInvariantes)
        {
            bool resultado = false;

            MathParserNet.Parser parser = new MathParserNet.Parser();
            parser.AddVariable("Periodo", 5);
            parser.AddVariable("Horizonte", 10);
            parser.AddVariable("PeriodosCierre", 1);
            parser.AddVariable("PeriodosPreOperativos", 1);
            parser.RegisterCustomDoubleFunction("Amortizacion", Generics.Ppmt);
            parser.RegisterCustomDoubleFunction("Intereses", Generics.IPmt);
            parser.RegisterCustomDoubleFunction("Cuota", Generics.Pmt);
            parser.RegisterCustomDoubleFunction("DepreciacionLineal", Generics.Sln);
            parser.RegisterCustomDoubleFunction("DepreciacionAcelerada", Generics.Syn);
            parser.RegisterCustomDoubleFunction("ValorResidual", Generics.ResSln);

            foreach (Parametro parametro in parametrosInvariantes)
            {
                parser.AddVariable(parametro.Referencia, 2);
            }

            foreach (Formula formula in formulasInvariantes)
            {
                parser.AddVariable(formula.Referencia, 2);
            }

            if (!Generics.Validar(this.Cadena, parser))
            {
                resultado = true;
            }

            return resultado;
        }

    }
}
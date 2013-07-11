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
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "El campo {0} debe tener un máximo de {1} carácteres.")]
        [DisplayName("Cadena")]
        public string Cadena { get; set; }

        public List<double> Valores;

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
            this.Visible = plantilla.Visible;
        }

        public List<double> Evaluar(int horizonte, List<Formula> formulas, List<Parametro> parametros, bool simular = false)
        {
            List<double> resultado = new List<double>();
            MathParserNet.Parser parser = new MathParserNet.Parser();
            formulas.OrderBy(f => f.Secuencia);
            double valor, pinicial, pfinal;

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
                    
                    foreach (Parametro parametro in parametros)
                    {
                        var celdas = simular ? parametro.CeldasSensibles : parametro.Celdas;
                        parser.AddVariable(parametro.Referencia, (double) celdas.First(c => c.Periodo == (parametro.Constante ? 1 : i)).Valor);
                    }

                    foreach (Formula formula in formulas)
                    {
                        pinicial = parser.SimplifyInt(formula.PeriodoInicial, MathParserNet.Parser.RoundingMethods.Round);
                        pfinal = parser.SimplifyInt(formula.PeriodoFinal, MathParserNet.Parser.RoundingMethods.Round);

                        parser.AddVariable(formula.Referencia, (i >= pinicial && i <= pfinal) ? formula.Cadena : "0");
                    }

                    pinicial = parser.SimplifyInt(this.PeriodoInicial, MathParserNet.Parser.RoundingMethods.Round);
                    pfinal = parser.SimplifyInt(this.PeriodoFinal, MathParserNet.Parser.RoundingMethods.Round);

                    valor = (i >= pinicial && i <= pfinal) ? parser.SimplifyDouble(this.Cadena) : 0;
                    valor = double.IsNaN(valor) ? 0 : valor;
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

                if (Generics.Reservadas.Contains(this.Referencia))
                {
                    yield return new ValidationResult("Ya existe una palabra reservada con el mismo nombre.", new string[] { "Referencia" });
                }

                Elemento elemento = context.Elementos.Find(this.IdElemento);

                if (context.Formulas.Include("TipoFormula").Include("Elemento").Any(f => f.IdTipoFormula == this.IdTipoFormula && f.TipoFormula.Unico && f.Elemento.IdProyecto == elemento.IdProyecto && (this.Id > 0 ? f.Id != this.Id : true)))
                {
                    yield return new ValidationResult("Ya existe una fórmula de este tipo en el proyecto. Dicho tipo de fórmula solo permite una por proyecto.", new string[] { "IdTipoFormula" });
                }

                //  Valida cadena de la fórmula

                bool cadenavalida = true;
                double testvalue = 0;
                MathParserNet.Parser parser = new MathParserNet.Parser();
                var parametros = context.Parametros.Where(p => p.IdElemento == this.IdElemento);
                var formulas = context.Formulas.Where(p => p.IdElemento == this.IdElemento && p.Secuencia < this.Secuencia);

                foreach (Parametro parametro in parametros)
                {
                    parser.AddVariable(parametro.Referencia, 2);
                }

                foreach (Formula formula in formulas)
                {
                    parser.AddVariable(formula.Referencia, 2);
                }

                parser.AddVariable("Periodo", 5);
                parser.AddVariable("Horizonte", 10);
                parser.RegisterCustomDoubleFunction("Amortizacion", Generics.Ppmt);
                parser.RegisterCustomDoubleFunction("Intereses", Generics.IPmt);
                parser.RegisterCustomDoubleFunction("Cuota", Generics.Pmt);
                parser.RegisterCustomDoubleFunction("DepreciacionLineal", Generics.Sln);
                parser.RegisterCustomDoubleFunction("DepreciacionAcelerada", Generics.Syn);
                parser.RegisterCustomDoubleFunction("ValorResidual", Generics.ResSln);

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

                try
                {
                    testvalue = parser.SimplifyDouble(this.PeriodoInicial);
                }
                catch (Exception)
                {
                    cadenavalida = false;
                }

                if (!cadenavalida)
                {
                    yield return new ValidationResult("Cadena inválida. La fórmula solo puede contener los parámetros y las fórmulas con menor secuencia del elemento.", new string[] { "PeriodoInicial" });
                }

                cadenavalida = true;

                try
                {
                    testvalue = parser.SimplifyDouble(this.PeriodoFinal);
                }
                catch (Exception)
                {
                    cadenavalida = false;
                }

                if (!cadenavalida)
                {
                    yield return new ValidationResult("Cadena inválida. La fórmula solo puede contener los parámetros y las fórmulas con menor secuencia del elemento.", new string[] { "PeriodoFinal" });
                }
            }
        }
    }
}
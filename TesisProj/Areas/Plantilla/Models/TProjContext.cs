using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TesisProj.Areas.Plantilla.Models;

namespace TesisProj.Models.Storage
{
    public partial class TProjContext : DbContext
    {
        public DbSet<TipoElemento> TipoElementos { get; set; }
        public DbSet<TipoFormula> TipoFormulas { get; set; }
        public DbSet<TipoParametro> TipoParametros { get; set; }
        public DbSet<Parametro> Parametros { get; set; }
        public DbSet<PlantillaElemento> PlantillaElementos { get; set; }
        public DbSet<Formula> Formulas { get; set; }
        public DbSet<SalidaElemento> SalidaElementos { get; set; }
        public DbSet<PlantillaProyecto> PlantillaProyectos { get; set; }
        public DbSet<PlantillaElementoProyecto> PlantillaElementoProyectos { get; set; }

        public DbRequester<TipoElemento> TipoElementosRequester { get; set; }
        public DbRequester<TipoFormula> TipoFormulasRequester { get; set; }
        public DbRequester<TipoParametro> TipoParametrosRequester { get; set; }
        public DbRequester<Parametro> ParametrosRequester { get; set; }
        public DbRequester<PlantillaElemento> PlantillaElementosRequester { get; set; }
        public DbRequester<Formula> FormulasRequester { get; set; }
        public DbRequester<SalidaElemento> SalidaElementosRequester { get; set; }
        public DbRequester<PlantillaProyecto> PlantillaProyectosRequester { get; set; }
        public DbRequester<PlantillaElementoProyecto> PlantillaElementoProyectosRequester { get; set; }

        public void RegistrarTablasPlantilla()
        {
            TipoElementosRequester = new DbRequester<TipoElemento>(this, TipoElementos);
            TipoFormulasRequester = new DbRequester<TipoFormula>(this, TipoFormulas);
            TipoParametrosRequester = new DbRequester<TipoParametro>(this, TipoParametros);
            ParametrosRequester = new DbRequester<Parametro>(this, Parametros);
            PlantillaElementosRequester = new DbRequester<PlantillaElemento>(this, PlantillaElementos);
            FormulasRequester = new DbRequester<Formula>(this, Formulas);
            SalidaElementosRequester = new DbRequester<SalidaElemento>(this, SalidaElementos);
            PlantillaProyectosRequester = new DbRequester<PlantillaProyecto>(this, PlantillaProyectos);
            PlantillaElementoProyectosRequester = new DbRequester<PlantillaElementoProyecto>(this, PlantillaElementoProyectos);
        }

        public void SeedPlantilla()
        {
            SeedTipoElementos();
            SeedTipoFormulas();
            SeedTipoParametros();
            SeedPlantillaElementos();
            SeedParametros();
            SeedFormulas();
            SeedSalidaElementos();
            SeedPlantillaProyectos();
        }

        public void SeedTipoElementos()
        {
            TipoElementosRequester.AddElement(new TipoElemento { Id = 1, Nombre = "Activo fijo" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 2, Nombre = "Activo intangible" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 3, Nombre = "Gasto" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 4, Nombre = "Operativo" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 5, Nombre = "Financiamiento" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 6, Nombre = "Impuesto" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 7, Nombre = "Participación" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 8, Nombre = "Inversión" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 9, Nombre = "Otros" });
        }
        
        public void SeedTipoFormulas()
        {
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 1, Nombre = "Depreciación", IdTipoElemento = 1 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 2, Nombre = "Amortización", IdTipoElemento = 2 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 3, Nombre = "Gasto operativo", IdTipoElemento = 3 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 4, Nombre = "Gasto administrativo", IdTipoElemento = 3 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 5, Nombre = "Gasto de venta", IdTipoElemento = 3 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 6, Nombre = "Gasto financiero", IdTipoElemento = 3 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 7, Nombre = "Ventas", IdTipoElemento = 4 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 8, Nombre = "Costos", IdTipoElemento = 4 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id =  9, Nombre = "Saldo inicial", IdTipoElemento = 5 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 10, Nombre = "Saldo final", IdTipoElemento = 5 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 11, Nombre = "Amortización", IdTipoElemento = 5 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 12, Nombre = "Cuota", IdTipoElemento = 5 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 13, Nombre = "Intereses", IdTipoElemento = 5 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 14, Nombre = "Renta", IdTipoElemento = 6 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 15, Nombre = "Operativo", IdTipoElemento = 6 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 16, Nombre = "Otros ingresos", IdTipoElemento = 9 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 17, Nombre = "Otros egresos", IdTipoElemento = 9 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 18, Nombre = "Ingresos extraordinarios", IdTipoElemento = 9 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 19, Nombre = "Egresos extraordinarios", IdTipoElemento = 9 });


        }

        public void SeedTipoParametros()
        {
            TipoParametrosRequester.AddElement(new TipoParametro { Id = 1, Nombre = "Real" });
            TipoParametrosRequester.AddElement(new TipoParametro { Id = 2, Nombre = "Entero" });
            TipoParametrosRequester.AddElement(new TipoParametro { Id = 3, Nombre = "Porcentaje" });
            TipoParametrosRequester.AddElement(new TipoParametro { Id = 4, Nombre = "Monetario" });
        }

        public void SeedPlantillaElementos()
        {
            PlantillaElementosRequester.AddElement(new PlantillaElemento { Id = 1, Nombre = "Activo fijo con depreciación lineal", IdTipoElemento = 1 });
            PlantillaElementosRequester.AddElement(new PlantillaElemento { Id = 2, Nombre = "Venta de mineral", IdTipoElemento = 4 });
            PlantillaElementosRequester.AddElement(new PlantillaElemento { Id = 3, Nombre = "Préstamo con cuotas constantes y períodos de gracia", IdTipoElemento = 5 });
        }

        public void SeedParametros()
        {
            ParametrosRequester.AddElement(new Parametro { Id = 1, Nombre = "Período inicial", Referencia = "PeriodoInicial", IdTipoParametro = 2, IdPlantillaElemento = 1 });
            ParametrosRequester.AddElement(new Parametro { Id = 2, Nombre = "Período final", Referencia = "PeriodoFinal", IdTipoParametro = 2, IdPlantillaElemento = 1 });
            ParametrosRequester.AddElement(new Parametro { Id = 3, Nombre = "Vida útil (períodos)", Referencia = "VidaUtil", IdTipoParametro = 2, IdPlantillaElemento = 1 });
            ParametrosRequester.AddElement(new Parametro { Id = 4, Nombre = "Valor inicial (US$)", Referencia = "ValorInicial", IdTipoParametro = 4, IdPlantillaElemento = 1 });

            ParametrosRequester.AddElement(new Parametro { Id = 5, Nombre = "Período inicial", Referencia = "PeriodoInicial", IdTipoParametro = 2, IdPlantillaElemento = 2 });
            ParametrosRequester.AddElement(new Parametro { Id = 6, Nombre = "Período final", Referencia = "PeriodoFinal", IdTipoParametro = 2, IdPlantillaElemento = 2 });
            ParametrosRequester.AddElement(new Parametro { Id = 7, Nombre = "Tamaño de mina (kTM/período)", Referencia = "TamanoMina", IdTipoParametro = 1, IdPlantillaElemento = 2 });
            ParametrosRequester.AddElement(new Parametro { Id = 8, Nombre = "Ley mineral (oz/TM)", Referencia = "LeyMineral", IdTipoParametro = 1, IdPlantillaElemento = 2 });
            ParametrosRequester.AddElement(new Parametro { Id = 9, Nombre = "Precio metal (US$/oz)", Referencia = "PrecioMetal", IdTipoParametro = 4, IdPlantillaElemento = 2 });
            ParametrosRequester.AddElement(new Parametro { Id = 10, Nombre = "Recuperación", Referencia = "Recuperacion", IdTipoParametro = 3, IdPlantillaElemento = 2 });
            ParametrosRequester.AddElement(new Parametro { Id = 11, Nombre = "Costo de producción (US$/TM)", Referencia = "CostoProduccion", IdTipoParametro = 4, IdPlantillaElemento = 2 });

            ParametrosRequester.AddElement(new Parametro { Id = 12, Nombre = "Período inicial", Referencia = "PeriodoInicial", IdTipoParametro = 2, IdPlantillaElemento = 3 });
            ParametrosRequester.AddElement(new Parametro { Id = 13, Nombre = "Período final", Referencia = "PeriodoFinal", IdTipoParametro = 2, IdPlantillaElemento = 3 });
            ParametrosRequester.AddElement(new Parametro { Id = 14, Nombre = "Valor inicial (US$)", Referencia = "ValorInicial", IdTipoParametro = 4, IdPlantillaElemento = 3 });
            ParametrosRequester.AddElement(new Parametro { Id = 15, Nombre = "Períodos de gracia", Referencia = "PeriodosGracia", IdTipoParametro = 2, IdPlantillaElemento = 3 });
            ParametrosRequester.AddElement(new Parametro { Id = 16, Nombre = "Tasa", Referencia = "Tasa", IdTipoParametro = 3, IdPlantillaElemento = 3 });
            ParametrosRequester.AddElement(new Parametro { Id = 17, Nombre = "Plazo", Referencia = "Plazo", IdTipoParametro = 2, IdPlantillaElemento = 3 });

        }

        public void SeedFormulas()
        {
            FormulasRequester.AddElement(new Formula { Id = 1, Nombre = "Depreciación lineal", Referencia = "DepreciacionLineal", Secuencia = 1, IdTipoFormula = 1, IdPlantillaElemento = 1, PeriodoInicial = "PeriodoInicial", PeriodoFinal = "PeriodoFinal", Cadena = "ValorInicial/VidaUtil" });
        }

        public void SeedSalidaElementos()
        {
            SalidaElementosRequester.AddElement(new SalidaElemento { Id = 1, IdFormula = 1, IdPlantillaElemento = 1, Secuencia = 1, Nombre = "Depreciación" });
        }

        public void SeedPlantillaProyectos()
        {
            PlantillaProyectosRequester.AddElement(new PlantillaProyecto { Id = 1, Nombre = "Proyecto genérico" });
        }
    }
}
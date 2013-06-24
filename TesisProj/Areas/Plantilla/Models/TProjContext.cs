using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TesisProj.Areas.Plantilla.Models;
using TesisProj.Areas.Modelo.Models;

namespace TesisProj.Models.Storage
{
    public partial class TProjContext : DbContext
    {
        public DbSet<TipoElemento> TipoElementos { get; set; }
        public DbSet<TipoFormula> TipoFormulas { get; set; }
        public DbSet<TipoParametro> TipoParametros { get; set; }
        public DbSet<PlantillaParametro> PlantillaParametros { get; set; }
        public DbSet<PlantillaElemento> PlantillaElementos { get; set; }
        public DbSet<PlantillaFormula> PlantillaFormulas { get; set; }
        public DbSet<PlantillaSalidaElemento> PlantillaSalidaElementos { get; set; }
        public DbSet<PlantillaProyecto> PlantillaProyectos { get; set; }
        public DbSet<PlantillaElementoProyecto> PlantillaElementoProyectos { get; set; }
        public DbSet<PlantillaSalidaProyecto> PlantillaSalidaProyectos { get; set; }

        public DbRequester<TipoElemento> TipoElementosRequester { get; set; }
        public DbRequester<TipoFormula> TipoFormulasRequester { get; set; }
        public DbRequester<TipoParametro> TipoParametrosRequester { get; set; }
        public DbRequester<PlantillaParametro> PlantillaParametrosRequester { get; set; }
        public DbRequester<PlantillaElemento> PlantillaElementosRequester { get; set; }
        public DbRequester<PlantillaFormula> PlantillaFormulasRequester { get; set; }
        public DbRequester<PlantillaSalidaElemento> PlantillaSalidaElementosRequester { get; set; }
        public DbRequester<PlantillaProyecto> PlantillaProyectosRequester { get; set; }
        public DbRequester<PlantillaElementoProyecto> PlantillaElementoProyectosRequester { get; set; }
        public DbRequester<PlantillaSalidaProyecto> PlantillaSalidaProyectosDbRequester { get; set; }

        public void RegistrarTablasPlantilla()
        {
            TipoElementosRequester = new DbRequester<TipoElemento>(this, TipoElementos);
            TipoFormulasRequester = new DbRequester<TipoFormula>(this, TipoFormulas);
            TipoParametrosRequester = new DbRequester<TipoParametro>(this, TipoParametros);
            PlantillaParametrosRequester = new DbRequester<PlantillaParametro>(this, PlantillaParametros);
            PlantillaElementosRequester = new DbRequester<PlantillaElemento>(this, PlantillaElementos);
            PlantillaFormulasRequester = new DbRequester<PlantillaFormula>(this, PlantillaFormulas);
            PlantillaSalidaElementosRequester = new DbRequester<PlantillaSalidaElemento>(this, PlantillaSalidaElementos);
            PlantillaProyectosRequester = new DbRequester<PlantillaProyecto>(this, PlantillaProyectos);
            PlantillaElementoProyectosRequester = new DbRequester<PlantillaElementoProyecto>(this, PlantillaElementoProyectos);
            PlantillaSalidaProyectosDbRequester = new DbRequester<PlantillaSalidaProyecto>(this, PlantillaSalidaProyectos);
        }

        public void SeedPlantilla()
        {
            SeedTipoElementos();
            SeedTipoFormulas();
            SeedTipoParametros();
            SeedPlantillaElementos();
            SeedPlantillaParametros();
            SeedPlantillaFormulas();
            SeedPlantillaSalidaElementos();
            SeedPlantillaProyectos();
            SeedPlantillaElementoProyectos();
            SeedPlantillaSalidaProyectos();
        }

        public void SeedTipoElementos()
        {
            TipoElementosRequester.AddElement(new TipoElemento { Id = 1, Nombre = "Activo fijo", NombrePlural = "Activos fijos" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 2, Nombre = "Activo intangible", NombrePlural = "Activos intangibles" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 10, Nombre = "Gasto operativo", NombrePlural = "Gastos operativos" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 11, Nombre = "Gasto administrativo", NombrePlural = "Gastos administrativos" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 12, Nombre = "Gasto financiero", NombrePlural = "Gastos financieros" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 4, Nombre = "Operativo", NombrePlural = "Operación" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 5, Nombre = "Financiamiento", NombrePlural = "Financiamientos" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 6, Nombre = "Impuesto", NombrePlural = "Impuestos" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 7, Nombre = "Participación", NombrePlural = "Participaciones" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 8, Nombre = "Inversión", NombrePlural = "Inversiones" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 9, Nombre = "Otros", NombrePlural = "Otros" });
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

        public void SeedPlantillaParametros()
        {
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 1, Nombre = "Período inicial", Referencia = "PeriodoInicial", IdTipoParametro = 2, IdPlantillaElemento = 1 });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 2, Nombre = "Período final", Referencia = "PeriodoFinal", IdTipoParametro = 2, IdPlantillaElemento = 1 });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 3, Nombre = "Vida útil (períodos)", Referencia = "VidaUtil", IdTipoParametro = 2, IdPlantillaElemento = 1 });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 4, Nombre = "Valor inicial (US$)", Referencia = "ValorInicial", IdTipoParametro = 4, IdPlantillaElemento = 1 });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 5, Nombre = "Período inicial", Referencia = "PeriodoInicial", IdTipoParametro = 2, IdPlantillaElemento = 2 });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 6, Nombre = "Período final", Referencia = "PeriodoFinal", IdTipoParametro = 2, IdPlantillaElemento = 2 });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 7, Nombre = "Tamaño de mina (kTM/período)", Referencia = "TamanoMina", IdTipoParametro = 1, IdPlantillaElemento = 2 });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 8, Nombre = "Ley mineral (oz/TM)", Referencia = "LeyMineral", IdTipoParametro = 1, IdPlantillaElemento = 2 });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 9, Nombre = "Precio metal (US$/oz)", Referencia = "PrecioMetal", IdTipoParametro = 4, IdPlantillaElemento = 2 });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 10, Nombre = "Recuperación", Referencia = "Recuperacion", IdTipoParametro = 3, IdPlantillaElemento = 2 });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 11, Nombre = "Costo de producción (US$/TM)", Referencia = "CostoProduccion", IdTipoParametro = 4, IdPlantillaElemento = 2 });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 12, Nombre = "Período inicial", Referencia = "PeriodoInicial", IdTipoParametro = 2, IdPlantillaElemento = 3 });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 13, Nombre = "Período final", Referencia = "PeriodoFinal", IdTipoParametro = 2, IdPlantillaElemento = 3 });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 14, Nombre = "Valor inicial (US$)", Referencia = "ValorInicial", IdTipoParametro = 4, IdPlantillaElemento = 3 });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 15, Nombre = "Períodos de gracia", Referencia = "PeriodosGracia", IdTipoParametro = 2, IdPlantillaElemento = 3 });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 16, Nombre = "Tasa", Referencia = "Tasa", IdTipoParametro = 3, IdPlantillaElemento = 3 });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 17, Nombre = "Plazo", Referencia = "Plazo", IdTipoParametro = 2, IdPlantillaElemento = 3 });

        }

        public void SeedPlantillaFormulas()
        {
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 1, Nombre = "Depreciación lineal", Referencia = "DepreciacionLineal", Secuencia = 1, IdTipoFormula = 1, IdPlantillaElemento = 1, PeriodoInicial = "PeriodoInicial", PeriodoFinal = "PeriodoFinal", Cadena = "ValorInicial/VidaUtil" });
        }

        public void SeedPlantillaSalidaElementos()
        {
            PlantillaSalidaElementosRequester.AddElement(new PlantillaSalidaElemento { Id = 1, IdFormula = 1, IdPlantillaElemento = 1, Secuencia = 1, Nombre = "Depreciación" });
        }

        public void SeedPlantillaProyectos()
        {
            PlantillaProyectosRequester.AddElement(new PlantillaProyecto { Id = 1, Nombre = "Proyecto genérico" });
        }

        public void SeedPlantillaElementoProyectos()
        {
            PlantillaElementoProyectosRequester.AddElement(new PlantillaElementoProyecto { Id = 1, IdElemento = 1, IdProyecto = 1 });
            PlantillaElementoProyectosRequester.AddElement(new PlantillaElementoProyecto { Id = 1, IdElemento = 2, IdProyecto = 1 });
            PlantillaElementoProyectosRequester.AddElement(new PlantillaElementoProyecto { Id = 1, IdElemento = 3, IdProyecto = 1 });
        }

        public void SeedPlantillaSalidaProyectos()
        {
            PlantillaSalidaProyectosDbRequester.AddElement(new PlantillaSalidaProyecto { Id = 1, Nombre = "Gastos", Secuencia = 1, IdPlantillaProyecto = 1, Cadena = "DepreciacionLineal" });
        }
    }
}
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
            TipoElementosRequester.AddElement(new TipoElemento { Id = 3, Nombre = "Gasto operativo", NombrePlural = "Gastos operativos" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 4, Nombre = "Gasto administrativo", NombrePlural = "Gastos administrativos" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 5, Nombre = "Gasto de venta", NombrePlural = "Gastos financieros" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 6, Nombre = "Gasto financiero", NombrePlural = "Gastos financieros" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 7, Nombre = "Operativo", NombrePlural = "Operación" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 8, Nombre = "Financiamiento", NombrePlural = "Financiamientos" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 9, Nombre = "Impuesto", NombrePlural = "Impuestos" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 10, Nombre = "Participación", NombrePlural = "Participaciones" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 11, Nombre = "Inversión", NombrePlural = "Inversiones" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 12, Nombre = "Otros", NombrePlural = "Otros" });
        }
        
        public void SeedTipoFormulas()
        {
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 1, Nombre = "Depreciación", IdTipoElemento = 1 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 2, Nombre = "Inversión inicial", IdTipoElemento = 1 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 3, Nombre = "Valor residual", IdTipoElemento = 1 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 4, Nombre = "Período final", IdTipoElemento = 1 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 5, Nombre = "Amortización", IdTipoElemento = 2 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 6, Nombre = "Inversión inicial", IdTipoElemento = 2 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 7, Nombre = "Gasto operativo", IdTipoElemento = 3 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 8, Nombre = "Gasto administrativo", IdTipoElemento = 4 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 9, Nombre = "Gasto de venta", IdTipoElemento = 5 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 10, Nombre = "Gasto financiero", IdTipoElemento = 6 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 11, Nombre = "Ventas", IdTipoElemento = 7 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 12, Nombre = "Costos", IdTipoElemento = 7 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 13, Nombre = "Amortización", IdTipoElemento = 8 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 14, Nombre = "Cuota", IdTipoElemento = 8 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 15, Nombre = "Intereses", IdTipoElemento = 8 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 16, Nombre = "Préstamo", IdTipoElemento = 8 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 17, Nombre = "Renta", IdTipoElemento = 9 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 18, Nombre = "Operativo", IdTipoElemento = 9 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 19, Nombre = "Participación", IdTipoElemento = 10 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 20, Nombre = "Inversión", IdTipoElemento = 11 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 21, Nombre = "Otros ingresos", IdTipoElemento = 12 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 22, Nombre = "Otros egresos", IdTipoElemento = 12 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 23, Nombre = "Ingresos extraordinarios", IdTipoElemento = 12 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 23, Nombre = "Egresos extraordinarios", IdTipoElemento = 12 });
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
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 1, Nombre = "Período inicial", Referencia = "periodoInicial", IdTipoParametro = 2, IdPlantillaElemento = 1, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 2, Nombre = "Vida útil (períodos)", Referencia = "vidaUtil", IdTipoParametro = 2, IdPlantillaElemento = 1, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 3, Nombre = "Valor inicial (US$)", Referencia = "valorInicial", IdTipoParametro = 4, IdPlantillaElemento = 1, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 4, Nombre = "Período inicial", Referencia = "periodoInicial", IdTipoParametro = 2, IdPlantillaElemento = 2, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 5, Nombre = "Período final", Referencia = "periodoFinal", IdTipoParametro = 2, IdPlantillaElemento = 2, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 6, Nombre = "Tamaño de mina (kTM/período)", Referencia = "tamanoMina", IdTipoParametro = 1, IdPlantillaElemento = 2, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 7, Nombre = "Ley mineral (oz/TM)", Referencia = "leyMineral", IdTipoParametro = 1, IdPlantillaElemento = 2, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 8, Nombre = "Precio metal (US$/oz)", Referencia = "precioMetal", IdTipoParametro = 4, IdPlantillaElemento = 2, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 9, Nombre = "Recuperación", Referencia = "recuperacion", IdTipoParametro = 3, IdPlantillaElemento = 2, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 10, Nombre = "Costo de producción (US$/TM)", Referencia = "costoProduccion", IdTipoParametro = 4, IdPlantillaElemento = 2, Constante = false });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 11, Nombre = "Período inicial", Referencia = "periodoInicial", IdTipoParametro = 2, IdPlantillaElemento = 3, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 12, Nombre = "Período final", Referencia = "periodoFinal", IdTipoParametro = 2, IdPlantillaElemento = 3, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 13, Nombre = "Valor inicial (US$)", Referencia = "valorInicial", IdTipoParametro = 4, IdPlantillaElemento = 3, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 14, Nombre = "Períodos de gracia", Referencia = "periodosGracia", IdTipoParametro = 2, IdPlantillaElemento = 3, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 15, Nombre = "Tasa", Referencia = "tasa", IdTipoParametro = 3, IdPlantillaElemento = 3, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 16, Nombre = "Plazo", Referencia = "plazo", IdTipoParametro = 2, IdPlantillaElemento = 3, Constante = true });

        }

        public void SeedPlantillaFormulas()
        {
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 1, Nombre = "Período final", Referencia = "periodoFinal", Secuencia = 1, IdTipoFormula = 4, IdPlantillaElemento = 1, PeriodoInicial = "periodoInicial", PeriodoFinal = "Horizonte", Cadena = "periodoInicial + vidaUtil - 1", Visible = false });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 2, Nombre = "Depreciación lineal", Referencia = "depreciacionLineal", Secuencia = 2, IdTipoFormula = 1, IdPlantillaElemento = 1, PeriodoInicial = "periodoInicial", PeriodoFinal = "periodoFinal", Cadena = "DepreciacionLineal(valorInicial, vidaUtil)", Visible = true });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 3, Nombre = "Inversión inicial", Referencia = "inversionInicial", Secuencia = 3, IdTipoFormula = 2, IdPlantillaElemento = 1, PeriodoInicial = "periodoInicial", PeriodoFinal = "periodoInicial", Cadena = "valorInicial", Visible = true });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 4, Nombre = "Valor residual", Referencia = "valorResidual", Secuencia = 4, IdTipoFormula = 3, IdPlantillaElemento = 1, PeriodoInicial = "Horizonte", PeriodoFinal = "Horizonte", Cadena = "ValorResidual(valorInicial, vidaUtil, periodoInicial, Horizonte)", Visible = true });
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
            PlantillaSalidaProyectosDbRequester.AddElement(new PlantillaSalidaProyecto { Id = 1, Nombre = "Gastos", Secuencia = 1, IdPlantillaProyecto = 1, Cadena = "depreciacionLineal" });
        }
    }
}
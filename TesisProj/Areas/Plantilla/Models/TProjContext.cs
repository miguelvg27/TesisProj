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
            SeedPlantillaProyectos();
            SeedPlantillaElementoProyectos();
            SeedPlantillaSalidaProyectos();
        }

        public void SeedTipoElementos()
        {
            TipoElementosRequester.AddElement(new TipoElemento { Id = 1, Nombre = "Activo fijo", NombrePlural = "Activos fijos" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 2, Nombre = "Activo intangible", NombrePlural = "Activos intangibles" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 3, Nombre = "Operativo", NombrePlural = "Operación" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 4, Nombre = "Gasto", NombrePlural = "Gastos" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 5, Nombre = "Financiamiento", NombrePlural = "Financiamientos" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 6, Nombre = "Impuesto", NombrePlural = "Impuestos" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 7, Nombre = "Participación", NombrePlural = "Participaciones" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 8, Nombre = "Inversión", NombrePlural = "Inversiones" });
            TipoElementosRequester.AddElement(new TipoElemento { Id = 9, Nombre = "Otros", NombrePlural = "Otros" });
        }
        
        public void SeedTipoFormulas()
        {
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 1, Nombre = "Depreciación", IdTipoElemento = 1 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 2, Nombre = "Inversión inicial", IdTipoElemento = 1 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 3, Nombre = "Valor residual", IdTipoElemento = 1 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 4, Nombre = "Período final", IdTipoElemento = 1 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 5, Nombre = "Amortización", IdTipoElemento = 2 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 6, Nombre = "Inversión inicial", IdTipoElemento = 2 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 7, Nombre = "Período final", IdTipoElemento = 2 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id =  8, Nombre = "Ventas", IdTipoElemento = 3 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id =  9, Nombre = "Costos", IdTipoElemento = 3 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 10, Nombre = "Valor unitario", IdTipoElemento = 3 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 11, Nombre = "Gasto administrativo", IdTipoElemento = 4 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 12, Nombre = "Gasto de venta", IdTipoElemento = 4 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 13, Nombre = "Amortización", IdTipoElemento = 5 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 14, Nombre = "Cuota", IdTipoElemento = 5 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 15, Nombre = "Intereses", IdTipoElemento = 5 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 16, Nombre = "Préstamo", IdTipoElemento = 5 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 17, Nombre = "Renta", IdTipoElemento = 6 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 18, Nombre = "Operativo", IdTipoElemento = 6 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 19, Nombre = "Participación", IdTipoElemento = 7 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 20, Nombre = "Inversión", IdTipoElemento = 8 });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 21, Nombre = "Otros ingresos", IdTipoElemento = 9 });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 22, Nombre = "Otros egresos", IdTipoElemento = 9 });
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
            PlantillaElementosRequester.AddElement(new PlantillaElemento { Id = 2, Nombre = "Activo intangible genérico", IdTipoElemento = 2 });
            PlantillaElementosRequester.AddElement(new PlantillaElemento { Id = 3, Nombre = "Operación con un metal", IdTipoElemento = 3 });
            PlantillaElementosRequester.AddElement(new PlantillaElemento { Id = 4, Nombre = "Gasto administrativo estándar", IdTipoElemento = 4 });
            PlantillaElementosRequester.AddElement(new PlantillaElemento { Id = 5, Nombre = "Préstamo de cuota fija con períodos de gracia", IdTipoElemento = 5 });
            PlantillaElementosRequester.AddElement(new PlantillaElemento { Id = 6, Nombre = "Impuesto a la renta", IdTipoElemento = 6 });
            PlantillaElementosRequester.AddElement(new PlantillaElemento { Id = 7, Nombre = "Regalías mineras", IdTipoElemento = 6 });
            PlantillaElementosRequester.AddElement(new PlantillaElemento { Id = 8, Nombre = "Impuesto especial a la minería", IdTipoElemento = 6 });
            PlantillaElementosRequester.AddElement(new PlantillaElemento { Id = 9, Nombre = "Participación de los trabajadores", IdTipoElemento = 7 });
            PlantillaElementosRequester.AddElement(new PlantillaElemento { Id = 10, Nombre = "Inversión estándar", IdTipoElemento = 8 });
            PlantillaElementosRequester.AddElement(new PlantillaElemento { Id = 11, Nombre = "Otro ingreso", IdTipoElemento = 9 });
        }

        public void SeedPlantillaParametros()
        {
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 1, Nombre = "Período inicial de depreciación",    Referencia = "periodoInicial",  IdTipoParametro = 2, IdPlantillaElemento = 1, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 2, Nombre = "Vida útil (períodos)",               Referencia = "vidaUtil",        IdTipoParametro = 2, IdPlantillaElemento = 1, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 3, Nombre = "Valor inicial (US$)",                Referencia = "valorInicial",    IdTipoParametro = 4, IdPlantillaElemento = 1, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 4, Nombre = "Período inicial de amortización",    Referencia = "periodoInicial",  IdTipoParametro = 2, IdPlantillaElemento = 2, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 5, Nombre = "Vida útil (períodos)",               Referencia = "vidaUtil",        IdTipoParametro = 2, IdPlantillaElemento = 2, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 6, Nombre = "Valor inicial (US$)",                Referencia = "valorInicial",    IdTipoParametro = 4, IdPlantillaElemento = 2, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id =  7, Nombre = "Período inicial",               Referencia = "periodoInicial",  IdTipoParametro = 2, IdPlantillaElemento = 3, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id =  8, Nombre = "Período final",                 Referencia = "periodoFinal",    IdTipoParametro = 2, IdPlantillaElemento = 3, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id =  9, Nombre = "Tamaño de mina (kTM/período)",  Referencia = "tamanoMina",      IdTipoParametro = 1, IdPlantillaElemento = 3, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 10, Nombre = "Ley mineral (oz/TM)",           Referencia = "ley",             IdTipoParametro = 1, IdPlantillaElemento = 3, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 11, Nombre = "Precio metal (US$/oz)",         Referencia = "precioMetal",     IdTipoParametro = 4, IdPlantillaElemento = 3, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 12, Nombre = "Recuperación",                  Referencia = "recuperacion",    IdTipoParametro = 3, IdPlantillaElemento = 3, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 13, Nombre = "Costo de producción (US$/TM)",  Referencia = "costoProduccion", IdTipoParametro = 4, IdPlantillaElemento = 3, Constante = false });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 14, Nombre = "Monto (US$)", Referencia = "monto", IdTipoParametro = 4, IdPlantillaElemento = 4, Constante = false });
            
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 15, Nombre = "Período",                Referencia = "periodoInicial",  IdTipoParametro = 2, IdPlantillaElemento = 5, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 16, Nombre = "Valor inicial (US$)",    Referencia = "valorInicial",    IdTipoParametro = 4, IdPlantillaElemento = 5, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 17, Nombre = "Períodos de gracia",     Referencia = "periodosGracia",  IdTipoParametro = 2, IdPlantillaElemento = 5, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 18, Nombre = "Tasa",                   Referencia = "tasa",            IdTipoParametro = 3, IdPlantillaElemento = 5, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 19, Nombre = "Plazo",                  Referencia = "plazo",           IdTipoParametro = 2, IdPlantillaElemento = 5, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 20, Nombre = "Tasa", Referencia = "tasa", IdTipoParametro = 3, IdPlantillaElemento = 6, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 21, Nombre = "Tasa", Referencia = "tasa", IdTipoParametro = 3, IdPlantillaElemento = 7, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 22, Nombre = "Tasa", Referencia = "tasa", IdTipoParametro = 3, IdPlantillaElemento = 8, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 23, Nombre = "Tasa", Referencia = "tasa", IdTipoParametro = 3, IdPlantillaElemento = 9, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 15, Nombre = "Período", Referencia = "periodoInicial", IdTipoParametro = 2, IdPlantillaElemento = 10, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 24, Nombre = "Aporte", Referencia = "aporte", IdTipoParametro = 4, IdPlantillaElemento = 10, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 25, Nombre = "Retorno esperado (K)", Referencia = "retorno", IdTipoParametro = 3, IdPlantillaElemento = 10, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 26, Nombre = "Período inicial", Referencia = "periodoInicial", IdTipoParametro = 2, IdPlantillaElemento = 11, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 27, Nombre = "Período final", Referencia = "periodoFinal", IdTipoParametro = 2, IdPlantillaElemento = 11, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Id = 28, Nombre = "Otro ingreso", Referencia = "otroingreso", IdTipoParametro = 4, IdPlantillaElemento = 11, Constante = false });

        }

        public void SeedPlantillaFormulas()
        {
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 1, Nombre = "Período final", Referencia = "periodoFinal", Secuencia = 1, IdTipoFormula = 4, IdPlantillaElemento = 1, PeriodoInicial = "periodoInicial", PeriodoFinal = "Horizonte", Cadena = "periodoInicial + vidaUtil - 1", Visible = false });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 2, Nombre = "Depreciación lineal", Referencia = "depreciacionLineal", Secuencia = 2, IdTipoFormula = 1, IdPlantillaElemento = 1, PeriodoInicial = "periodoInicial", PeriodoFinal = "periodoFinal", Cadena = "DepreciacionLineal(valorInicial, vidaUtil)", Visible = true });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 3, Nombre = "Inversión inicial", Referencia = "inversionInicial", Secuencia = 3, IdTipoFormula = 2, IdPlantillaElemento = 1, PeriodoInicial = "periodoInicial - 1", PeriodoFinal = "periodoInicial - 1", Cadena = "valorInicial", Visible = true });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 4, Nombre = "Valor residual", Referencia = "valorResidual", Secuencia = 4, IdTipoFormula = 3, IdPlantillaElemento = 1, PeriodoInicial = "Horizonte", PeriodoFinal = "Horizonte", Cadena = "ValorResidual(valorInicial, vidaUtil, periodoInicial, Horizonte)", Visible = true });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 5, Nombre = "Período final", Referencia = "periodoFinal", Secuencia = 1, IdTipoFormula = 7, IdPlantillaElemento = 2, PeriodoInicial = "periodoInicial", PeriodoFinal = "Horizonte", Cadena = "periodoInicial + vidaUtil - 1", Visible = false });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 6, Nombre = "Amortización", Referencia = "amortizacion", Secuencia = 2, IdTipoFormula = 5, IdPlantillaElemento = 2, PeriodoInicial = "periodoInicial", PeriodoFinal = "periodoFinal", Cadena = "DepreciacionLineal(valorInicial, vidaUtil)", Visible = true });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 7, Nombre = "Inversión inicial", Referencia = "inversionInicial", Secuencia = 3, IdTipoFormula = 6, IdPlantillaElemento = 2, PeriodoInicial = "periodoInicial - 1", PeriodoFinal = "periodoInicial - 1", Cadena = "valorInicial", Visible = true });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 8, Nombre = "Valor unitario mineral", Referencia = "valorUnitario", Secuencia = 1, IdTipoFormula = 10, IdPlantillaElemento = 3, PeriodoInicial = "periodoInicial", PeriodoFinal = "periodoFinal", Visible = false, Cadena = "ley * precioMetal * recuperacion / 31.1035" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 9, Nombre = "Ventas", Referencia = "ventas", Secuencia = 2, IdTipoFormula = 8, IdPlantillaElemento = 3, PeriodoInicial = "periodoInicial", PeriodoFinal = "periodoFinal", Visible = true, Cadena = "valorUnitario * tamanoMina" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 9, Nombre = "Costos", Referencia = "costos", Secuencia = 3, IdTipoFormula = 9, IdPlantillaElemento = 3, PeriodoInicial = "periodoInicial", PeriodoFinal = "periodoFinal", Visible = true, Cadena = "costoProduccion * tamanoMina" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 10, Nombre = "Gasto administrativo", Referencia = "administrativo", Secuencia = 1, IdTipoFormula = 11, IdPlantillaElemento = 4, PeriodoInicial = "2", PeriodoFinal = "Horizonte", Visible = true, Cadena = "monto" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 11, Nombre = "Préstamo",              Referencia = "prestamo",        Secuencia = 1, IdTipoFormula = 16, IdPlantillaElemento = 5, PeriodoInicial = "periodoInicial",                       PeriodoFinal = "periodoInicial",                           Visible = true, Cadena = "valorInicial" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 12, Nombre = "Amortización",          Referencia = "amortizacion",    Secuencia = 2, IdTipoFormula = 13, IdPlantillaElemento = 5, PeriodoInicial = "periodoInicial + periodosGracia + 1",  PeriodoFinal = "periodoInicial + periodosGracia + plazo",  Visible = true, Cadena = "Amortizacion(tasa, (Periodo - periodoInicial - periodosGracia), plazo, valorInicial)" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 13, Nombre = "Intereses gracia",    Referencia = "igracia",           Secuencia = 3, IdTipoFormula = 15, IdPlantillaElemento = 5, PeriodoInicial = "periodoInicial + 1",                   PeriodoFinal = "periodoInicial + periodosGracia",          Visible = true, Cadena = "Intereses(tasa, 1, plazo, valorInicial)" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 14, Nombre = "Intereses",             Referencia = "intereses",       Secuencia = 4, IdTipoFormula = 15, IdPlantillaElemento = 5, PeriodoInicial = "periodoInicial + periodosGracia + 1",  PeriodoFinal = "periodoInicial + periodosGracia + plazo",  Visible = true, Cadena = "Intereses(tasa, (Periodo - periodoInicial - periodosGracia), plazo, valorInicial)" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 15, Nombre = "Cuota",                 Referencia = "cuota",           Secuencia = 5, IdTipoFormula = 14, IdPlantillaElemento = 5, PeriodoInicial = "periodoInicial + 1",                   PeriodoFinal = "periodoInicial + periodosGracia + plazo",  Visible = true, Cadena = "amortizacion + igracia + intereses" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 16, Nombre = "Tasa", Referencia = "tasanual", Secuencia = 1, IdTipoFormula = 17, IdPlantillaElemento = 6, PeriodoInicial = "2", PeriodoFinal = "Horizonte", Visible = true, Cadena = "tasa" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 17, Nombre = "Tasa", Referencia = "tasanual", Secuencia = 1, IdTipoFormula = 18, IdPlantillaElemento = 7, PeriodoInicial = "2", PeriodoFinal = "Horizonte", Visible = true, Cadena = "tasa" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 18, Nombre = "Tasa", Referencia = "tasanual", Secuencia = 1, IdTipoFormula = 18, IdPlantillaElemento = 8, PeriodoInicial = "2", PeriodoFinal = "Horizonte", Visible = true, Cadena = "tasa" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 19, Nombre = "Tasa", Referencia = "tasanual", Secuencia = 1, IdTipoFormula = 19, IdPlantillaElemento = 9, PeriodoInicial = "2", PeriodoFinal = "Horizonte", Visible = true, Cadena = "tasa" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 20, Nombre = "Aporte", Referencia = "aporteInicial", Secuencia = 1, IdTipoFormula = 20, IdPlantillaElemento = 10, PeriodoInicial = "periodoInicial", PeriodoFinal = "periodoInicial", Visible = true, Cadena = "aporte" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Id = 21, Nombre = "Monto", Referencia = "monto", Secuencia = 1, IdTipoFormula = 21, IdPlantillaElemento = 11, PeriodoInicial = "periodoInicial", PeriodoFinal = "periodoFinal", Visible = true, Cadena = "otroingreso" });

        }

        public void SeedPlantillaProyectos()
        {
        }

        public void SeedPlantillaElementoProyectos()
        {
        }

        public void SeedPlantillaSalidaProyectos()
        {
        }
    }
}
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
        public DbSet<PlantillaProyecto> PlantillaProyectos { get; set; }
        public DbSet<PlantillaSalidaOperacion> PlantillaSalidaOperaciones { get; set; }
        public DbSet<PlantillaSalidaProyecto> PlantillaSalidaProyectos { get; set; }
        public DbSet<PlantillaOperacion> PlantillaOperaciones { get; set; }

        public DbRequester<TipoElemento> TipoElementosRequester { get; set; }
        public DbRequester<TipoFormula> TipoFormulasRequester { get; set; }
        public DbRequester<TipoParametro> TipoParametrosRequester { get; set; }
        public DbRequester<PlantillaParametro> PlantillaParametrosRequester { get; set; }
        public DbRequester<PlantillaElemento> PlantillaElementosRequester { get; set; }
        public DbRequester<PlantillaFormula> PlantillaFormulasRequester { get; set; }
        public DbRequester<PlantillaProyecto> PlantillaProyectosRequester { get; set; }
        public DbRequester<PlantillaSalidaOperacion> PlantillaSalidaOperacionesRequester { get; set; }
        public DbRequester<PlantillaSalidaProyecto> PlantillaSalidaProyectosRequester { get; set; }
        public DbRequester<PlantillaOperacion> PlantillaOperacionesRequester { get; set; }

        public void RegistrarTablasPlantilla()
        {
            TipoElementosRequester = new DbRequester<TipoElemento>(this, TipoElementos);
            TipoFormulasRequester = new DbRequester<TipoFormula>(this, TipoFormulas);
            TipoParametrosRequester = new DbRequester<TipoParametro>(this, TipoParametros);
            PlantillaParametrosRequester = new DbRequester<PlantillaParametro>(this, PlantillaParametros);
            PlantillaElementosRequester = new DbRequester<PlantillaElemento>(this, PlantillaElementos);
            PlantillaFormulasRequester = new DbRequester<PlantillaFormula>(this, PlantillaFormulas);
            PlantillaProyectosRequester = new DbRequester<PlantillaProyecto>(this, PlantillaProyectos);
            PlantillaSalidaOperacionesRequester = new DbRequester<PlantillaSalidaOperacion>(this, PlantillaSalidaOperaciones);
            PlantillaSalidaProyectosRequester = new DbRequester<PlantillaSalidaProyecto>(this, PlantillaSalidaProyectos);
            PlantillaOperacionesRequester = new DbRequester<PlantillaOperacion>(this, PlantillaOperaciones);
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
            SeedPlantillaSalidaProyectos();
            SeedPlantillaOperaciones();
            SeedPlantillaSalidaOperaciones();
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
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 1, Nombre = "Depreciación", IdTipoElemento = 1, Referencia = "ActivosFijos_Depreciacion" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 2, Nombre = "Inversión inicial", IdTipoElemento = 1, Referencia = "ActivosFijos_InversionInicial" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 3, Nombre = "Valor residual", IdTipoElemento = 1, Referencia = "ActivosFijos_ValorResidual" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 4, Nombre = "Período final", IdTipoElemento = 1, Referencia = "ActivosFijos_PeriodoFinal" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 5, Nombre = "Amortización", IdTipoElemento = 2, Referencia = "ActivosIntangibles_Amortizacion" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 6, Nombre = "Inversión inicial", IdTipoElemento = 2, Referencia = "ActivosIntangibles_InversionInicial" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 7, Nombre = "Período final", IdTipoElemento = 2, Referencia = "ActivosIntangibles_PeriodoFinal" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 8, Nombre = "Ventas", IdTipoElemento = 3, Referencia = "Operativos_Ventas" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 9, Nombre = "Costos", IdTipoElemento = 3, Referencia = "Operativos_Costos" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 10, Nombre = "Valor unitario", IdTipoElemento = 3, Referencia = "Operativos_ValorUnitario" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 11, Nombre = "Gasto administrativo", IdTipoElemento = 4, Referencia = "Gastos_GastoAdministrativo" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 12, Nombre = "Gasto de venta", IdTipoElemento = 4, Referencia = "Gastos_GastoVenta" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 13, Nombre = "Amortización", IdTipoElemento = 5, Referencia = "Financiamientos_Amortizacion" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 14, Nombre = "Cuota", IdTipoElemento = 5, Referencia = "Financiamientos_Cuota" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 15, Nombre = "Intereses", IdTipoElemento = 5, Referencia = "Financiamientos_Intereses" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 16, Nombre = "Préstamo", IdTipoElemento = 5, Referencia = "Financiamientos_Prestamo" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 17, Nombre = "Costo capital", IdTipoElemento = 5, Referencia = "Financiamientos_Costo" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 18, Nombre = "Renta", IdTipoElemento = 6, Unico = true, Referencia = "Impuestos_Renta" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 19, Nombre = "Operativo", IdTipoElemento = 6, Referencia = "Impuestos_Operativo" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 20, Nombre = "Participación", IdTipoElemento = 7, Referencia = "Participaciones" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 21, Nombre = "Aporte", IdTipoElemento = 8, Referencia = "Inversiones_Aporte" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 22, Nombre = "Costo capital", IdTipoElemento = 8, Referencia = "Inversiones_Costo" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 23, Nombre = "Otros ingresos", IdTipoElemento = 9, Referencia = "OtrosIngresos" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 24, Nombre = "Otros egresos", IdTipoElemento = 9, Referencia = "OtrosEgresos" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 25, Nombre = "Inversión", IdTipoElemento = 5, Referencia = "Financiamientos_Inversion" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 26, Nombre = "Inversión", IdTipoElemento = 8, Referencia = "Inversiones_Inversion" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 27, Nombre = "Inversión", IdTipoElemento = 1, Referencia = "ActivosFijos_Inversion" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 28, Nombre = "Inversión", IdTipoElemento = 2, Referencia = "ActivosIntangibles_Inversion" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 29, Nombre = "Garantía", IdTipoElemento = 8, Referencia = "Inversiones_Garantia" });
        }

        public void SeedTipoParametros()
        {
            TipoParametrosRequester.AddElement(new TipoParametro { Id = 1, Nombre = "Real" });
            TipoParametrosRequester.AddElement(new TipoParametro { Id = 2, Nombre = "Entero" });
            TipoParametrosRequester.AddElement(new TipoParametro { Id = 3, Nombre = "Monetario" });
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
            PlantillaElementosRequester.AddElement(new PlantillaElemento { Id = 11, Nombre = "Inversión de cierre simple", IdTipoElemento = 8 });
            PlantillaElementosRequester.AddElement(new PlantillaElemento { Id = 12, Nombre = "Inversión de cierre con garantía", IdTipoElemento = 8 });
            PlantillaElementosRequester.AddElement(new PlantillaElemento { Id = 13, Nombre = "Otro ingreso", IdTipoElemento = 9 });
        }

        public void SeedPlantillaParametros()
        {
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Vida útil (períodos)",               Referencia = "vidaUtil",        IdTipoParametro = 2, IdPlantillaElemento = 1, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Valor inicial (US$)",                Referencia = "valorInicial",    IdTipoParametro = 3, IdPlantillaElemento = 1, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Vida útil (períodos)",               Referencia = "vidaUtil",        IdTipoParametro = 2, IdPlantillaElemento = 2, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Valor inicial (US$)",                Referencia = "valorInicial",    IdTipoParametro = 3, IdPlantillaElemento = 2, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Tamaño de mina (kTM/período)",  Referencia = "tamanoMina",      IdTipoParametro = 1, IdPlantillaElemento = 3, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Ley mineral (oz/TM)",           Referencia = "ley",             IdTipoParametro = 1, IdPlantillaElemento = 3, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Precio metal (US$/oz)",         Referencia = "precioMetal",     IdTipoParametro = 3, IdPlantillaElemento = 3, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Recuperación",                  Referencia = "recuperacion",    IdTipoParametro = 1, IdPlantillaElemento = 3, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Costo de producción (US$/TM)",  Referencia = "costoProduccion", IdTipoParametro = 3, IdPlantillaElemento = 3, Constante = false });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Monto (US$)", Referencia = "monto", IdTipoParametro = 3, IdPlantillaElemento = 4, Constante = false });
            
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Período",                Referencia = "periodoInicial",  IdTipoParametro = 2, IdPlantillaElemento = 5, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Valor inicial (US$)",    Referencia = "valorInicial",    IdTipoParametro = 3, IdPlantillaElemento = 5, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Períodos de gracia",     Referencia = "periodosGracia",  IdTipoParametro = 2, IdPlantillaElemento = 5, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Tasa",                   Referencia = "tasa",            IdTipoParametro = 1, IdPlantillaElemento = 5, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Plazo",                  Referencia = "plazo",           IdTipoParametro = 2, IdPlantillaElemento = 5, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Tasa", Referencia = "tasa", IdTipoParametro = 1, IdPlantillaElemento = 6, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Tasa", Referencia = "tasa", IdTipoParametro = 1, IdPlantillaElemento = 7, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Tasa", Referencia = "tasa", IdTipoParametro = 1, IdPlantillaElemento = 8, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Tasa", Referencia = "tasa", IdTipoParametro = 1, IdPlantillaElemento = 9, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Período", Referencia = "periodoInicial", IdTipoParametro = 2, IdPlantillaElemento = 10, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Monto", Referencia = "aporte", IdTipoParametro = 3, IdPlantillaElemento = 10, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Costo capital", Referencia = "tasa", IdTipoParametro = 1, IdPlantillaElemento = 10, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Período", Referencia = "periodoInicial", IdTipoParametro = 2, IdPlantillaElemento = 11, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Monto", Referencia = "aporte", IdTipoParametro = 3, IdPlantillaElemento = 11, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Presupuesto", Referencia = "presupuesto", IdTipoParametro = 3, IdPlantillaElemento = 12, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Costo de garantía", Referencia = "costoGarantia", IdTipoParametro = 3, IdPlantillaElemento = 12, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Monto inicial", Referencia = "montoInicial", IdTipoParametro = 3, IdPlantillaElemento = 12, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Otro ingreso", Referencia = "otroingreso", IdTipoParametro = 3, IdPlantillaElemento = 13, Constante = false });

        }

        public void SeedPlantillaFormulas()
        {
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Período final", Referencia = "periodoFinal", Secuencia = 1, IdTipoFormula = 4, IdPlantillaElemento = 1, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte", Cadena = "PeriodosPreOperativos + vidaUtil", Visible = false });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Depreciación lineal", Referencia = "depreciacionLineal", Secuencia = 2, IdTipoFormula = 1, IdPlantillaElemento = 1, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Cadena = "DepreciacionLineal(valorInicial, vidaUtil, PeriodosPreOperativos + 1, Periodo)", Visible = true });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Inversión inicial", Referencia = "inversionInicial", Secuencia = 3, IdTipoFormula = 2, IdPlantillaElemento = 1, PeriodoInicial = "PeriodosPreOperativos", PeriodoFinal = "PeriodosPreOperativos", Cadena = "valorInicial", Visible = true });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Valor residual", Referencia = "valorResidual", Secuencia = 4, IdTipoFormula = 3, IdPlantillaElemento = 1, PeriodoInicial = "Horizonte - PeriodosCierre", PeriodoFinal = "Horizonte - PeriodosCierre", Cadena = "ValorResidual(valorInicial, vidaUtil, PeriodosPreOperativos + 1, Periodo)", Visible = true });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Inversión", Referencia = "inversion", Secuencia = 5, IdTipoFormula = 27, IdPlantillaElemento = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Cadena = "valorInicial", Visible = false });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Período final", Referencia = "periodoFinal", Secuencia = 1, IdTipoFormula = 7, IdPlantillaElemento = 2, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte", Cadena = "PeriodosPreOperativos + vidaUtil", Visible = false });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Amortización", Referencia = "amortizacion", Secuencia = 2, IdTipoFormula = 5, IdPlantillaElemento = 2, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Cadena = "DepreciacionLineal(valorInicial, vidaUtil, PeriodosPreOperativos + 1, Periodo)", Visible = true });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Inversión inicial", Referencia = "inversionInicial", Secuencia = 3, IdTipoFormula = 6, IdPlantillaElemento = 2, PeriodoInicial = "PeriodosPreOperativos", PeriodoFinal = "PeriodosPreOperativos", Cadena = "valorInicial", Visible = true });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Inversión", Referencia = "inversion", Secuencia = 4, IdTipoFormula = 28, IdPlantillaElemento = 2, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Cadena = "valorInicial", Visible = false });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Valor unitario mineral", Referencia = "valorUnitario", Secuencia = 1, IdTipoFormula = 10, IdPlantillaElemento = 3, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Visible = false, Cadena = "ley * precioMetal * recuperacion" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Ventas", Referencia = "ventas", Secuencia = 2, IdTipoFormula = 8, IdPlantillaElemento = 3, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Visible = true, Cadena = "valorUnitario * tamanoMina" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Costos", Referencia = "costos", Secuencia = 3, IdTipoFormula = 9, IdPlantillaElemento = 3, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Visible = true, Cadena = "costoProduccion * tamanoMina" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Gasto administrativo", Referencia = "administrativo", Secuencia = 1, IdTipoFormula = 11, IdPlantillaElemento = 4, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Visible = true, Cadena = "monto" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Préstamo",           Referencia = "prestamo",        Secuencia = 1, IdTipoFormula = 16, IdPlantillaElemento = 5, PeriodoInicial = "periodoInicial",                       PeriodoFinal = "periodoInicial",                           Visible = true, Cadena = "valorInicial" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Amortización",       Referencia = "amortizacion",    Secuencia = 2, IdTipoFormula = 13, IdPlantillaElemento = 5, PeriodoInicial = "periodoInicial + periodosGracia + 1",  PeriodoFinal = "periodoInicial + periodosGracia + plazo",  Visible = true, Cadena = "Amortizacion(tasa, (Periodo - periodoInicial - periodosGracia), plazo, valorInicial)" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Intereses gracia",   Referencia = "igracia",           Secuencia = 3, IdTipoFormula = 15, IdPlantillaElemento = 5, PeriodoInicial = "periodoInicial + 1",                   PeriodoFinal = "periodoInicial + periodosGracia",          Visible = true, Cadena = "Intereses(tasa, 1, plazo, valorInicial)" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Intereses",          Referencia = "intereses",       Secuencia = 4, IdTipoFormula = 15, IdPlantillaElemento = 5, PeriodoInicial = "periodoInicial + periodosGracia + 1",  PeriodoFinal = "periodoInicial + periodosGracia + plazo",  Visible = true, Cadena = "Intereses(tasa, (Periodo - periodoInicial - periodosGracia), plazo, valorInicial)" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Cuota",              Referencia = "cuota",           Secuencia = 5, IdTipoFormula = 14, IdPlantillaElemento = 5, PeriodoInicial = "periodoInicial + 1",                   PeriodoFinal = "periodoInicial + periodosGracia + plazo",  Visible = true, Cadena = "amortizacion + igracia + intereses" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Inversión", Referencia = "inversion", Secuencia = 6, IdTipoFormula = 25, IdPlantillaElemento = 5, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Visible = false, Cadena = "valorInicial" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Costo capital", Referencia = "costoCapital", Secuencia = 7, IdTipoFormula = 17, IdPlantillaElemento = 5, PeriodoInicial = "1", PeriodoFinal = "1", Visible = false, Cadena = "tasa * inversion" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Tasa", Referencia = "tasanual", Secuencia = 1, IdTipoFormula = 18, IdPlantillaElemento = 6, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Visible = true, Cadena = "tasa" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Tasa", Referencia = "tasanual", Secuencia = 1, IdTipoFormula = 19, IdPlantillaElemento = 7, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Visible = true, Cadena = "tasa" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Tasa", Referencia = "tasanual", Secuencia = 1, IdTipoFormula = 19, IdPlantillaElemento = 8, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Visible = true, Cadena = "tasa" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Tasa", Referencia = "tasanual", Secuencia = 1, IdTipoFormula = 20, IdPlantillaElemento = 9, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Visible = true, Cadena = "tasa" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Aporte", Referencia = "aporteInicial", Secuencia = 1, IdTipoFormula = 21, IdPlantillaElemento = 10, PeriodoInicial = "periodoInicial", PeriodoFinal = "periodoInicial", Visible = true, Cadena = "aporte" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Inversión", Referencia = "inversion", Secuencia = 2, IdTipoFormula = 26, IdPlantillaElemento = 10, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Visible = false, Cadena = "aporte" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Costo capital", Referencia = "costoCapital", Secuencia = 3, IdTipoFormula = 22, IdPlantillaElemento = 10, PeriodoInicial = "1", PeriodoFinal = "1", Visible = false, Cadena = "tasa * inversion" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Aporte", Referencia = "aporteInicial", Secuencia = 1, IdTipoFormula = 21, IdPlantillaElemento = 11, PeriodoInicial = "periodoInicial", PeriodoFinal = "periodoInicial", Visible = true, Cadena = "aporte" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Plan de cierre", Referencia = "planCierre", Secuencia = 1, IdTipoFormula = 21, IdPlantillaElemento = 12, PeriodoInicial = "Horizonte - PeriodosCierre + 1", PeriodoFinal = "Horizonte - PeriodosCierre + 1", Visible = true, Cadena = "montoInicial" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Plan de postcierre", Referencia = "planPostCierre", Secuencia = 2, IdTipoFormula = 21, IdPlantillaElemento = 12, PeriodoInicial = "Horizonte - PeriodosCierre + 2", PeriodoFinal = "Horizonte", Visible = true, Cadena = "(presupuesto - montoInicial)/(PeriodosCierre - 1)" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Gasto de garantías", Referencia = "garantia", Secuencia = 3, IdTipoFormula = 29, IdPlantillaElemento = 12, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Visible = true, Cadena = "(presupuesto / (Horizonte - PeriodosPreOperativos - PeriodosCierre)) * costoGarantia * (Periodo - PeriodosPreOperativos)" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Monto", Referencia = "monto", Secuencia = 1, IdTipoFormula = 23, IdPlantillaElemento = 13, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Visible = true, Cadena = "otroingreso" });

        }

        public void SeedPlantillaProyectos()
        {
            PlantillaProyectosRequester.AddElement(new PlantillaProyecto { Id = 1, Nombre = "Proyecto genérico" });
        }

        public void SeedPlantillaSalidaProyectos()
        {
            PlantillaSalidaProyectosRequester.AddElement(new PlantillaSalidaProyecto { Id = 1, Nombre = "EGP del proyecto", Secuencia = 1, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte" });
            PlantillaSalidaProyectosRequester.AddElement(new PlantillaSalidaProyecto { Id = 2, Nombre = "Flujo de caja del proyecto", Secuencia = 2, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte" });
            PlantillaSalidaProyectosRequester.AddElement(new PlantillaSalidaProyecto { Id = 3, Nombre = "EGP financiero del proyecto", Secuencia = 3, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte" });
            PlantillaSalidaProyectosRequester.AddElement(new PlantillaSalidaProyecto { Id = 4, Nombre = "Flujo de caja financiero del proyecto", Secuencia = 4, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte" });
            PlantillaSalidaProyectosRequester.AddElement(new PlantillaSalidaProyecto { Id = 5, Nombre = "Indicadores del proyecto", Secuencia = 5, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1" });
        }

        public void SeedPlantillaOperaciones()
        {
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id =  1, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia =  1, Nombre = "Préstamos", Referencia = "prestamos", Cadena = "Financiamientos_Prestamo" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id =  2, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia =  2, Nombre = "Ventas", Referencia = "ventas", Cadena = "Operativos_Ventas" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id =  3, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia =  3, Nombre = "Otros ingresos", Referencia = "otrosIngresos", Cadena = "OtrosIngresos" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id =  4, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia =  4, Nombre = "Valor residual", Referencia = "valorResidual", Cadena = "ActivosFijos_ValorResidual" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 5, IdPlantillaProyecto = 1, PeriodoInicial = "Horizonte - PeriodosCierre", PeriodoFinal = "Horizonte - PeriodosCierre", Secuencia = 5, Nombre = "Recuperacion de capital", Referencia = "capital", Cadena = "Financiamientos_Inversion + Inversiones_Inversion - ActivosFijos_Inversion - ActivosIntangibles_Inversion" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id =  6, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia =  6, Nombre = "Total ingresos", Referencia = "ingresos", Cadena = "ventas + otrosIngresos + valorResidual + capital", Subrayar = true });

            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 7, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 7, Nombre = "Total ingresos", Referencia = "ingresosFin", Cadena = "prestamos + ventas + otrosIngresos + valorResidual + capital", Subrayar = true });
            
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id =  8, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia =  8, Nombre = "Inversiones", Referencia = "inversiones", Cadena = "Inversiones_Aporte + Financiamientos_Prestamo" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id =  9, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia =  9, Nombre = "Costos", Referencia = "costos", Cadena = "Operativos_Costos" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 10, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia =  10, Nombre = "Depreaciación de activos fijos", Referencia = "depreciacion", Cadena = "ActivosFijos_Depreciacion" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 11, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 11, Nombre = "Amortización de activos intangibles", Referencia = "amortizacionIntangibles", Cadena = "ActivosIntangibles_Amortizacion" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 12, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 12, Nombre = "Utilidad bruta", Referencia = "utilidadBruta", Cadena = "ventas + otrosIngresos + valorResidual - costos - depreciacion - amortizacionIntangibles", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 13, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 13, Nombre = "Gastos administrativos", Referencia = "gastosAdmin", Cadena = "Gastos_GastoAdministrativo" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 14, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 14, Nombre = "Gastos de venta", Referencia = "gastosVenta", Cadena = "Gastos_GastoVenta" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 15, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 15, Nombre = "Utilidad operativa", Referencia = "utilidadOperativa", Cadena = "utilidadBruta - gastosAdmin - gastosVenta", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 16, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 16, Nombre = "Amortización de préstamos", Referencia = "amortizacion", Cadena = "Financiamientos_Amortizacion" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 17, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 17, Nombre = "Intereses", Referencia = "intereses", Cadena = "Financiamientos_Intereses" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 18, IdPlantillaProyecto = 1, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Secuencia = 18, Nombre = "Regalía minera e Impuesto especial", Referencia = "impuestosOperativos", Cadena = "utilidadOperativa * Impuestos_Operativo" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 19, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 19, Nombre = "Gastos de garantía", Referencia = "gastosGarantia", Cadena = "Inversiones_Garantia" });

            //
            // del proyecto

            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 20, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 20, Nombre = "Utilidad antes de participaciones", Referencia = "utilidadPreParticpacion", Cadena = "utilidadOperativa - impuestosOperativos - gastosGarantia", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 21, IdPlantillaProyecto = 1, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Secuencia = 21, Nombre = "Participaciones", Referencia = "participaciones", Cadena = "utilidadPreParticpacion * Participaciones" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 22, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 22, Nombre = "Utilidad antes de impuestos", Referencia = "utilidadPreImpuestos", Cadena = "utilidadPreParticpacion - participaciones", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 23, IdPlantillaProyecto = 1, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Secuencia = 23, Nombre = "Impuesto a la renta", Referencia = "impuestoRenta", Cadena = "utilidadPreImpuestos * Impuestos_Renta" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 24, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 24, Nombre = "Utilidad neta", Referencia = "utilidadNeta", Cadena = "utilidadPreImpuestos - impuestoRenta", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 25, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 25, Nombre = "Total egresos", Referencia = "egresos", Cadena = "inversiones + costos + gastosAdmin + gastosVenta + impuestosOperativos + participaciones + impuestoRenta + gastosGarantia", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 26, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 26, Nombre = "Saldo final", Referencia = "saldo", Cadena = "ingresos - egresos", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 27, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 27, Nombre = "Inversión del proyecto", Referencia = "inversionE", Cadena = "Inversiones_Inversion + Financiamientos_Inversion", Indicador = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 28, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 28, Nombre = "Costo capital del proyecto", Referencia = "KE", Cadena = "(Financiamientos_Costo * (1 - Impuestos_Renta) + Inversiones_Costo)/inversionE", Indicador = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 29, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 29, Nombre = "TIR del proyecto", Referencia = "TIRE", Cadena = "Tir(saldo)", Indicador = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 30, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 30, Nombre = "VAN del proyecto", Referencia = "VANE", Cadena = "Van(KE,saldo)", Indicador = true });
            
            //
            // del inversionista
            
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 31, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 31, Nombre = "Gastos financieros", Referencia = "gastosFinancieros", Cadena = "Financiamientos_Intereses" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 32, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 32, Nombre = "Utilidad antes de participaciones ", Referencia = "utilidadPreParticpacionFin", Cadena = "utilidadOperativa - impuestosOperativos - gastosFinancieros - gastosGarantia", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 33, IdPlantillaProyecto = 1, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Secuencia = 33, Nombre = "Participaciones ", Referencia = "participacionesFin", Cadena = "utilidadPreParticpacionFin * Participaciones" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 34, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 34, Nombre = "Utilidad antes de impuestos", Referencia = "utilidadPreImpuestosFin", Cadena = "utilidadPreParticpacionFin - participacionesFin", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 35, IdPlantillaProyecto = 1, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Secuencia = 35, Nombre = "Impuesto a la renta", Referencia = "impuestoRentaFin", Cadena = "utilidadPreImpuestosFin * Impuestos_Renta" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 36, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 36, Nombre = "Utilidad neta", Referencia = "utilidadNetaFin", Cadena = "utilidadPreImpuestosFin - impuestoRentaFin", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 37, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 37, Nombre = "Total egresos", Referencia = "egresosFin", Cadena = "inversiones + costos + gastosAdmin + gastosVenta + amortizacion + intereses + impuestosOperativos + participacionesFin + impuestoRentaFin + gastosGarantia", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 38, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 38, Nombre = "Saldo final", Referencia = "saldoFin", Cadena = "ingresosFin - egresosFin", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 39, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 39, Nombre = "Inversión del accionista", Referencia = "inversionF", Cadena = "Inversiones_Inversion", Indicador = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 40, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 40, Nombre = "Costo capital de inversionista", Referencia = "KF", Cadena = "(Inversiones_Costo)/inversionF", Indicador = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 41, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 41, Nombre = "TIR del inversionista", Referencia = "TIRF", Cadena = "Tir(saldoFin)", Indicador = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 42, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 42, Nombre = "VAN del inversionista", Referencia = "VANF", Cadena = "Van(KF,saldoFin)", Indicador = true });
        
            //
            //  del banco

            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 43, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 43, Nombre = "Inversión del banco", Referencia = "inversionB", Cadena = "Financiamientos_Inversion", Indicador = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 44, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 44, Nombre = "TIR del banco", Referencia = "TIRB", Cadena = "(Financiamientos_Costo)/inversionB", Indicador = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 45, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 45, Nombre = "VAN del banco", Referencia = "VANB", Cadena = "VANE - VANF", Indicador = true });
        }

        public void SeedPlantillaSalidaOperaciones()
        {
            //
            // EGP del proyecto

            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion =  2 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion =  3 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion =  4 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion =  9 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 10 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 11 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 12 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 13 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 14 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 15 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 18 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 19 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 20 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 21 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 22 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 23 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 24 });

            //
            // Flujo de caja del proyecto

            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 2 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 3 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 4 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 5 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 6 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 8 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 9 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 13 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 14 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 18 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 19 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 21 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 23 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 24 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 25 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 28 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 29 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 30 });

            //
            // EGP del inversionista

            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 2 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 3 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 4 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 9 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 10 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 11 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 12 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 13 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 14 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 15 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 18 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 19 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 31 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 32 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 33 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 34 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 35 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 36 });

            //
            // Flujo de caja del inversionista

            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 1 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 2 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 3 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 4 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 5 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 7 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 8 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 9 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 13 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 14 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 16 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 17 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 18 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 19 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 31 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 33 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 35 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 37 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 38 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 40 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 41 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 42 });

            //
            // Indicadores generales

            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 27 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 28 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 29 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 30 });

            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 39 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 40 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 41 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 42 });

            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 43 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 44 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 45 });
        }
    }
}
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
        public DbSet<TipoDato> TipoDatos { get; set; }
        public DbSet<PlantillaParametro> PlantillaParametros { get; set; }
        public DbSet<PlantillaElemento> PlantillaElementos { get; set; }
        public DbSet<PlantillaFormula> PlantillaFormulas { get; set; }
        public DbSet<PlantillaProyecto> PlantillaProyectos { get; set; }
        public DbSet<PlantillaSalidaOperacion> PlantillaSalidaOperaciones { get; set; }
        public DbSet<PlantillaSalidaProyecto> PlantillaSalidaProyectos { get; set; }
        public DbSet<PlantillaOperacion> PlantillaOperaciones { get; set; }

        public DbRequester<TipoElemento> TipoElementosRequester { get; set; }
        public DbRequester<TipoFormula> TipoFormulasRequester { get; set; }
        public DbRequester<TipoDato> TipoDatosRequester { get; set; }
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
            TipoDatosRequester = new DbRequester<TipoDato>(this, TipoDatos);
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
            SeedTipoDatos();
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
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 5, Nombre = "Inversión", IdTipoElemento = 1, Referencia = "ActivosFijos_Inversion" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 6, Nombre = "Otro", IdTipoElemento = 1, Referencia = "ActivosFijos_Otro" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 7, Nombre = "Amortización", IdTipoElemento = 2, Referencia = "ActivosIntangibles_Amortizacion" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 8, Nombre = "Inversión inicial", IdTipoElemento = 2, Referencia = "ActivosIntangibles_InversionInicial" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 9, Nombre = "Período final", IdTipoElemento = 2, Referencia = "ActivosIntangibles_PeriodoFinal" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 10, Nombre = "Inversión", IdTipoElemento = 2, Referencia = "ActivosIntangibles_Inversion" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 11, Nombre = "Otro", IdTipoElemento = 2, Referencia = "ActivosIntangibles_Otro" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 12, Nombre = "Ventas", IdTipoElemento = 3, Referencia = "Operativos_Ventas" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 13, Nombre = "Costos", IdTipoElemento = 3, Referencia = "Operativos_Costos" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 14, Nombre = "Valor unitario", IdTipoElemento = 3, Referencia = "Operativos_ValorUnitario" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 15, Nombre = "Otro", IdTipoElemento = 3, Referencia = "Operativos_Otro" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 16, Nombre = "Gasto administrativo", IdTipoElemento = 4, Referencia = "Gastos_GastoAdministrativo" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 17, Nombre = "Gasto de venta", IdTipoElemento = 4, Referencia = "Gastos_GastoVenta" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 18, Nombre = "Otro", IdTipoElemento = 4, Referencia = "Gastos_Otro" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 19, Nombre = "Amortización", IdTipoElemento = 5, Referencia = "Financiamientos_Amortizacion" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 20, Nombre = "Cuota", IdTipoElemento = 5, Referencia = "Financiamientos_Cuota" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 21, Nombre = "Intereses", IdTipoElemento = 5, Referencia = "Financiamientos_Intereses" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 22, Nombre = "Préstamo", IdTipoElemento = 5, Referencia = "Financiamientos_Prestamo" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 23, Nombre = "Costo capital", IdTipoElemento = 5, Referencia = "Financiamientos_Costo" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 24, Nombre = "Inversión", IdTipoElemento = 5, Referencia = "Financiamientos_Inversion" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 25, Nombre = "Otro", IdTipoElemento = 5, Referencia = "Financiamientos_Otro" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 26, Nombre = "Renta", IdTipoElemento = 6, Unico = true, Referencia = "Impuestos_Renta" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 27, Nombre = "Operativo", IdTipoElemento = 6, Referencia = "Impuestos_Operativo" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 28, Nombre = "Otro", IdTipoElemento = 6, Referencia = "Impuestos_Otro" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 29, Nombre = "Participación", IdTipoElemento = 7, Referencia = "Participaciones" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 30, Nombre = "Otro", IdTipoElemento = 7, Referencia = "Participaciones_Otro" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 31, Nombre = "Aporte", IdTipoElemento = 8, Referencia = "Inversiones_Aporte" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 32, Nombre = "Costo capital", IdTipoElemento = 8, Referencia = "Inversiones_Costo" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 33, Nombre = "Garantía", IdTipoElemento = 8, Referencia = "Inversiones_Garantia" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 34, Nombre = "Inversión", IdTipoElemento = 8, Referencia = "Inversiones_Inversion" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 35, Nombre = "Otro", IdTipoElemento = 8, Referencia = "Inversiones_Otro" });

            TipoFormulasRequester.AddElement(new TipoFormula { Id = 36, Nombre = "Otros ingresos", IdTipoElemento = 9, Referencia = "Otros_Ingresos" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 37, Nombre = "Otros egresos", IdTipoElemento = 9, Referencia = "Otros_Egresos" });
            TipoFormulasRequester.AddElement(new TipoFormula { Id = 38, Nombre = "Otro", IdTipoElemento = 9, Referencia = "Otros_Otro" });
           
        }

        public void SeedTipoDatos()
        {
            TipoDatosRequester.AddElement(new TipoDato { Id = 1, Nombre = "Entero", Formato = "{0:#,##0}" });
            TipoDatosRequester.AddElement(new TipoDato { Id = 2, Nombre = "Real", Formato = "{0:#,##0.##########}" });
            TipoDatosRequester.AddElement(new TipoDato { Id = 3, Nombre = "Monetario", Formato = "{0:#,##0}" });
            TipoDatosRequester.AddElement(new TipoDato { Id = 4, Nombre = "Porcentaje", Formato = "{0:P2}" });
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
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Vida útil (períodos)",               Referencia = "vidaUtil",        IdTipoDato = 2, IdPlantillaElemento = 1, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Valor inicial (US$)",                Referencia = "valorInicial",    IdTipoDato = 3, IdPlantillaElemento = 1, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Vida útil (períodos)",               Referencia = "vidaUtil",        IdTipoDato = 2, IdPlantillaElemento = 2, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Valor inicial (US$)",                Referencia = "valorInicial",    IdTipoDato = 3, IdPlantillaElemento = 2, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Tamaño de mina (kTM/período)",  Referencia = "tamanoMina",      IdTipoDato = 1, IdPlantillaElemento = 3, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Ley mineral (oz/TM)",           Referencia = "ley",             IdTipoDato = 1, IdPlantillaElemento = 3, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Precio metal (US$/oz)",         Referencia = "precioMetal",     IdTipoDato = 3, IdPlantillaElemento = 3, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Recuperación",                  Referencia = "recuperacion",    IdTipoDato = 1, IdPlantillaElemento = 3, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Costo de producción (US$/TM)",  Referencia = "costoProduccion", IdTipoDato = 3, IdPlantillaElemento = 3, Constante = false });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Costos fijos (US$)", Referencia = "costoFijo", IdTipoDato = 3, IdPlantillaElemento = 3, Constante = false });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Monto (US$)", Referencia = "monto", IdTipoDato = 3, IdPlantillaElemento = 4, Constante = false });
            
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Período",                Referencia = "periodoInicial",  IdTipoDato = 2, IdPlantillaElemento = 5, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Valor inicial (US$)",    Referencia = "valorInicial",    IdTipoDato = 3, IdPlantillaElemento = 5, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Períodos de gracia",     Referencia = "periodosGracia",  IdTipoDato = 2, IdPlantillaElemento = 5, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Tasa",                   Referencia = "tasa",            IdTipoDato = 1, IdPlantillaElemento = 5, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Plazo",                  Referencia = "plazo",           IdTipoDato = 2, IdPlantillaElemento = 5, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Tasa", Referencia = "tasa", IdTipoDato = 1, IdPlantillaElemento = 6, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Tasa", Referencia = "tasa", IdTipoDato = 1, IdPlantillaElemento = 7, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Tasa", Referencia = "tasa", IdTipoDato = 1, IdPlantillaElemento = 8, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Tasa", Referencia = "tasa", IdTipoDato = 1, IdPlantillaElemento = 9, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Período", Referencia = "periodoInicial", IdTipoDato = 2, IdPlantillaElemento = 10, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Monto", Referencia = "aporte", IdTipoDato = 3, IdPlantillaElemento = 10, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Costo capital", Referencia = "tasa", IdTipoDato = 1, IdPlantillaElemento = 10, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Período", Referencia = "periodoInicial", IdTipoDato = 2, IdPlantillaElemento = 11, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Monto", Referencia = "aporte", IdTipoDato = 3, IdPlantillaElemento = 11, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Presupuesto", Referencia = "presupuesto", IdTipoDato = 3, IdPlantillaElemento = 12, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Costo de garantía", Referencia = "costoGarantia", IdTipoDato = 3, IdPlantillaElemento = 12, Constante = true });
            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Monto inicial", Referencia = "montoInicial", IdTipoDato = 3, IdPlantillaElemento = 12, Constante = true });

            PlantillaParametrosRequester.AddElement(new PlantillaParametro { Nombre = "Otro ingreso", Referencia = "otroingreso", IdTipoDato = 3, IdPlantillaElemento = 13, Constante = false });

        }

        public void SeedPlantillaFormulas()
        {
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Período final", IdTipoDato = 1, Referencia = "periodoFinal", Secuencia = 10, IdTipoFormula = 4, IdPlantillaElemento = 1, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte", Cadena = "PeriodosPreOperativos + vidaUtil", Visible = false });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Depreciación lineal", IdTipoDato = 3, Referencia = "depreciacionLineal", Secuencia = 20, IdTipoFormula = 1, IdPlantillaElemento = 1, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Cadena = "DepreciacionLineal(valorInicial, vidaUtil, PeriodosPreOperativos + 1, Periodo)", Visible = true });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Inversión inicial", IdTipoDato = 3, Referencia = "inversionInicial", Secuencia = 30, IdTipoFormula = 2, IdPlantillaElemento = 1, PeriodoInicial = "PeriodosPreOperativos", PeriodoFinal = "PeriodosPreOperativos", Cadena = "valorInicial", Visible = true });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Valor residual", IdTipoDato = 3, Referencia = "valorResidual", Secuencia = 40, IdTipoFormula = 3, IdPlantillaElemento = 1, PeriodoInicial = "Horizonte - PeriodosCierre", PeriodoFinal = "Horizonte - PeriodosCierre", Cadena = "ValorResidual(valorInicial, vidaUtil, PeriodosPreOperativos + 1, Periodo)", Visible = true });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Inversión", IdTipoDato = 3, Referencia = "inversion", Secuencia = 50, IdTipoFormula = 5, IdPlantillaElemento = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Cadena = "valorInicial", Visible = false });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Período final", IdTipoDato = 1, Referencia = "periodoFinal", Secuencia = 10, IdTipoFormula = 9, IdPlantillaElemento = 2, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte", Cadena = "PeriodosPreOperativos + vidaUtil", Visible = false });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Amortización", IdTipoDato = 3, Referencia = "amortizacion", Secuencia = 20, IdTipoFormula = 7, IdPlantillaElemento = 2, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Cadena = "DepreciacionLineal(valorInicial, vidaUtil, PeriodosPreOperativos + 1, Periodo)", Visible = true });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Inversión inicial", IdTipoDato = 3, Referencia = "inversionInicial", Secuencia = 30, IdTipoFormula = 8, IdPlantillaElemento = 2, PeriodoInicial = "PeriodosPreOperativos", PeriodoFinal = "PeriodosPreOperativos", Cadena = "valorInicial", Visible = true });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Inversión", IdTipoDato = 3, Referencia = "inversion", Secuencia = 40, IdTipoFormula = 10, IdPlantillaElemento = 2, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Cadena = "valorInicial", Visible = false });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Valor unitario mineral", IdTipoDato = 3, Referencia = "valorUnitario", Secuencia = 10, IdTipoFormula = 14, IdPlantillaElemento = 3, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Visible = false, Cadena = "ley * precioMetal * recuperacion" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Ventas", IdTipoDato = 3, Referencia = "ventas", Secuencia = 20, IdTipoFormula = 12, IdPlantillaElemento = 3, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Visible = true, Cadena = "valorUnitario * tamanoMina" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Costos", IdTipoDato = 3, Referencia = "costos", Secuencia = 30, IdTipoFormula = 13, IdPlantillaElemento = 3, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Visible = true, Cadena = "costoProduccion * tamanoMina + costoFijo" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Gasto administrativo", IdTipoDato = 3, Referencia = "administrativo", Secuencia = 10, IdTipoFormula = 16, IdPlantillaElemento = 4, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Visible = true, Cadena = "monto" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Préstamo", IdTipoDato = 3, Referencia = "prestamo", Secuencia = 10, IdTipoFormula = 22, IdPlantillaElemento = 5, PeriodoInicial = "periodoInicial", PeriodoFinal = "periodoInicial", Visible = true, Cadena = "valorInicial" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Amortización", IdTipoDato = 3, Referencia = "amortizacion", Secuencia = 20, IdTipoFormula = 19, IdPlantillaElemento = 5, PeriodoInicial = "periodoInicial + periodosGracia + 1", PeriodoFinal = "periodoInicial + periodosGracia + plazo", Visible = true, Cadena = "Amortizacion(tasa, (Periodo - periodoInicial - periodosGracia), plazo, valorInicial)" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Intereses gracia", IdTipoDato = 3, Referencia = "igracia", Secuencia = 30, IdTipoFormula = 21, IdPlantillaElemento = 5, PeriodoInicial = "periodoInicial + 1", PeriodoFinal = "periodoInicial + periodosGracia", Visible = true, Cadena = "Intereses(tasa, 1, plazo, valorInicial)" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Intereses", IdTipoDato = 3, Referencia = "intereses", Secuencia = 40, IdTipoFormula = 21, IdPlantillaElemento = 5, PeriodoInicial = "periodoInicial + periodosGracia + 1", PeriodoFinal = "periodoInicial + periodosGracia + plazo", Visible = true, Cadena = "Intereses(tasa, (Periodo - periodoInicial - periodosGracia), plazo, valorInicial)" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Cuota", IdTipoDato = 3, Referencia = "cuota", Secuencia = 50, IdTipoFormula = 20, IdPlantillaElemento = 5, PeriodoInicial = "periodoInicial + 1", PeriodoFinal = "periodoInicial + periodosGracia + plazo", Visible = true, Cadena = "amortizacion + igracia + intereses" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Inversión", IdTipoDato = 3, Referencia = "inversion", Secuencia = 60, IdTipoFormula = 24, IdPlantillaElemento = 5, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Visible = false, Cadena = "valorInicial" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Costo capital", IdTipoDato = 3, Referencia = "costoCapital", Secuencia = 70, IdTipoFormula = 25, IdPlantillaElemento = 5, PeriodoInicial = "1", PeriodoFinal = "1", Visible = false, Cadena = "tasa * inversion" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Tasa", IdTipoDato = 4, Referencia = "tasanual", Secuencia = 10, IdTipoFormula = 26, IdPlantillaElemento = 6, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Visible = true, Cadena = "tasa" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Tasa", IdTipoDato = 4, Referencia = "tasanual", Secuencia = 10, IdTipoFormula = 27, IdPlantillaElemento = 7, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Visible = true, Cadena = "tasa" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Tasa", IdTipoDato = 4, Referencia = "tasanual", Secuencia = 10, IdTipoFormula = 27, IdPlantillaElemento = 8, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Visible = true, Cadena = "tasa" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Tasa", IdTipoDato = 4, Referencia = "tasanual", Secuencia = 10, IdTipoFormula = 29, IdPlantillaElemento = 9, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Visible = true, Cadena = "tasa" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Aporte", IdTipoDato = 3, Referencia = "aporteInicial", Secuencia = 10, IdTipoFormula = 31, IdPlantillaElemento = 10, PeriodoInicial = "periodoInicial", PeriodoFinal = "periodoInicial", Visible = true, Cadena = "aporte" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Inversión", IdTipoDato = 3, Referencia = "inversion", Secuencia = 20, IdTipoFormula = 34, IdPlantillaElemento = 10, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Visible = false, Cadena = "aporte" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Costo capital", IdTipoDato = 3, Referencia = "costoCapital", Secuencia = 30, IdTipoFormula = 32, IdPlantillaElemento = 10, PeriodoInicial = "1", PeriodoFinal = "1", Visible = false, Cadena = "tasa * inversion" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Aporte", IdTipoDato = 3, Referencia = "aporteInicial", Secuencia = 10, IdTipoFormula = 31, IdPlantillaElemento = 11, PeriodoInicial = "periodoInicial", PeriodoFinal = "periodoInicial", Visible = true, Cadena = "aporte" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Plan de cierre", IdTipoDato = 3, Referencia = "planCierre", Secuencia = 10, IdTipoFormula = 34, IdPlantillaElemento = 12, PeriodoInicial = "Horizonte - PeriodosCierre + 1", PeriodoFinal = "Horizonte - PeriodosCierre + 1", Visible = true, Cadena = "montoInicial" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Plan de postcierre", IdTipoDato = 3, Referencia = "planPostCierre", Secuencia = 20, IdTipoFormula = 34, IdPlantillaElemento = 12, PeriodoInicial = "Horizonte - PeriodosCierre + 2", PeriodoFinal = "Horizonte", Visible = true, Cadena = "(presupuesto - montoInicial)/(PeriodosCierre - 1)" });
            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Gasto de garantías", IdTipoDato = 3, Referencia = "garantia", Secuencia = 30, IdTipoFormula = 33, IdPlantillaElemento = 12, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Visible = true, Cadena = "(presupuesto / (Horizonte - PeriodosPreOperativos - PeriodosCierre)) * costoGarantia * (Periodo - PeriodosPreOperativos)" });

            PlantillaFormulasRequester.AddElement(new PlantillaFormula { Nombre = "Monto", IdTipoDato = 3, Referencia = "monto", Secuencia = 10, IdTipoFormula = 36, IdPlantillaElemento = 13, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Visible = true, Cadena = "otroingreso" });

        }

        public void SeedPlantillaProyectos()
        {
            PlantillaProyectosRequester.AddElement(new PlantillaProyecto { Id = 1, Nombre = "Proyecto genérico" });
        }

        public void SeedPlantillaSalidaProyectos()
        {
            PlantillaSalidaProyectosRequester.AddElement(new PlantillaSalidaProyecto { Id = 1, Nombre = "EGP del proyecto", Secuencia = 10, IdPlantillaProyecto = 1, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre" });
            PlantillaSalidaProyectosRequester.AddElement(new PlantillaSalidaProyecto { Id = 2, Nombre = "Flujo de caja del proyecto", Secuencia = 20, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte" });
            PlantillaSalidaProyectosRequester.AddElement(new PlantillaSalidaProyecto { Id = 3, Nombre = "EGP financiero del proyecto", Secuencia = 30, IdPlantillaProyecto = 1, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre" });
            PlantillaSalidaProyectosRequester.AddElement(new PlantillaSalidaProyecto { Id = 4, Nombre = "Flujo de caja financiero del proyecto", Secuencia = 40, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte" });
            PlantillaSalidaProyectosRequester.AddElement(new PlantillaSalidaProyecto { Id = 5, Nombre = "Indicadores del proyecto", Secuencia = 50, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1" });
        }

        public void SeedPlantillaOperaciones()
        {
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id =  1, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia =  10, Nombre = "Préstamos", Referencia = "prestamos", IdTipoDato = 3, Cadena = "Financiamientos_Prestamo" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 2, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 20, Nombre = "Ventas", Referencia = "ventas", IdTipoDato = 3, Cadena = "Operativos_Ventas" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 3, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 30, Nombre = "Otros ingresos", Referencia = "otrosIngresos", IdTipoDato = 3, Cadena = "Otros_Ingresos" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 4, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 40, Nombre = "Valor residual", Referencia = "valorResidual", IdTipoDato = 3, Cadena = "ActivosFijos_ValorResidual" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 5, IdPlantillaProyecto = 1, PeriodoInicial = "Horizonte - PeriodosCierre", PeriodoFinal = "Horizonte - PeriodosCierre", Secuencia = 50, Nombre = "Recuperacion de capital", Referencia = "capital", IdTipoDato = 3, Cadena = "Financiamientos_Inversion + Inversiones_Inversion - ActivosFijos_Inversion - ActivosIntangibles_Inversion" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 6, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 60, Nombre = "Total ingresos", Referencia = "ingresos", IdTipoDato = 3, Cadena = "ventas + otrosIngresos + valorResidual + capital", Subrayar = true });

            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 7, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 70, Nombre = "Total ingresos", Referencia = "ingresosFin", IdTipoDato = 3, Cadena = "prestamos + ventas + otrosIngresos + valorResidual + capital", Subrayar = true });

            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 8, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 80, Nombre = "Inversiones", Referencia = "inversiones", IdTipoDato = 3, Cadena = "Inversiones_Aporte + Financiamientos_Prestamo" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 9, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 90, Nombre = "Costos", Referencia = "costos", IdTipoDato = 3, Cadena = "Operativos_Costos" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 10, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 100, Nombre = "Depreaciación de activos fijos", Referencia = "depreciacion", IdTipoDato = 3, Cadena = "ActivosFijos_Depreciacion" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 11, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 110, Nombre = "Amortización de activos intangibles", Referencia = "amortizacionIntangibles", IdTipoDato = 3, Cadena = "ActivosIntangibles_Amortizacion" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 12, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 120, Nombre = "Utilidad bruta", Referencia = "utilidadBruta", IdTipoDato = 3, Cadena = "ventas + otrosIngresos + valorResidual - costos - depreciacion - amortizacionIntangibles", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 13, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 130, Nombre = "Gastos administrativos", Referencia = "gastosAdmin", IdTipoDato = 3, Cadena = "Gastos_GastoAdministrativo" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 14, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 140, Nombre = "Gastos de venta", Referencia = "gastosVenta", IdTipoDato = 3, Cadena = "Gastos_GastoVenta" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 15, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 150, Nombre = "Utilidad operativa", Referencia = "utilidadOperativa", IdTipoDato = 3, Cadena = "utilidadBruta - gastosAdmin - gastosVenta", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 16, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 160, Nombre = "Amortización de préstamos", Referencia = "amortizacion", IdTipoDato = 3, Cadena = "Financiamientos_Amortizacion" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 17, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 170, Nombre = "Intereses", Referencia = "intereses", IdTipoDato = 3, Cadena = "Financiamientos_Intereses" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 18, IdPlantillaProyecto = 1, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Secuencia = 180, Nombre = "Regalía minera e Impuesto especial", Referencia = "impuestosOperativos", IdTipoDato = 3, Cadena = "utilidadOperativa * Impuestos_Operativo" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 19, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 190, Nombre = "Gastos de garantía", Referencia = "gastosGarantia", IdTipoDato = 3, Cadena = "Inversiones_Garantia" });

            //
            // del proyecto

            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 20, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 200, Nombre = "Utilidad antes de participaciones", Referencia = "utilidadPreParticpacion", IdTipoDato = 3, Cadena = "utilidadOperativa - impuestosOperativos - gastosGarantia", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 21, IdPlantillaProyecto = 1, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Secuencia = 210, Nombre = "Participaciones", Referencia = "participaciones", IdTipoDato = 3, Cadena = "utilidadPreParticpacion * Participaciones" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 22, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 220, Nombre = "Utilidad antes de impuestos", Referencia = "utilidadPreImpuestos", IdTipoDato = 3, Cadena = "utilidadPreParticpacion - participaciones", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 23, IdPlantillaProyecto = 1, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Secuencia = 230, Nombre = "Impuesto a la renta", Referencia = "impuestoRenta", IdTipoDato = 3, Cadena = "utilidadPreImpuestos * Impuestos_Renta" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 24, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 240, Nombre = "Utilidad neta", Referencia = "utilidadNeta", IdTipoDato = 3, Cadena = "utilidadPreImpuestos - impuestoRenta", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 25, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 250, Nombre = "Total egresos", Referencia = "egresos", IdTipoDato = 3, Cadena = "inversiones + costos + gastosAdmin + gastosVenta + impuestosOperativos + participaciones + impuestoRenta + gastosGarantia", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 26, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 260, Nombre = "Saldo final", Referencia = "saldo", IdTipoDato = 3, Cadena = "ingresos - egresos", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 27, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 270, Nombre = "Inversión del proyecto", Referencia = "inversionE", IdTipoDato = 3, Cadena = "Inversiones_Inversion + Financiamientos_Inversion", Indicador = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 28, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 280, Nombre = "Costo capital del proyecto", Referencia = "KE", IdTipoDato = 4, Cadena = "(Financiamientos_Costo * (1 - Impuestos_Renta) + Inversiones_Costo)/inversionE", Indicador = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 29, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 290, Nombre = "TIR del proyecto", Referencia = "TIRE", IdTipoDato = 4, Cadena = "Tir(saldo)", Indicador = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 30, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 300, Nombre = "VAN del proyecto", Referencia = "VANE", IdTipoDato = 3, Cadena = "Van(KE,saldo)", Indicador = true });
            
            //
            // del inversionista

            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 31, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 310, Nombre = "Gastos financieros", Referencia = "gastosFinancieros", IdTipoDato = 3, Cadena = "Financiamientos_Intereses" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 32, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 320, Nombre = "Utilidad antes de participaciones ", Referencia = "utilidadPreParticpacionFin", IdTipoDato = 3, Cadena = "utilidadOperativa - impuestosOperativos - gastosFinancieros - gastosGarantia", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 33, IdPlantillaProyecto = 1, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Secuencia = 330, Nombre = "Participaciones ", Referencia = "participacionesFin", IdTipoDato = 3, Cadena = "utilidadPreParticpacionFin * Participaciones" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 34, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 340, Nombre = "Utilidad antes de impuestos", Referencia = "utilidadPreImpuestosFin", IdTipoDato = 3, Cadena = "utilidadPreParticpacionFin - participacionesFin", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 35, IdPlantillaProyecto = 1, PeriodoInicial = "PeriodosPreOperativos + 1", PeriodoFinal = "Horizonte - PeriodosCierre", Secuencia = 350, Nombre = "Impuesto a la renta", Referencia = "impuestoRentaFin", IdTipoDato = 3, Cadena = "utilidadPreImpuestosFin * Impuestos_Renta" });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 36, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 360, Nombre = "Utilidad neta", Referencia = "utilidadNetaFin", IdTipoDato = 3, Cadena = "utilidadPreImpuestosFin - impuestoRentaFin", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 37, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 370, Nombre = "Total egresos", Referencia = "egresosFin", IdTipoDato = 3, Cadena = "inversiones + costos + gastosAdmin + gastosVenta + amortizacion + intereses + impuestosOperativos + participacionesFin + impuestoRentaFin + gastosGarantia", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 38, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "Horizonte", Secuencia = 380, Nombre = "Saldo final", Referencia = "saldoFin", IdTipoDato = 3, Cadena = "ingresosFin - egresosFin", Subrayar = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 39, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 390, Nombre = "Inversión del accionista", Referencia = "inversionF", IdTipoDato = 3, Cadena = "Inversiones_Inversion", Indicador = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 40, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 400, Nombre = "Costo capital de inversionista", Referencia = "KF", IdTipoDato = 4, Cadena = "(Inversiones_Costo)/inversionF", Indicador = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 41, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 410, Nombre = "TIR del inversionista", Referencia = "TIRF", IdTipoDato = 4, Cadena = "Tir(saldoFin)", Indicador = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 42, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 420, Nombre = "VAN del inversionista", Referencia = "VANF", IdTipoDato = 3, Cadena = "Van(KF,saldoFin)", Indicador = true });
        
            //
            //  del banco

            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 43, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 430, Nombre = "Inversión del banco", Referencia = "inversionB", IdTipoDato = 3, Cadena = "Financiamientos_Inversion", Indicador = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 44, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 440, Nombre = "TIR del banco", Referencia = "TIRB", IdTipoDato = 4, Cadena = "(Financiamientos_Costo)/inversionB", Indicador = true });
            PlantillaOperacionesRequester.AddElement(new PlantillaOperacion { Id = 45, IdPlantillaProyecto = 1, PeriodoInicial = "1", PeriodoFinal = "1", Secuencia = 450, Nombre = "VAN del banco", Referencia = "VANB", IdTipoDato = 3, Cadena = "VANE - VANF", Indicador = true });
        }

        public void SeedPlantillaSalidaOperaciones()
        {
            //
            // EGP del proyecto

            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion =  2, Secuencia = 1 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion =  3, Secuencia = 2 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 4, Secuencia = 3 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 9, Secuencia = 4 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 10, Secuencia = 5 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 11, Secuencia = 6 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 12, Secuencia = 7 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 13, Secuencia = 8 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 14, Secuencia = 9 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 15, Secuencia = 10 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 18, Secuencia = 11 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 19, Secuencia = 12 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 20, Secuencia = 13 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 21, Secuencia = 14 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 22, Secuencia = 15 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 23, Secuencia = 16 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 1, IdOperacion = 24, Secuencia = 17 });

            //
            // Flujo de caja del proyecto

            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 2, Secuencia = 1 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 3, Secuencia = 2 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 4, Secuencia = 3 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 5, Secuencia = 4 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 6, Secuencia = 5 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 8, Secuencia = 6 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 9, Secuencia = 7 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 13, Secuencia = 8 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 14, Secuencia = 9 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 18, Secuencia = 10 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 19, Secuencia = 11 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 21, Secuencia = 12 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 23, Secuencia = 13 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 24, Secuencia = 14 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 25, Secuencia = 15 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 28, Secuencia = 16 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 29, Secuencia = 17 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 2, IdOperacion = 30, Secuencia = 18 });

            //
            // EGP del inversionista

            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 2, Secuencia = 1 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 3, Secuencia = 2 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 4, Secuencia = 3 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 9, Secuencia = 4 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 10, Secuencia = 5 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 11, Secuencia = 6 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 12, Secuencia = 7 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 13, Secuencia = 8 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 14, Secuencia = 9 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 15, Secuencia = 10 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 18, Secuencia = 11 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 19, Secuencia = 12 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 31, Secuencia = 13 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 32, Secuencia = 14 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 33, Secuencia = 15 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 34, Secuencia = 16 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 35, Secuencia = 17 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 3, IdOperacion = 36, Secuencia = 18 });

            //
            // Flujo de caja del inversionista

            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 1, Secuencia = 1 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 2, Secuencia = 2 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 3, Secuencia = 3 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 4, Secuencia = 4 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 5, Secuencia = 5 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 7, Secuencia = 6 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 8, Secuencia = 7 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 9, Secuencia = 8 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 13, Secuencia = 9 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 14, Secuencia = 10 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 16, Secuencia = 11 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 17, Secuencia = 12 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 18, Secuencia = 13 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 19, Secuencia = 14 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 31, Secuencia = 15 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 33, Secuencia = 16 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 35, Secuencia = 17 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 37, Secuencia = 18 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 38, Secuencia = 19 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 40, Secuencia = 20 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 41, Secuencia = 21 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 4, IdOperacion = 42, Secuencia = 22 });

            //
            // Indicadores generales

            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 27, Secuencia = 1 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 28, Secuencia = 2 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 29, Secuencia = 3 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 30, Secuencia = 4 });

            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 39, Secuencia = 5 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 40, Secuencia = 6 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 41, Secuencia = 7 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 42, Secuencia = 8 });

            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 43, Secuencia = 9 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 44, Secuencia = 10 });
            PlantillaSalidaOperacionesRequester.AddElement(new PlantillaSalidaOperacion { IdSalida = 5, IdOperacion = 45, Secuencia = 11 });
        }
    }
}
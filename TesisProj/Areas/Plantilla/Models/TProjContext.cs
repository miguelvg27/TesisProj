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
        public DbSet<SubTipoElemento> SubTipoElementos { get; set; }
        public DbSet<TipoParametro> TipoParametros { get; set; }
        public DbSet<Parametro> Parametros { get; set; }
        public DbSet<PlantillaElemento> PlantillaElementos { get; set; }

        public DbRequester<TipoElemento> TipoElementosRequester { get; set; }
        public DbRequester<SubTipoElemento> SubTipoElementosRequester { get; set; }
        public DbRequester<TipoParametro> TipoParametrosRequester { get; set; }
        public DbRequester<Parametro> ParametrosRequester { get; set; }
        public DbRequester<PlantillaElemento> PlantillaElementosRequester { get; set; }

        public void RegistrarTablasPlantilla()
        {
            TipoElementosRequester = new DbRequester<TipoElemento>(this, TipoElementos);
            SubTipoElementosRequester = new DbRequester<SubTipoElemento>(this, SubTipoElementos);
            TipoParametrosRequester = new DbRequester<TipoParametro>(this, TipoParametros);
            ParametrosRequester = new DbRequester<Parametro>(this, Parametros);
            PlantillaElementosRequester = new DbRequester<PlantillaElemento>(this, PlantillaElementos);
        }

        public void SeedPlantilla()
        {
            SeedTipoElemento();
            SeedSubTipoElemento();
            SeedTipoParametro();
        }

        public void SeedTipoElemento()
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
        
        public void SeedSubTipoElemento()
        {
            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 1, Nombre = "Depreciación", IdTipoElemento = 1 });
            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 2, Nombre = "Amortización", IdTipoElemento = 2 });

            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 3, Nombre = "Gasto operativo", IdTipoElemento = 3 });
            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 4, Nombre = "Gasto administrativo", IdTipoElemento = 3 });
            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 5, Nombre = "Gasto de venta", IdTipoElemento = 3 });
            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 6, Nombre = "Gasto financiero", IdTipoElemento = 3 });

            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 7, Nombre = "Ventas", IdTipoElemento = 4 });
            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 8, Nombre = "Costos", IdTipoElemento = 4 });

            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id =  9, Nombre = "Saldo inicial", IdTipoElemento = 5 });
            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 10, Nombre = "Saldo final", IdTipoElemento = 5 });
            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 11, Nombre = "Amortización", IdTipoElemento = 5 });
            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 12, Nombre = "Cuota", IdTipoElemento = 5 });
            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 13, Nombre = "Intereses", IdTipoElemento = 5 });

            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 14, Nombre = "Renta", IdTipoElemento = 6 });
            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 15, Nombre = "Operativo", IdTipoElemento = 6 });

            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 16, Nombre = "Otros ingresos", IdTipoElemento = 9 });
            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 17, Nombre = "Otros egresos", IdTipoElemento = 9 });
            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 18, Nombre = "Ingresos extraordinarios", IdTipoElemento = 9 });
            SubTipoElementosRequester.AddElement(new SubTipoElemento { Id = 19, Nombre = "Egresos extraordinarios", IdTipoElemento = 9 });


        }

        public void SeedTipoParametro()
        {
            TipoParametrosRequester.AddElement(new TipoParametro { Id = 1, Nombre = "Real" });
            TipoParametrosRequester.AddElement(new TipoParametro { Id = 2, Nombre = "Entero" });
            TipoParametrosRequester.AddElement(new TipoParametro { Id = 3, Nombre = "Período" });
            TipoParametrosRequester.AddElement(new TipoParametro { Id = 4, Nombre = "Porcentaje" });
        }
    }
}
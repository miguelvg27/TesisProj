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
        public DbSet<TipoElemento> InternalTipoElementos { get; set; }
        public DbSet<SubTipoElemento> InternalSubTipoElementos { get; set; }
        public DbSet<TipoParametro> InternalTipoParametros { get; set; }

        public DbRequester<TipoElemento> TipoElementos { get; set; }
        public DbRequester<SubTipoElemento> SubTipoElementos { get; set; }
        public DbRequester<TipoParametro> TipoParametros { get; set; }

        public void RegistrarTablasPlantilla()
        {
            TipoElementos = new DbRequester<TipoElemento>(this, InternalTipoElementos);
            SubTipoElementos = new DbRequester<SubTipoElemento>(this, InternalSubTipoElementos);
            TipoParametros = new DbRequester<TipoParametro>(this, InternalTipoParametros);
        }

        public void SeedPlantilla()
        {
            SeedTipoElemento();
            SeedSubTipoElemento();
            SeedTipoParametro();
        }

        public void SeedTipoElemento()
        {
            TipoElementos.AddElement(new TipoElemento { Id = 1, Nombre = "Activo fijo" });
            TipoElementos.AddElement(new TipoElemento { Id = 2, Nombre = "Activo intangible" });
            TipoElementos.AddElement(new TipoElemento { Id = 3, Nombre = "Gasto" });
            TipoElementos.AddElement(new TipoElemento { Id = 4, Nombre = "Operativo" });
            TipoElementos.AddElement(new TipoElemento { Id = 5, Nombre = "Financiamiento" });
            TipoElementos.AddElement(new TipoElemento { Id = 6, Nombre = "Impuesto" });
            TipoElementos.AddElement(new TipoElemento { Id = 7, Nombre = "Participación" });
            TipoElementos.AddElement(new TipoElemento { Id = 8, Nombre = "Inversión" });
            TipoElementos.AddElement(new TipoElemento { Id = 9, Nombre = "Otros" });
        }
        
        public void SeedSubTipoElemento()
        {
            SubTipoElementos.AddElement(new SubTipoElemento { Id = 1, Nombre = "Depreciación", IdTipoElemento = 1 });
            SubTipoElementos.AddElement(new SubTipoElemento { Id = 2, Nombre = "Amortización", IdTipoElemento = 2 });

            SubTipoElementos.AddElement(new SubTipoElemento { Id = 3, Nombre = "Gasto operativo", IdTipoElemento = 3 });
            SubTipoElementos.AddElement(new SubTipoElemento { Id = 4, Nombre = "Gasto administrativo", IdTipoElemento = 3 });
            SubTipoElementos.AddElement(new SubTipoElemento { Id = 5, Nombre = "Gasto de venta", IdTipoElemento = 3 });
            SubTipoElementos.AddElement(new SubTipoElemento { Id = 6, Nombre = "Gasto financiero", IdTipoElemento = 3 });

            SubTipoElementos.AddElement(new SubTipoElemento { Id = 7, Nombre = "Ventas", IdTipoElemento = 4 });
            SubTipoElementos.AddElement(new SubTipoElemento { Id = 8, Nombre = "Costos", IdTipoElemento = 4 });

            SubTipoElementos.AddElement(new SubTipoElemento { Id =  9, Nombre = "Saldo inicial", IdTipoElemento = 5 });
            SubTipoElementos.AddElement(new SubTipoElemento { Id = 10, Nombre = "Saldo final", IdTipoElemento = 5 });
            SubTipoElementos.AddElement(new SubTipoElemento { Id = 11, Nombre = "Amortización", IdTipoElemento = 5 });
            SubTipoElementos.AddElement(new SubTipoElemento { Id = 12, Nombre = "Cuota", IdTipoElemento = 5 });
            SubTipoElementos.AddElement(new SubTipoElemento { Id = 13, Nombre = "Intereses", IdTipoElemento = 5 });

            SubTipoElementos.AddElement(new SubTipoElemento { Id = 14, Nombre = "Renta", IdTipoElemento = 6 });
            SubTipoElementos.AddElement(new SubTipoElemento { Id = 15, Nombre = "Operativo", IdTipoElemento = 6 });

            SubTipoElementos.AddElement(new SubTipoElemento { Id = 16, Nombre = "Otros ingresos", IdTipoElemento = 9 });
            SubTipoElementos.AddElement(new SubTipoElemento { Id = 17, Nombre = "Otros egresos", IdTipoElemento = 9 });
            SubTipoElementos.AddElement(new SubTipoElemento { Id = 18, Nombre = "Ingresos extraordinarios", IdTipoElemento = 9 });
            SubTipoElementos.AddElement(new SubTipoElemento { Id = 19, Nombre = "Egresos extraordinarios", IdTipoElemento = 9 });


        }

        public void SeedTipoParametro()
        {
            TipoParametros.AddElement(new TipoParametro { Id = 1, Nombre = "Real" });
            TipoParametros.AddElement(new TipoParametro { Id = 2, Nombre = "Entero" });
            TipoParametros.AddElement(new TipoParametro { Id = 3, Nombre = "Período" });
            TipoParametros.AddElement(new TipoParametro { Id = 4, Nombre = "Porcentaje" });
        }
    }
}
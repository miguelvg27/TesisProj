using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TesisProj.Areas.Distribuciones.Models;
using TesisProj.Areas.Modelos.Models;

namespace TesisProj.Models.Storage
{
    public partial class TProjContext : DbContext
    {
        public DbSet<Distribucion> InternalDistribucion { get; set; }
        public DbRequester<Distribucion> TablaDistribucion { get; set; }

        public void RegistrarTablasDistribucion()
        {
            TablaDistribucion = new DbRequester<Distribucion>(this, InternalDistribucion);
        }

        public void SeedDistribuciones()
        {
            Distribucion d1 = new Distribucion();
            d1.Id = 1;
            d1.IsEliminado = false;
            d1.Tipo = "Función de distrbución de variable aleatoria Discreta";
            d1.NombreCorto = "Distribucion Discreta";
            d1.Descripcion = "Sea X una variable aleatoria discreta. Se denomina " +
                           "función (ley, modelo o distribución) de probabilidad " +
                           "de X a la funcion f(x) definida por " +
                           "f(x) = P[X=x] en todo x número real y que satisface las siguientes condiciones";
            d1.Imagen = "~/Graficos/Discreta.png";
            TablaDistribucion.AddElement(d1);

            Distribucion d2 = new Distribucion();
            d2.Id = 2;
            d2.IsEliminado = false;
            d2.Tipo = "Función de distrbución de variable aleatoria Continua";
            d2.NombreCorto = "Distribucion Continua";
            d2.Descripcion = "Se dice que la función f(x) es función de densidad (ley, " +
                           "modelo o distribución) de probabilidad " +
                           "a la variable aleatoria continua X, si satisfacelas siguientes condiciones ";
            d2.Imagen = "~/Graficos/Continua.png";
            TablaDistribucion.AddElement(d2);
        }

    }
}
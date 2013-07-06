using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TesisProj.Areas.Proyectos.Models;


namespace TesisProj.Models.Storage
{
    public partial class TProjContext : DbContext
    {
        public DbSet<Pedro_Proyecto> InternalProyecto { get; set; }
        public DbRequester<Pedro_Proyecto> TablaProyecto { get; set; }

        public DbSet<Pedro_Parametro> InternalParametro { get; set; }
        public DbRequester<Pedro_Parametro> TablaParametro { get; set; }

        public DbSet<Pedro_Elemento> InternalElemento { get; set; }
        public DbRequester<Pedro_Elemento> TablaElemento { get; set; }

        public void RegistrarTablasProyectoPedro()
        {
            TablaProyecto = new DbRequester<Pedro_Proyecto>(this, InternalProyecto);
            TablaParametro = new DbRequester<Pedro_Parametro>(this, InternalParametro);
            TablaElemento = new DbRequester<Pedro_Elemento>(this, InternalElemento);
        }
 
    }
}


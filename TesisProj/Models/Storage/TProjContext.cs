using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using TesisProj.Areas.Plantilla.Models;

namespace TesisProj.Models.Storage
{
    public partial class TProjContext : DbContext
    {
        
        public TProjContext() : base("TProjDb") 
        {
            RegistrarTablas();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToOneConstraintIntroductionConvention>();
        }

        public void RegistrarTablas()
        {
            RegistrarTablasPlantilla();
            RegistrarTablasProyecto();
        }

        public void Seed(){
            SeedPlantilla();
            SeedProyecto();
        }
    }

    public class TProjInitializer : DropCreateDatabaseIfModelChanges<TProjContext>
//  public class TProjInitializer : DropCreateDatabaseAlways<TProjContext>
    {
        protected override void Seed(TProjContext context){
            context.Seed();
        }
    }
}
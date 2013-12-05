using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.ComponentModel;
using TesisProj.Models.Storage;
using TesisProj.Areas.Modelo.Models;

//  CoAutor: Walter Erquinigo

namespace TesisProj.Models.Storage
{
    public class DbRequester<T> where T : DbObject
    {
        public DbSet<T> Dbset { get; set; }
        public DbSet<Audit> DbAudit { get; set; }
        private DbContext context;
        private Type classType;

        public DbRequester(DbContext context, DbSet<T> dbset, DbSet<Audit> dbAudit = null)
        {
            this.context = context;
            this.DbAudit = dbAudit;
            Dbset = dbset;
            classType = typeof(T);
        }

        /*
         * Remueve un elemento dado su id. Se debe especificar cuando se quiera un eliminado fisico.
         */
        public void RemoveElementByID(int id, bool isFisico = true, bool log = false, int idProyecto = 0, int idUsuario = 0)
        {
            var elemento = Dbset.Find(id);

            if (elemento == null) throw new Exception("No existe el Id del registro en la base de datos.");
            if (!isFisico)
            {
                elemento.IsEliminado = true;
                ModifyElement(elemento);
            }
            else
            {
                if (log)
                {
                    DbAudit.Add(new Audit { IdProyecto = idProyecto, Fecha = DateTime.Now, IdUsuario = idUsuario, Transaccion = "Eliminar", TipoObjeto = classType.ToString(), Original = elemento.LogValues() });
                }

                Dbset.Remove(elemento);
                context.SaveChanges();
            }
        }

        /* 
         * Retorna todos los elementos en la tabla.
         */
        public List<T> All(bool isFisico = false)
        {
            var res = Dbset.ToList();
            if (res == null) return new List<T>();
            List<T> ans = new List<T>();
            foreach (var item in res)
                if (isFisico || !isFisico && !item.IsEliminado) ans.Add(item);
            return ans;
        }

        /*
         * Ejecuta Where. Si se desea obtener algun eliminado logico, se debe especificar aparte.
         */
        public List<T> Where(Func<T, bool> predicado, bool isFisico = false)
        {
            try
            {
                return Dbset.Where(predicado).Where(p => !p.IsEliminado || isFisico).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /*
         * Retorna un elemento que cumpla cierta condicion
         */
        public T One(Func<T, bool> predicado, bool isFisico = false)
        {
            try
            {
                return Dbset.Where(predicado).Where(p => !p.IsEliminado || isFisico).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Any(Func<T, bool> predicado, bool isFisico = false)
        {
            try
            {
                return Dbset.Where(predicado).Where(p => !p.IsEliminado || isFisico).Count() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /*
         * Busca un elemento por su ID.
         */
        public T FindByID(int id, bool isFisico = true)
        {
            try
            {
                var a = Dbset.Find(id);
                if (a.IsEliminado && !isFisico) return null;
                return a;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /*
         * Modifica en la base de datos el elemento dado por ID. El objeto camposCambiados debe tener null en todo campo que no cambia. El resto de campos
         * debe tener su nuevo valor. No se puede modificar el ID.
         */
        public void ModifyElement(T elemento_dirty, bool log = false, int idProyecto = 0, int idUsuario = 0)
        {
            if (!(elemento_dirty.Id >= 1))
                throw new Exception("El campo Id no puede ser nulo.");

            var elemento_db = Dbset.Find(elemento_dirty.Id);
            if (elemento_db == null) throw new Exception("No existe el Id del registro en la base de datos.");

            if (log && elemento_dirty is Celda)
            {
                Celda clean = elemento_db as Celda;
                Celda dirty = elemento_dirty as Celda;

                log = clean.Valor.Equals(dirty.Valor) ? false : true;
            }

            if (log)
            {
                DbAudit.Add(new Audit { IdProyecto = idProyecto, Fecha = DateTime.Now, IdUsuario = idUsuario, Transaccion = "Editar", TipoObjeto = classType.ToString(), Modificado = elemento_dirty.LogValues(), Original = elemento_db.LogValues() });
            }


            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(elemento_dirty))
            {
                if (!property.Name.Equals("Id") && property.GetValue(elemento_dirty) != null)
                {
                    property.SetValue(elemento_db, property.GetValue(elemento_dirty));
                }
            }
            context.Entry(elemento_db).State = EntityState.Modified;
            context.SaveChanges();
        }

        /*
         * Agrega el elemento dado a la base de datos. Esta funcion retorna el ID del elemento agregado.
         */
        public int AddElement(T elemento, bool log = false, int idProyecto = 0, int idUsuario = 0)
        {
            Dbset.Add(elemento);

            if (log)
            {
                DbAudit.Add(new Audit { IdProyecto = idProyecto, Fecha = DateTime.Now, IdUsuario = idUsuario, Transaccion = "Crear", TipoObjeto = classType.ToString(), Original = elemento.LogValues() });
            }

            context.SaveChanges();
            return elemento.Id;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.ComponentModel;
using TesisProj.Models.Storage;

//  Autor: Walter Erquinigo

namespace TesisProj.Models.Storage
{
    public class DbRequester<T> where T : DbObject
    {
        public DbSet<T> Dbset { get; set; }
        private DbContext context;
        private Type classType;

        public DbRequester(DbContext context, DbSet<T> dbset)
        {
            this.context = context;
            Dbset = dbset;
            classType = typeof(T);
        }

        /*
         * Remueve un elemento dado su id. Se debe especificar cuando se quiera un eliminado fisico.
         */
        public void RemoveElementByID(int id, bool isFisico = false)
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
        public void ModifyElement(T elemento_dirty)
        {
            if (!(elemento_dirty.Id >= 1))
                throw new Exception("El campo Id no puede ser nulo.");

            var elemento_db = Dbset.Find(elemento_dirty.Id);
            if (elemento_db == null) throw new Exception("No existe el Id del registro en la base de datos.");
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
        public int AddElement(T elemento)
        {
            Dbset.Add(elemento);
            context.SaveChanges();
            return elemento.Id;
        }

    }
}
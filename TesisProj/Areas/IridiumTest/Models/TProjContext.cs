using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using TesisProj.Areas.IridiumTest.Models;

namespace TesisProj.Models.Storage
{
    public partial class TProjContext : DbContext
    {
        public DbSet<ListField> listFields { get; set; }
        public DbRequester<ListField> ListFields { get; set; }

        public void RegistrarTablaImagenes()
        {
            ListFields = new DbRequester<ListField>(this, listFields, Audits);
        }

        public void SeedTablaImagenes()
        {
            List<ListField> lista = new List<ListField>();

            string root = System.IO.Directory.GetDirectories(System.AppDomain.CurrentDomain.BaseDirectory)[8];
            string[] files= System.IO.Directory.GetDirectories(root);
            foreach (string file in files)
            {
                string nombreModelo = file.Substring(root.Length + 2);
                string[] pngs=System.IO.Directory.GetFiles(file);
                foreach (string png in pngs)
                {
                    ListField l = new ListField();
                    l.Modelo = nombreModelo;
                    l.Atributo = png.Substring(file.Length + 1).Replace(".png", "");
                    l.Imagen=Image2Bytes(png);
                    lista.Add(l);
                }
            }

            foreach (ListField elemento in lista)
                ListFields.AddElement(elemento);
        }

        public byte[] Image2Bytes(String uri)
        {
            FileStream streams = new FileStream(uri, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(streams);
            Byte[] imagens = reader.ReadBytes((int)streams.Length);
            reader.Close(); streams.Close();
            return imagens;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.IridiumTest.Models
{
    [Table("ListField")]
    public class ListField : DbObject
    {
        public String Modelo { get; set; }
        public String Atributo { set;get;}
        public byte[] Imagen { get; set; }
    }
}
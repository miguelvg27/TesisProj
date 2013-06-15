using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

//  Autor: Walter Erquinigo

namespace TesisProj.Models.Storage
{
    public abstract class DbObject
    {
        [Key]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public bool IsEliminado { get; set; }
    }
}
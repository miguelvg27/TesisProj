﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Seguridad.Controllers
{
    public class SeguridadController : Controller
    {
        //
        // GET: /Seguridad/Seguridad/
        TProjContext context = new TProjContext();

        public ActionResult Index()
        {
            
            List<Proyecto> proyectos = context.Proyectos.Where(p => p.Creador.UserName == User.Identity.Name).ToList();
            return View(proyectos);
        }

        public ActionResult Colaboradores(int ProyectoID)
        {
            Session["ProyectoID"]=ProyectoID;
            Proyecto proyecto = context.Proyectos.Where(p => p.Id==ProyectoID).FirstOrDefault();
            ViewBag.proyecto = proyecto.Nombre;
            return View(proyecto.Colaboradores);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _Delete(int UserId)
        {
            Proyecto proyecto = context.Proyectos.Where(p => p.Id == (int)Session["ProyectoID"]).FirstOrDefault();
            List<UserProfile> actualizados = new List<UserProfile>();

            foreach (UserProfile u in proyecto.Colaboradores)
            {
                if (u.UserId != UserId)
                {
                    actualizados.Add(u);
                }
            }
            proyecto.Colaboradores = actualizados;
            context.Entry(proyecto).State = EntityState.Modified;
            context.SaveChanges();

            //return View(new GridModel(SessionProductRepository.All()));
            return View("Colaboradores", (int)Session["ProyectoID"]);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _Insert(UserProfile u)
        {
            Proyecto proyecto = context.Proyectos.Where(p => p.Id == (int)Session["ProyectoID"]).FirstOrDefault();
            bool existe=false;

            foreach(UserProfile x in proyecto.Colaboradores)
            {
                if(x.UserId==u.UserId)
                {
                    existe=true;
                }
            }
            
            if(existe)
            {
                proyecto.Colaboradores.Add(u);
                context.Entry(proyecto).State = EntityState.Modified;
                context.SaveChanges();
            }
            return View("Colaboradores", (int)Session["ProyectoID"]);
        }
    }
}
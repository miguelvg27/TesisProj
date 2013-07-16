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
            Session["ProyectoID"] = ProyectoID;
            Proyecto proyecto = context.Proyectos.Where(p => p.Id == ProyectoID).FirstOrDefault();
            ViewBag.proyecto = proyecto.Nombre;
            Session["colaboradores"] = context.UserProfiles.Where(c => c.UserName != User.Identity.Name).ToList();
            return View(proyecto.Colaboradores);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _Delete(int id, int ProyectoID)
        {
            Session["ProyectoID"] = ProyectoID;
            Proyecto proyecto = context.Proyectos.Find((int)Session["ProyectoID"]);
            List<UserProfile> actualizados = new List<UserProfile>();

            foreach (UserProfile u in proyecto.Colaboradores)
            {
                if (u.UserId != id)
                {
                    actualizados.Add(u);
                }
            }
            proyecto.Colaboradores = actualizados;
            context.Entry(proyecto).State = EntityState.Modified;
            context.SaveChanges();

            //return View(new GridModel(SessionProductRepository.All()));
            return View(new GridModel(context.Proyectos.Find((int)Session["ProyectoID"]).Colaboradores));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [GridAction]
        public ActionResult _Insert(FormCollection f)
        {
            Proyecto proyecto = context.Proyectos.Find((int)Session["ProyectoID"]);
            bool existe = true;
            UserProfile u = context.UserProfiles.Find(Convert.ToInt16(f["DropDownList"]));
            
            if (proyecto.Colaboradores.Count == 0)
            {
                proyecto.Colaboradores.Add(u);
                context.Entry(proyecto).State = EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                foreach (UserProfile x in proyecto.Colaboradores)
                {
                    if (x.UserId == u.UserId)
                    {
                        existe = false;
                    }
                }

                if (existe)
                {
                    proyecto.Colaboradores.Add(u);
                    context.Entry(proyecto).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            return View(new GridModel(context.Proyectos.Find((int)Session["ProyectoID"]).Colaboradores));
        }
    }
}
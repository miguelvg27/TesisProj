using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesisProj.Models.Storage;

namespace TesisProj.Filters
{
    public class ProjAuthorization : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string actionName = (string)filterContext.RouteData.Values["action"];
            bool idElemento = false;
            bool idProyecto = false;
            bool idFormula = false;
            bool idSalidaElemento = false;
            bool idParametro = false;

            var id = new object();

            if (filterContext.ActionParameters.ContainsKey("idProyecto"))
            {
                id = filterContext.ActionParameters["idProyecto"];
                idProyecto = true;
            }

            if (filterContext.ActionParameters.ContainsKey("idElemento"))
            {
                id = filterContext.ActionParameters["idProyecto"];
                idElemento = true;
            }

            if (idElemento || idProyecto)
            {

                switch (actionName)
                {
                    case "Console":
                    case "Journal":
                    case "Details":
                    case "Edit":
                    case "Delete":
                        idProyecto = true;
                        break;
                    case "Catalog":
                    case "Cuaderno":
                    case "Pizarra":
                    case "PutParametros":
                    case "SetParametos":
                    case "EditElemento":
                    case "DeleteElemento":
                        idElemento = true;
                        break;
                    case "EditParametro":
                    case "DeleteParametro":
                        idParametro = true;
                        break;
                    case "EditFormula":
                    case "DeleteFormula":
                        idFormula = true;
                        break;
                    case "EditSalidaElemento":
                    case "DeleteSalidaElemento":
                        idSalidaElemento = true;
                        break;
                }

                if (filterContext.ActionParameters.ContainsKey("id"))
                {
                    id = filterContext.ActionParameters["id"];
                }
            }

            int idObj = 0;
            try
            {
                idObj = (int)id;
            

                using(TProjContext context = new TProjContext())
                {
                    

                    if (idFormula)
                    {
                        idObj = context.Formulas.Find(idObj).IdElemento;
                    }

                    if (idParametro)
                    {
                        idObj = context.Parametros.Find(idObj).IdElemento;
                    }

                    if (idSalidaElemento)
                    {
                        idObj = context.SalidaElementos.Find(idObj).IdElemento;
                    }

                    if (!idProyecto)
                    {
                        idObj = context.Elementos.Find(idObj).IdProyecto;
                    }
                }

            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("/");
                
                return;
            }
            
        }
    }
}
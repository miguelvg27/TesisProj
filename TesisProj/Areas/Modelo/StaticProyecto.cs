using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Xml.Serialization;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Models.Storage;
using TesisProj.Areas.Modelo.Controllers;
using TesisProj.Areas.Plantilla.Models;

namespace TesisProj.Areas.Modelo
{
    public class StaticProyecto
    {
        private static List<double> StringToArray(string str)
        {
            List<double> arr = str.Split(',').Select(s => double.Parse(s)).ToList();
            return arr;
        }

        private static string ArrayToString(double[] arr)
        {
            string str = String.Join(",", arr.Select(p => p.ToString()).ToArray());
            return str;
        }

        public static ProyectoLite simular(int horizonte, int preoperativos, int cierre, List<Operacion> operaciones, List<Elemento> elementos, List<TipoFormula> tipoformulas, bool siSimular = true)
        {
            ProyectoLite resultado = new ProyectoLite();

            CalcularProyecto(horizonte, preoperativos, cierre, operaciones, elementos, tipoformulas, siSimular);

            Operacion op = operaciones.FirstOrDefault(o => o.Referencia.Equals("TIRE"));
            resultado.TirE = op != null ? op.Valores[0] : 0;

            op = operaciones.FirstOrDefault(o => o.Referencia.Equals("TIRF"));
            resultado.TirF = op != null ? op.Valores[0] : 0;

            op = operaciones.FirstOrDefault(o => o.Referencia.Equals("VANE"));
            resultado.VanE = op != null ? op.Valores[0] : 0;

            op = operaciones.FirstOrDefault(o => o.Referencia.Equals("VANF"));
            resultado.VanF = op != null ? op.Valores[0] : 0;

            return resultado;
        }

        private static void CalcularResultados(int id, TProjContext context)
        {
            Proyecto proyecto = context.Proyectos.Find(id);
            if (proyecto == null)
            {
                return;
            }

            int horizonte = proyecto.Horizonte;
            int preoperativos = proyecto.PeriodosPreOp;
            int cierre = proyecto.PeriodosCierre;
            var operaciones = context.Operaciones.Where(o => o.IdProyecto == id).ToList();
            var elementos = context.Elementos.Include(f => f.Formulas).Include(f => f.Parametros).Include("Parametros.Celdas").Where(e => e.IdProyecto == id).ToList();
            var tipoformulas = context.TipoFormulas.ToList();

            CalcularProyecto(horizonte, preoperativos, cierre, operaciones, elementos, tipoformulas);

            foreach (Operacion operacion in operaciones)
            {
                operacion.strValores = ArrayToString(operacion.Valores.ToArray());
                context.OperacionesRequester.ModifyElement(operacion);
            }

            return;
        }

        public static List<Operacion> CalcularProyecto(int horizonte, int preoperativos, int cierre, List<Operacion> operaciones, List<Elemento> elementos, List<TipoFormula> tipoformulas, bool simular = false)
        {
            foreach (TipoFormula tipoformula in tipoformulas)
            {
                tipoformula.Valores = new double[horizonte];
                Array.Clear(tipoformula.Valores, 0, horizonte);
            }

            foreach (Elemento elemento in elementos)
            {
                var refFormulas = new List<Formula>();
                var valFormulas = elemento.Formulas.OrderBy(f => f.Secuencia);
                var refParametros = elemento.Parametros;

                foreach (Formula formula in valFormulas)
                {
                    formula.Evaluar(horizonte, preoperativos, cierre, refFormulas, refParametros, simular);
                    refFormulas.Add(formula);

                    //  Sumo los elementos
                    var tipoformula = tipoformulas.First(t => t.Id == formula.IdTipoFormula);
                    tipoformula.Valores = tipoformula.Valores.Zip(formula.Valores, (x, y) => x + y).ToArray();
                }
            }

            var refOperaciones = new List<Operacion>();
            var valOperaciones = operaciones.OrderBy(o => o.Secuencia);
            foreach (Operacion operacion in operaciones)
            {
                operacion.Evaluar(horizonte, preoperativos, cierre, refOperaciones, tipoformulas);
                refOperaciones.Add(operacion);
            }

            return operaciones;
        }

        public static List<ProyectoLite> getProyectoList(TProjContext context, int idUser)
        {
            var proyectos = context.Colaboradores.Include(c => c.Proyecto).Where(c => c.IdUsuario == idUser).Select(c => c.Proyecto).ToList();
            List<ProyectoLite> resultados = new List<ProyectoLite>();

            foreach (Proyecto proyecto in proyectos)
            {
                var operaciones = context.Operaciones.Where(o => o.IdProyecto == proyecto.Id).ToList();
                if (operaciones.Any(o => o.strValores == null))
                {
                    CalcularResultados(proyecto.Id, context);
                    proyecto.Calculado = DateTime.Now;
                    context.ProyectosRequester.ModifyElement(proyecto);
                }

                ProyectoLite resultado = new ProyectoLite();
                resultado.Id = proyecto.Id;
                resultado.Nombre = proyecto.Nombre;
                resultado.Fecha = proyecto.Creacion;
                resultado.Version = proyecto.Version;

                Operacion op = operaciones.FirstOrDefault(o => o.Referencia.Equals("TIRE"));
                resultado.TirE = op != null ? StringToArray(op.strValores)[0] : 0;

                op = operaciones.FirstOrDefault(o => o.Referencia.Equals("TIRF"));
                resultado.TirF = op != null ? StringToArray(op.strValores)[0] : 0;

                op = operaciones.FirstOrDefault(o => o.Referencia.Equals("VANE"));
                resultado.VanE = op != null ? StringToArray(op.strValores)[0] : 0;

                op = operaciones.FirstOrDefault(o => o.Referencia.Equals("VANF"));
                resultado.VanF = op != null ? StringToArray(op.strValores)[0] : 0;

                resultados.Add(resultado);
            }

            return resultados;
        }

        public static List<ProyectoLite> getVersionesList(TProjContext context, int id)
        {
            List<ProyectoLite> resultados = new List<ProyectoLite>();
            Proyecto proyecto = context.Proyectos.Find(id);
            if (proyecto == null)
            {
                return resultados;
            }

            var versiones = context.DbVersions.Where(v => v.IdProyecto == proyecto.Id);

            foreach (DbVersion version in versiones)
            {
                XmlSerializer s = new XmlSerializer(typeof(Proyecto));
                MemoryStream memStream = new MemoryStream(version.Data);
                Proyecto proyecto_dirty = (Proyecto)s.Deserialize(memStream);
                var operaciones = proyecto_dirty.Operaciones;

                ProyectoLite resultado = new ProyectoLite();
                resultado.Id = version.Id;
                resultado.Nombre = version.Comentarios;
                resultado.Fecha = version.Fecha;
                resultado.Version = version.Version;

                Operacion op = operaciones.FirstOrDefault(o => o.Referencia.Equals("TIRE"));
                resultado.TirE = op != null ? StringToArray(op.strValores)[0] : 0;

                op = operaciones.FirstOrDefault(o => o.Referencia.Equals("TIRF"));
                resultado.TirF = op != null ? StringToArray(op.strValores)[0] : 0;

                op = operaciones.FirstOrDefault(o => o.Referencia.Equals("VANE"));
                resultado.VanE = op != null ? StringToArray(op.strValores)[0] : 0;

                op = operaciones.FirstOrDefault(o => o.Referencia.Equals("VANF"));
                resultado.VanF = op != null ? StringToArray(op.strValores)[0] : 0;

                resultados.Add(resultado);
            }

            return resultados;
        }
    }
}
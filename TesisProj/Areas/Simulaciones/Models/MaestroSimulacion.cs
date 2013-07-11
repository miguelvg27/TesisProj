﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesisProj.Areas.Modelo.Models;
using TesisProj.Areas.Modelos.Models;
using TesisProj.Models.Storage;

namespace TesisProj.Areas.Simulaciones.Models
{
    public class MaestroSimulacion 
    {
        public virtual ModeloSimlacion modelobase { get; set; }
        public virtual List<Celda> CeldasSensibles { get; set; }
        public virtual List<Grafico> Graficos { get; set; }

        public MaestroSimulacion(ModeloSimlacion m)
        {
            // aca le di los parametros iniciales al modelo
            this.Graficos = new List<Grafico>();
            this.CeldasSensibles = new List<Celda>();
            this.modelobase = m;
        }

        public void ActualizarCeldas(string modelo,Parametro parametro)
        {
            //aca genero los numeros aleatorios
            if (modelobase.Nombre.Equals("Normal"))
            {
                List<Celda> celdas = new List<Celda>();
                Graficos= modelobase.Normal.GenerarNumerosAleatorios(parametro.Celdas.Count);
                int i = 0;
                foreach (Grafico g in Graficos)
                {
                    decimal valor = Convert.ToDecimal(g.fx);
                    celdas.Add(new Celda { IdParametro = parametro.Celdas[i].IdParametro, Valor = valor, Periodo = parametro.Celdas[i].Periodo });
                    i++;
                }
                CeldasSensibles = celdas;                
            }

            if(modelobase.Nombre.Equals("Uniforme"))
            {
                List<Celda> celdas = new List<Celda>();
                Graficos = modelobase.Uniforme.GenerarNumerosAleatorios(parametro.Celdas.Count);
                int i = 0;
                foreach (Grafico g in Graficos)
                {
                    decimal valor = Convert.ToDecimal(g.fx);
                    celdas.Add(new Celda { IdParametro = parametro.Celdas[i].IdParametro, Valor = valor, Periodo = parametro.Celdas[i].Periodo });
                    i++;
                }
                CeldasSensibles = celdas;
            }
        }
    }
}
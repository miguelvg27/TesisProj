using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TesisProj.Areas.Modelos.Models;

namespace TesisProj.Models.Storage
{
    public partial class TProjContext : DbContext
    {
        public DbSet<ModeloSimlacion> InternalModeloSimulacion { get; set; }
        public DbRequester<ModeloSimlacion> TablaModeloSimulacion { get; set; }

        public DbSet<Normal> InternalNormal { get; set; }
        public DbRequester<Normal> TablaNormal { get; set; }

        public DbSet<Uniforme> InternalUniforme { get; set; }
        public DbRequester<Uniforme> TablaUniforme { get; set; }

        public DbSet<Binomial> InternalBinomial { get; set; }
        public DbRequester<Binomial> TablaBinomial { get; set; }

        public DbSet<Geometrica> InternalGeometrica { get; set; }
        public DbRequester<Geometrica> TablaGeometrica { get; set; }

        public DbSet<Hipergeometrica> InternalHipergeometrica { get; set; }
        public DbRequester<Hipergeometrica> TablaHipergeometrica { get; set; }

        public DbSet<Pascal> InternalPascal { get; set; }
        public DbRequester<Pascal> TablaPascal { get; set; }

        public DbSet<Poisson> InternalPoisson { get; set; }
        public DbRequester<Poisson> TablaPoisson { get; set; }

        public void RegistrarTablasModelo()
        {
            TablaModeloSimulacion = new DbRequester<ModeloSimlacion>(this, InternalModeloSimulacion);
            TablaNormal = new DbRequester<Normal>(this, InternalNormal);
            TablaUniforme = new DbRequester<Uniforme>(this, InternalUniforme);
            TablaBinomial = new DbRequester<Binomial>(this, InternalBinomial);
            TablaGeometrica = new DbRequester<Geometrica>(this, InternalGeometrica);
            TablaHipergeometrica = new DbRequester<Hipergeometrica>(this, InternalHipergeometrica);
            TablaPascal = new DbRequester<Pascal>(this, InternalPascal);
            TablaPoisson = new DbRequester<Poisson>(this, InternalPoisson);
        }

        //public void seedModelo()
        //{
        //    List<ModeloSimlacion> modelos = new List<ModeloSimlacion>
        //    {
        //        new Binomial {
        //            Id=1, 
        //            IsEliminado=false,
        //            Abreviatura="B(n,p)",
        //            Nombre="Binomial",
        //            Definicion="Combinatoria(n,k)*Potencia(p,k)*Potencia(q,n-k)",
        //            Descripcion="La distribución binomial es una distribución de probabilidad discreta "+
        //                        "que mide el número de éxitos en una secuencia de n ensayos de Bernoulli "+
        //                        "independientes entre sí, con una probabilidad fija p de ocurrencia del "+
        //                        "éxito entre los ensayos. Un experimento de Bernoulli se caracteriza por "+
        //                        "ser dicotómico, esto es, sólo son posibles dos resultados. "+
        //                        "A uno de estos se denomina éxito y tiene una probabilidad de ocurrencia p y "+
        //                        "al otro, fracaso, con una probabilidad q = 1 - p. "+
        //                        "En la distribución binomial el anterior experimento se repite n veces, "+
        //                        "de forma independiente, y se trata de calcular la probabilidad de un "+
        //                        "determinado número de éxitos. Para n = 1, la binomial se convierte, de hecho, "+
        //                        "en una distribución de Bernoulli.",
        //            distribucion=TablaDistribucion.One(d=>d.Id==1)
        //        },
                                        
        //        new Geometrica {
        //            Id=1, 
        //            IsEliminado=false,
        //            Abreviatura="G(p)",
        //            Nombre="Geometrica",
        //            Definicion="Potencia(p,1)*Potencia(q,k-1)",
        //            Descripcion="La Distribución Geométrica es una distribución de probabilidad discreta la cual "+
        //                        "mide hasta que ocurra el primer éxitos en una secuencia de n ensayos de Bernoulli "+
        //                        "sucesivas w independientes ",
        //            distribucion=TablaDistribucion.One(d=>d.Id==1)
        //        },

        //        new Hipergeometrica {
        //            Id=1, 
        //            IsEliminado=false,
        //            Abreviatura="H(N,n,r)",
        //            Nombre="Hipergeométrica",
        //            Definicion="Combinatoria(n,k)*Combinatoria(N-r,n-k)",
        //            Descripcion="Un experimento hipergeométrico consiste en escoger al azar una muestra de tamaño n, uno a uno sin restitución "+
        //                        ", de N elementos o resultados posibles, donde r de los cuales pueden clasificarse como éxitos, y "+
        //                        "los N-r restantes como fracasos. En cada extracción, la probabilidad de que el elemento sea un éxito es diferente"+
        //                        "ya que la extracción es sin reposición. \n\n"+
        //                        "Nota: El Numerador de la funcion de la probabilidad hipergeometrica, se requiere que "+
        //                        "N-r>=n-k, donde resulta que k>=n+r-N, luego el menor valor que toma la variable "+
        //                        "aleatoria X es el numero : \n\n"+
        //                        "a=max(0,n+r-N).\n\n"+
        //                        "Por Otro lado, el mayor valor que debe vereficar k<=n y k<r, luego, el mayor valor "+
        //                        "que toma Xpuede denotarse por:\n\n"+
        //                        "b=min(n,r).",
        //            distribucion=TablaDistribucion.One(d=>d.Id==1)
        //        },

        //        new Pascal
        //        {
        //            Id=1,
        //            IsEliminado=false,
        //            Abreviatura="P(r,p)",
        //            Nombre="Pascal",
        //            Definicion="Combinatoria(k-1,r-1)*Potencia(p,1)*Potencia(q,k-r)",
        //            Descripcion = "Se denomina experimento binomial negativo o de pascal "+
        //                          "a las repeticiones independientes de un experimento de "+
        //                          "aleatorio de Bernulli hasta obtener el éxito número r. "+
        //                          "En cada enseayo de Bernulli ´puede ocurrir un éxito con "+
        //                          "probabilidad p o un fracaso con probabilidad q=p-1.\n\n"+
        //                          "A la variable aleatoria X que se define como el número de "+
        //                          "intentos hasta que ocurra el éxito número r se le denomina" +
        //                          "variable aleatoria binomial negativa o de Pascal. Su rango "+
        //                          "es el conjunto: Rx = {r,r+1,r+2,... }\n\n"+
        //                          "Si k pertenece a Rx, el evento [X = k] ocurre, si resulta éxito "+
        //                          "en la k-ésima prueba y en los restantes k-1 pruebas resultan r-1 éxitos "+
        //                          "y (k-1)-(r-1) =k-r fracasos.\n\n",
        //            distribucion=TablaDistribucion.One(d=>d.Id==1)
        //        },

        //        new Poisson
        //        {
        //            Id=5,
        //            IsEliminado=false,
        //            Abreviatura="P(l)",
        //            Nombre="Poisson",
        //            Definicion=" ",
        //            Descripcion = "Se dice que la variabl ealeatoria discreta X cuyos valores  "+
        //                          "posibles son : 0,1,2,... tienen distribucion de Poisson con parametro l (l>0) "+
        //                          "si su funcion de Probabilidad es: \n\n",
        //            distribucion=TablaDistribucion.One(d=>d.Id==1)
        //        },

        //        new Uniforme
        //        {
        //            Id=6,
        //            IsEliminado=false,
        //            Abreviatura="U(a,b)",
        //            Nombre="Uniforme",
        //            Definicion=" ",
        //            Descripcion = "Se dice que la variable aleatoria continua X, tiene distribución "+
        //                          "uniforme (o rectangular) en el intervalo [a,b], a < b, y se describe por "+
        //                          "X - U[a,b], si su funcion de densidad de probabilidad es:\n\n",
        //            distribucion=TablaDistribucion.One(d=>d.Id==2)
        //        },

        //        new Normal
        //        {
        //            Id=7,
        //            IsEliminado=false,
        //            Abreviatura="N(u,o)",
        //            Nombre="Normal",
        //            Definicion=" ",
        //            Descripcion = "Se dice que la variable aleatoria continua X, que toma los valores reales "+
        //                          ", - inf < x < inf, es normal coon parametros u y o y se describe por "+
        //                          "X - N[u,o], si su funcion de densidad de probabilidad es:\n\n",
        //            distribucion=TablaDistribucion.One(d=>d.Id==2)
        //        },


        //    };
        //    foreach (ModeloSimlacion m in modelos)
        //    {
        //        TablaModeloSimulacion.AddElement(m);
        //    }
        //}
    }
}


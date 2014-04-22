using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesisProj.Areas.IridiumTest.Models
{
    public class Graphic
    {
        public int N { get; set; }
        public double fx { get; set; }
        public double Ac { get; set; }

        public void limpia()
        {
            this.fx = Math.Round(fx, 2);
            this.Ac = Math.Round(Ac, 2);
        }
    }

    public class GraphicList
    {
        public string N { get; set; }
        public string fx { get; set; }
        public string Ac { get; set; }

        public GraphicList() { }

        public GraphicList(double min, double max, double fx, double ac) 
        {
            this.N = "[" + Math.Round(min, 4).ToString() + " : " + Math.Round(max, 4).ToString() + "[";
            this.fx = Math.Round(fx, 4).ToString()+" %";
            this.Ac = Math.Round(ac, 4).ToString() + " %";
        }
    }
}

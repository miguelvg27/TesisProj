using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IridiumTest.Models
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
}

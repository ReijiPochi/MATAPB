using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATAPB
{
    public class MatColor
    {
        public MatColor(double a, double r, double g, double b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public double A { get; set; }
        public double R { get; set; }
        public double G { get; set; }
        public double B { get; set; }

        public static MatColor Black { get; } = new MatColor(1.0, 0.0, 0.0, 0.0);
        public static MatColor Red { get; } = new MatColor(1.0, 1.0, 0.0, 0.0);
        public static MatColor Green { get; } = new MatColor(1.0, 0.0, 1.0, 0.0);
        public static MatColor Blue { get; } = new MatColor(1.0, 0.0, 0.0, 1.0);
    }
}

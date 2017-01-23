using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MATAPB.Objects
{
    public class Circle : Primitive.Line
    {
        public Circle() : base()
        {
            CountOfPoints = 50;
        }

        private bool valueChanged = true;

        private double _Radius = 1.0;
        public double Radius
        {
            get { return _Radius; }
            set
            {
                if (_Radius == value)
                    return;

                _Radius = value;
                valueChanged = true;
            }
        }

        public override Vector3 LineFormula(double a)
        {
            double angle = a * Math.PI * 2;
            return new Vector3((float)(Math.Cos(angle) * Radius), (float)(Math.Sin(angle) * Radius), 0);
        }

        public override void Draw(RenderingContext context)
        {
            if (valueChanged)
            {
                valueChanged = false;
                UpdateBuffer();
            }

            base.Draw(context);
        }
    }
}

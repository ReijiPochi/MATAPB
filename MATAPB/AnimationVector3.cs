using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATAPB
{
    public class AnimationVector3 : AnimationObject
    {
        private bool valueChanged;

        private double _X;
        public double X
        {
            get { return _X; }
            set
            {
                valueChanged = true;
                _X = value;
            }
        }

        private double _Y;
        public double Y
        {
            get { return _Y; }
            set
            {
                valueChanged = true;
                _Y = value;
            }
        }

        private double _Z;
        public double Z
        {
            get { return _Z; }
            set
            {
                valueChanged = true;
                _Z = value;
            }
        }

        public bool ValueChanged { get; private set; }
        public AnimationMode Mode { get; set; } = AnimationMode.Step;
        public double ActualX { get; set; }
        public double ActualY { get; set; }
        public double ActualZ { get; set; }

        public double Delta { get; set; } = 0.1;
        public double Threshold { get; set; } = 0.1;

        protected override void AnimationOneFrame()
        {
            if (!valueChanged)
            {
                ValueChanged = false;
                return;
            }

            switch (Mode)
            {
                case AnimationMode.Step:
                    ActualX = _X;
                    ActualY = _Y;
                    ActualZ = _Z;
                    break;

                case AnimationMode.Liner:
                    ValueChanged = true;

                    if (ActualX < _X)
                        ActualX += Delta;
                    else if (ActualX > _X)
                        ActualX -= Delta;

                    if (Math.Abs(ActualX - _X) < Threshold)
                    {
                        ActualX = _X;
                        valueChanged = false;
                    }

                    if (ActualY < _Y)
                        ActualY += Delta;
                    else if (ActualY > _Y)
                        ActualY -= Delta;

                    if (Math.Abs(ActualY - _Y) < Threshold)
                    {
                        ActualY = _Y;
                        valueChanged = false;
                    }

                    if (ActualZ < _Z)
                        ActualZ += Delta;
                    else if (ActualZ > _Z)
                        ActualZ -= Delta;

                    if (Math.Abs(ActualZ - _Z) < Threshold)
                    {
                        ActualX = _Z;
                        valueChanged = false;
                    }
                    break;

                default:
                    break;
            }
        }
    }
}

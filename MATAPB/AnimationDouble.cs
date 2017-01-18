using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATAPB
{
    public class AnimationDouble : AnimationObject
    {
        private double _Value;
        public double Value
        {
            get { return _Value; }
            set
            {
                valueChanged = true;
                _Value = value;
            }
        }

        private bool valueChanged;
        public bool ValueChanged { get; private set; }
        public AnimationMode Mode { get; set; } = AnimationMode.Step;
        public double ActualValue { get; set; }

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
                    ActualValue = _Value;
                    break;

                case AnimationMode.Liner:
                    ValueChanged = true;
                    if (ActualValue < _Value)
                        ActualValue += Delta;
                    else if (ActualValue > Value)
                        ActualValue -= Delta;

                    if (Math.Abs(ActualValue - _Value) < Threshold)
                    {
                        ActualValue = _Value;
                        valueChanged = false;
                    }
                    break;

                default:
                    break;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATAPB.Gaming
{
    public delegate void MineHitEventHandler();

    public delegate void MineLeaveEventHandler();

    public class Mine : ActionObject
    {
        public MatVector3 MinePosition { get; set; } = new MatVector3();

        public double MineRadius { get; set; } = 1.0;

        public double Hysteresis { get; set; } = 0.1;

        public MatVector3 TargetPosition { get; set; } = new MatVector3();

        public event MineHitEventHandler MineHit;
        public event MineLeaveEventHandler MineLeave;

        private bool hit;

        public override void Action()
        {
            if (!hit)
            {
                if (MatVector3.Distance(MinePosition, TargetPosition) < MineRadius - Hysteresis)
                {
                    hit = true;
                    MineHit?.Invoke();
                }
            }
            else
            {
                if(MatVector3.Distance(MinePosition, TargetPosition) > MineRadius + Hysteresis)
                {
                    hit = false;
                    MineLeave?.Invoke();
                }
            }
        }
    }
}

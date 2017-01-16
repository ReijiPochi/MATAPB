using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector3 = System.Numerics.Vector3;

namespace MATAPB.Gaming
{
    public delegate void MineHitEventHandler();

    public delegate void MineLeaveEventHandler();

    public class Mine : ActionObject
    {
        public Vector3 MinePosition { get; set; } = new Vector3();

        public double MineRadius { get; set; } = 1.0;

        public double Hysteresis { get; set; } = 0.1;

        public Vector3 TargetPosition { get; set; } = new Vector3();

        public event MineHitEventHandler MineHit;
        public event MineLeaveEventHandler MineLeave;

        private bool hit = false;

        public override void Action()
        {
            if (!hit)
            {
                if (Vector3.Distance(MinePosition, TargetPosition) < MineRadius - Hysteresis)
                {
                    hit = true;
                    MineHit?.Invoke();
                }
            }
            else
            {
                if(Vector3.Distance(MinePosition, TargetPosition) > MineRadius + Hysteresis)
                {
                    hit = false;
                    MineLeave?.Invoke();
                }
            }
        }
    }
}

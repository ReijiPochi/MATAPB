using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATAPB.Timeline
{
    public class TimelineObject
    {
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public bool Running { get; protected set; }

        public Timeline Host { get; set; }

        public virtual void Start()
        {
            Running = true;
        }

        public virtual void OnRunning(double currentTime)
        {

        }

        public virtual void End()
        {
            Running = false;
        }
    }
}

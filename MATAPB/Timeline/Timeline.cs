using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATAPB.Timeline
{
    public enum TimelineState
    {
        Stop,
        Run,
        Pause
    }

    public class Timeline : AnimationObject
    {
        private List<TimelineObject> Objects { get; } = new List<TimelineObject>();

        public double CurrentTime { get; protected set; } = 0.0;
        public TimelineState State { get; protected set; } = TimelineState.Stop;

        public void Add(TimelineObject obj, double start, double end)
        {
            obj.Host = this;
            obj.StartTime = start;
            obj.EndTime = end;
            Objects.Add(obj);
        }

        public void Start()
        {
            State = TimelineState.Run;
        }

        public void Pouse()
        {
            State = TimelineState.Pause;
        }

        public void Stop()
        {
            CurrentTime = 0.0;
            State = TimelineState.Stop;
        }

        protected override void AnimationOneFrame()
        {
            if (State != TimelineState.Run)
                return;

            CurrentTime += PresentationBase.TimelengthOfFrame;

            foreach(TimelineObject to in Objects)
            {
                if (to.Running)
                {
                    if(to.EndTime < CurrentTime)
                    {
                        to.End();
                    }
                }
                else
                {
                    if (to.StartTime < CurrentTime && CurrentTime < to.EndTime)
                    {
                        to.Start();
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MATAPB
{
    public delegate void TickEventHandler(long time);

    public class Clock : AutoDisposeObject
    {
        public Clock(double interval)
        {
            Interval = interval;
        }

        private Thread thread;

        public bool IsRunning { get; private set; }

        public bool IsCounting { get; private set; }

        public double Interval { get; set; }

        public event TickEventHandler Tick;
        protected void RaiseTick(long time)
        {
            Tick?.Invoke(time);
        }

        public void Start()
        {
            if (!IsRunning)
            {
                thread = new Thread(Work);
                IsRunning = true;
                IsCounting = true;
                thread.Start();
            }
            else
            {
                IsCounting = true;
            }
        }

        public void Pause()
        {
            IsCounting = false;
        }

        public void Stop()
        {
            IsRunning = false;
            IsCounting = false;

            if (thread != null) thread.Join();
        }

        private void Work()
        {
            double nextTick = 0;

            while (IsRunning)
            {
                if (!IsCounting)
                {
                    Thread.Sleep(1);
                    continue;
                }

                DateTime now = DateTime.Now;
                long current = now.Hour * 3600000 + now.Minute * 60000 + now.Second * 1000 + now.Millisecond;

                if (nextTick == 0)
                    nextTick = current + Interval;

                if (current >= nextTick)
                {
                    nextTick += Interval;

                    //if (current >= nextTick)
                    //    nextTick = current + nextTick;

                    RaiseTick(current);
                }

                Thread.Sleep(1);
            }
        }

        protected override void OnDispose()
        {
            IsRunning = false;
            if (thread != null) thread.Join();
        }
    }
}

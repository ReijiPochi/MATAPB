using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATAPB.Gaming
{
    public abstract class ActionObject
    {
        public ActionObject()
        {
            if (locking)
            {
                stackList.Add(this);
            }
            else
            {
                list.Add(this);
            }
        }

        private static bool locking;
        private static List<ActionObject> stackList = new List<ActionObject>();

        private static bool trasitioning, acing, waitingToEndActing;

        private static List<ActionObject> list = new List<ActionObject>();

        public static void Lock()
        {
            locking = true;
        }

        public static void Unlock()
        {
            if (acing)
            {
                waitingToEndActing = true;
                return;
            }

            trasitioning = true;

            foreach (ActionObject ao in stackList)
            {
                list.Add(ao);
            }

            stackList.Clear();

            trasitioning = false;
        }

        public static void DoAction()
        {
            while (trasitioning) { }

            if (waitingToEndActing)
            {
                Unlock();
                waitingToEndActing = false;
            }

            acing = true;

            foreach (ActionObject ao in list)
            {
                ao.Action();
            }

            acing = false;
        }

        public abstract void Action();
    }
}

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
            list.Add(this);
        }

        ~ActionObject()
        {
            list.Remove(this);
        }

        private static List<ActionObject> list = new List<ActionObject>();

        public static void DoAction()
        {
            foreach(ActionObject ao in list)
            {
                ao.Action();
            }
        }

        public abstract void Action();
    }
}

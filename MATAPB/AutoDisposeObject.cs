using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MATAPB
{
    public delegate void AllDisposingEventHandler();

    public abstract class AutoDisposeObject : IDisposable
    {
        static AutoDisposeObject()
        {
            Application.Current.Exit += Current_Exit;
        }

        public AutoDisposeObject()
        {
            list.Add(this);
        }

        private static List<AutoDisposeObject> list = new List<AutoDisposeObject>();

        public static event AllDisposingEventHandler AllDisposing;

        private static void Current_Exit(object sender, ExitEventArgs e)
        {
            AllDisposing?.Invoke();

            while(list.Count > 0)
            {
                list[0].Dispose();
            }
        }


        public virtual void Dispose()
        {
            OnDispose();
            list.Remove(this);
        }

        protected abstract void OnDispose();
    }
}

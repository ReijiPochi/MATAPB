using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATAPB
{
    public class AnimationObject : AutoDisposeObject
    {
        public AnimationObject()
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

        public static bool Pause { get; set; } = false;

        private static List<AnimationObject> list = new List<AnimationObject>();

        private static bool locking;
        private static List<AnimationObject> stackList = new List<AnimationObject>();

        private static bool trasitioning, animating, waitingToEndAnimating;

        public static void Lock()
        {
            locking = true;
        }

        public static void Unlock()
        {
            if (animating)
            {
                waitingToEndAnimating = true;
                return;
            }

            trasitioning = true;

            foreach(AnimationObject ao in stackList)
            {
                list.Add(ao);
            }

            stackList.Clear();

            trasitioning = false;
        }

        public static void DoAnimation()
        {
            while (trasitioning) { }

            if(waitingToEndAnimating)
            {
                Unlock();
                waitingToEndAnimating = false;
            }

            if(!Pause)
            {
                animating = true;

                foreach (AnimationObject ao in list)
                {
                    ao.AnimationOneFrame();
                }

                animating = false;
            }
        }

        protected virtual void AnimationOneFrame()
        {

        }

        protected override void OnDispose()
        {
            list.Remove(this);
        }
    }
}

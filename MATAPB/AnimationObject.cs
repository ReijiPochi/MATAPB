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

        private static List<AnimationObject> list = new List<AnimationObject>();

        private static bool locking;
        private static List<AnimationObject> stackList = new List<AnimationObject>();

        public static void Lock()
        {
            locking = true;
        }

        public static void Unlock()
        {
            foreach(AnimationObject ao in stackList)
            {
                list.Add(ao);
            }

            stackList.Clear();
        }

        public static void DoAnimation()
        {
            foreach(AnimationObject ao in list)
            {
                ao.AnimationOneFrame();
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

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
            list.Add(this);
        }

        ~AnimationObject()
        {
            list.Remove(this);
        }

        private static List<AnimationObject> list = new List<AnimationObject>();

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
            
        }
    }
}

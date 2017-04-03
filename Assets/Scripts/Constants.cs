using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Constants
{
    struct AnimationTuple
    {
        public string parameter;
        public bool value;

        public AnimationTuple(string parameter, bool value)
        {
            this.parameter = parameter;
            this.value = value;
        }
    }

    internal class AnimationTuples
    {
        internal static AnimationTuple introAnimation = new AnimationTuple("IntroAnimationIn", true);
        internal static AnimationTuple exitAnimation = new AnimationTuple("IntroAnimationIn", false);
    }
}

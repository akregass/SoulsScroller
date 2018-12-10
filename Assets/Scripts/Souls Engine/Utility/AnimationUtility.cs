using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsEngine.Utility.Animation
{

    public enum ActorState
    {
        IDLE,
        MOVING,
        ATTACKING,
        BLOCKING
    }

    public class AnimationManager
    {
        public Animator animator;
        public Dictionary<string, int> animInt;
        public Dictionary<string, float> animFloat;
        public Dictionary<string, bool> animBool;

        public int AnimatorState { get; set; }

        public void FreezeAnimation() { animator.speed = 0; }

        public void UnfreezeAnimation() { animator.speed = 1; }

        public bool CheckState(string name) { return (animator.GetCurrentAnimatorStateInfo(0).IsName(name)) ? true : false; }

        public void UpdateInt(string parameter, int value) { animInt[parameter] = value; }

        public void UpdateFloat(string parameter, float value) { animFloat[parameter] = value; }

        public void UpdateBool(string parameter, bool value) { animBool[parameter] = value; }

        public void PlayAnimation(string animation)
        {
            animator.Play(animation);
        }

        public void CrossFadeAnimation(string animation, float duration)
        {
            animator.CrossFade(animation, duration);
        }

        public void UpdateAnimator()
        {
            //ClearAnimator ();

            if (animInt.Count > 0)
            {
                foreach (KeyValuePair<string, int> flt in animInt)
                    animator.SetInteger(flt.Key, flt.Value);
            }

            if (animFloat.Count > 0)
            {
                foreach (KeyValuePair<string, float> flt in animFloat)
                    animator.SetFloat(flt.Key, flt.Value);
            }

            if (animBool.Count > 0)
            {
                foreach (KeyValuePair<string, bool> flt in animBool)
                    animator.SetBool(flt.Key, flt.Value);
            }
        }

        private void ClearAnimator()
        {
            foreach (KeyValuePair<string, int> flt in animInt)
                animInt[flt.Key] = 0;

            foreach (KeyValuePair<string, float> flt in animFloat)
                animFloat[flt.Key] = 0f;

            foreach (KeyValuePair<string, bool> flt in animBool)
                animBool[flt.Key] = false;
        }
        
    }

    public class AnimationUtility
    {
        
    }
}
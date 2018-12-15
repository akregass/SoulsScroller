using System.Collections.Generic;
using UnityEngine;
using SoulsEngine.Utility.Animation;

namespace SoulsEngine
{
    public class AnimationManager : MonoBehaviour
    {
        Animator Animator { get; set; }
        Actor Actor { get; set; }

        public ActorState State { get; set; }

        public bool InCombat { get; set; }

        int _State;
        int _InCombat;

        void Start()
        {
            Animator = GetComponent<Animator>();
            Actor = GetComponent<Actor>();
            State = ActorState.IDLE;

            _State = Animator.StringToHash("Base Layer.State");
            _InCombat = Animator.StringToHash("Base Layer.In Combat");
        }

        void LateUpdate()
        {
            UpdateAnimatorField(_State, (int)State);
            UpdateAnimatorField(_InCombat, InCombat); 
        }

        public void SetState(ActorState _state)
        {
            State = _state;
        }

        void UpdateAnimatorField(int field, int value)
        {
            Animator.SetInteger(field, value);
        }

        void UpdateAnimatorField(int field, bool value)
        {
            Animator.SetBool(field, value);
        }

        void UpdateAnimatorField(int field, float value)
        {
            Animator.SetFloat(field, value);
        }
    }
}

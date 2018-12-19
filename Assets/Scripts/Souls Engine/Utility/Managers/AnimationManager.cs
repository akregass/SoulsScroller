using System.Collections.Generic;
using UnityEngine;
using SoulsEngine.Utility.Animation;

namespace SoulsEngine
{
    public class AnimationManager : MonoBehaviour
    {
        Animator Animator { get; set; }
        Actor Actor { get; set; }

        [SerializeField]
        ActorState state;
        public ActorState State { get { return state; } set { state = value; } }

        [SerializeField]
        bool lockStateChange = false;
        HashSet<ActorState> stateChangeQueue;

        int _State;
        int _InCombat;
        int _TakingHit;
        int _Attacking;

        void Start()
        {
            Animator = GetComponent<Animator>();
            Actor = GetComponent<Actor>();
            State = ActorState.IDLE;

            stateChangeQueue = new HashSet<ActorState>();

            _State = Animator.StringToHash("State");
            _InCombat = Animator.StringToHash("In Combat");
            _TakingHit = Animator.StringToHash("Taking Hit");
            _Attacking = Animator.StringToHash("Attacking");
        }

        void LateUpdate()
        {
            if (!lockStateChange)
            {
                if (stateChangeQueue.Contains(ActorState.ATTACKING))
                {
                    State = ActorState.ATTACKING;
                }
                else if (stateChangeQueue.Contains(ActorState.DASHING))
                {
                    State = ActorState.DASHING;
                }
                else if (stateChangeQueue.Contains(ActorState.JUMPING))
                {
                    State = ActorState.JUMPING;
                }
                else if (stateChangeQueue.Contains(ActorState.RUNNING))
                {
                    State = ActorState.RUNNING;
                }
                else if (stateChangeQueue.Contains(ActorState.IDLE))
                {
                    State = ActorState.IDLE;
                }
            }

            if (stateChangeQueue.Contains(ActorState.DYING))
            {
                State = ActorState.DYING;
            }
            else if (stateChangeQueue.Contains(ActorState.HIT))
            {
                State = ActorState.HIT;
            }

            if (Animator.GetInteger(_State) != (int)State)
                UpdateAnimatorParam(_State, (int)State);

            stateChangeQueue.Clear();
        }

        public void SetState(ActorState _state)
        {
            stateChangeQueue.Add(_state);
        }

        void UpdateAnimatorParam(int field, int value)
        {
            Animator.SetInteger(field, value);
        }

        void UpdateAnimatorParam(int field, bool value)
        {
            Animator.SetBool(field, value);
        }

        void UpdateAnimatorParam(int field, float value)
        {
            Animator.SetFloat(field, value);
        }
    }
}

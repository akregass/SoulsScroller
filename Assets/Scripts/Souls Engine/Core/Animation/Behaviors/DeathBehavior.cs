using UnityEngine;

public class DeathBehavior : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        var actor = animator.GetComponent<Actor>();
        actor.Death(3);
    }
}
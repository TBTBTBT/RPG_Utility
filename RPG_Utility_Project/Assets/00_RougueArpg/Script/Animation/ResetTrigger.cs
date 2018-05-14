using UnityEngine;

public class ResetTrigger : StateMachineBehaviour
{

    [SerializeField]
    string triggerName;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(triggerName);
    }
}
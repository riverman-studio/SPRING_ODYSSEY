using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chainReactStates : StateMachineBehaviour
{
    public enum StateBehaviour { StateEnter, StateExit };

    [System.Serializable]
    public class animatorChain
    {
        [SerializeField]
        private Animator subAnimator;
        [SerializeField]
        private string triggerName;

        [SerializeField]
        public StateBehaviour triggerBehaviour;
        // optionally some other fields

        public void triggerIt()
        {
            subAnimator.SetTrigger(triggerName);
        }
    }

    [SerializeField]
    private List<animatorChain> chainReactionList;

     //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       for(int i=0; i<chainReactionList.Count; i++)
        {
            if(chainReactionList[i].triggerBehaviour == StateBehaviour.StateEnter)
            {
                chainReactionList[i].triggerIt();
            }
        }
    }
     //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < chainReactionList.Count; i++)
        {
            if (chainReactionList[i].triggerBehaviour == StateBehaviour.StateExit)
            {
                chainReactionList[i].triggerIt();
            }
        }
    }



    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}



    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

using ProjectB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectB
{
    public class CharacterAnimStateMachine_Idle : StateMachineBehaviour
    {
        [SerializeField] float MoveBlockTransition = 0.03f;

        bool bIsTransitionCompleted = true;
        bool bIsEntering = false;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            bIsEntering = true;
        }
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime > MoveBlockTransition && !bIsTransitionCompleted)
            {
                bIsTransitionCompleted = true;
                if (bIsEntering)
                {
                    OnEnterTransitionCompleted(animator, stateInfo, layerIndex);
                    bIsEntering=false;
                }
            }
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            bIsTransitionCompleted = false;
        }

        void OnEnterTransitionCompleted(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("tlfgod");
            PlayerController playerController = animator.GetComponentInParent<PlayerController>();
            if (playerController)
            {
                playerController.bIsSit = false;
            }
        }
    }
}

using ProjectB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimStateMachine_Hoeing : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        
        PlayerController playerController = animator.GetComponentInParent<PlayerController>();
        if (playerController)
        {
            Farmland farmland = playerController.focusedInteractableObj.GetComponent<Farmland>();
            if (farmland)
            {
                farmland.bIsReady = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nangkuri_Hurt : StateMachineBehaviour
{
    Base_Nangkuri nangkuriBase;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nangkuriBase = animator.GetComponent<Base_Nangkuri>();

        nangkuriBase.PlayerHurt.enabled = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<Base_Nangkuri>().DetactGround())
            animator.SetTrigger("Pass");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nangkuriBase.PlayerHurt.enabled = true;
    }
}

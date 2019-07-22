using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nangkuri_Idle : StateMachineBehaviour
{
    Base_Nangkuri nangkuriBase;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nangkuriBase = animator.GetComponent<Base_Nangkuri>();
        animator.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, animator.GetComponent<Rigidbody2D>().velocity.y);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nangkuriBase.SetRandomNumber(1);
    }

}

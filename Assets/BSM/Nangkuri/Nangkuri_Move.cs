using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nangkuri_Move : StateMachineBehaviour
{
    Base_Nangkuri nangkuriBase;
    Rigidbody2D rBody;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nangkuriBase = animator.GetComponent<Base_Nangkuri>();
        rBody = animator.GetComponent<Rigidbody2D>();

        if(Random.Range(0,1) == 0)
            nangkuriBase.SwitchDirection();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nangkuriBase.UpdateRays();

        if (nangkuriBase.detactChangeDir)
            nangkuriBase.SwitchDirection();
        else if (nangkuriBase.detactForward)
            nangkuriBase.SwitchDirection();

        if (Mathf.Abs(rBody.velocity.x) < nangkuriBase.WalkSpeed)
            rBody.velocity += new Vector2(nangkuriBase.MoveAcc * nangkuriBase.headingTo.x, 0.0f);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nangkuriBase.SetRandomNumber(1);
    }
}

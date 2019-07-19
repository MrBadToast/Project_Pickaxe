using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nangkuri_Run : StateMachineBehaviour
{
    Rigidbody2D rBody;
    Base_Nangkuri nangkuriBase;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nangkuriBase = animator.GetComponent<Base_Nangkuri>();
        rBody = animator.GetComponent<Rigidbody2D>();
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(nangkuriBase.playerDetacted || nangkuriBase.PlayerLostTime < 0.2f)
        nangkuriBase.SwitchDirToPlayer();
        else
        {
            nangkuriBase.UpdateRays();
            if (nangkuriBase.detactChangeDir)
                nangkuriBase.SwitchDirection();
            else if (nangkuriBase.detactForward)
                nangkuriBase.SwitchDirection();
        }

        if(Mathf.Abs(rBody.velocity.x) < nangkuriBase.RunSpeed)
            rBody.velocity += new Vector2(nangkuriBase.MoveAcc * nangkuriBase.headingTo.x, 0.0f);

    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
//        nangkuriBase.SetRandomNumber(3);
    }
}

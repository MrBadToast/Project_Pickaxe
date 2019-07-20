using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nangkuri_Dead : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Instantiate(animator.GetComponent<Base_Nangkuri>().DeadParticle, animator.transform.position, Quaternion.identity);
        Destroy(animator.gameObject);
    }

}

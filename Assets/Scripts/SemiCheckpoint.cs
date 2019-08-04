using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SemiCheckpoint : MonoBehaviour
{
    public Transform restartPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("SCP Setted : " + transform.position);
            ChapterManager.Instance.SetSemiCheckpoint(this);
        }
    }
}

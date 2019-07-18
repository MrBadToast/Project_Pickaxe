using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(PlayerCore))]
public class PA_Dash : MonoBehaviour
{
    public float DashSpeed = 5.0f;
    public float DashTime = 0.3f;
    public GameObject AfterImage;
    public GameObject DashEffect;

    private bool Dashing = false;

    PlayerCore player;

    void Awake()
    {
        player = GetComponent<PlayerCore>();
    }

    void Update()
    {
        if(Input.GetKeyDown(player.dash)&&!Dashing)
        {
            player.CancelAllActions();
            Instantiate(DashEffect, transform.position, transform.rotation);
            StartCoroutine(Dash());
        }

        if (Dashing)
            Instantiate(AfterImage, transform.position, transform.rotation);
    }

    IEnumerator Dash()
    {
        player.RBody.velocity = Vector2.zero;
        player.RBody.velocity += new Vector2(-player.headingTo.x, 0f) * DashSpeed;
        Dashing = true;
        player.AllowControl = false;
        yield return new WaitForSeconds(DashTime);
        player.AllowControl = true;
        Dashing = false;
        player.RBody.velocity = Vector2.zero;

        yield return null;
    }
}

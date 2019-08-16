using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(PlayerCore))]
public class PA_Dash : MonoBehaviour
{
    public float DashSpeed = 5.0f;
    public float DashTime = 0.3f;
    public float DashCooldown = 0.5f;
    public GameObject AfterImage;
    public GameObject DashEffect;
    [Space]
    public string Anistate_Dash;


    private bool Dashing = false;
    private float CoolDownTimer = 0.0f;

    PlayerCore player;

    void Awake()
    {
        player = GetComponent<PlayerCore>();
    }

    void Update()
    {
        if (player.ControlAllowed)
        {
            if (Input.GetKeyDown(player.dash) && !Dashing && CoolDownTimer > DashCooldown)
            {
                player.CancelAllActions();
                Instantiate(DashEffect, transform.position, transform.rotation);
                StartCoroutine(Dash());
            }
        }
    }

    void FixedUpdate()
    {
        if (Dashing)
            Instantiate(AfterImage, transform.position, transform.rotation);
        else
            CoolDownTimer += Time.deltaTime;
    }

    IEnumerator Dash()
    {
        GetComponent<Animator>().Play(Anistate_Dash);

        if (Input.GetKey(player.rightmove))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            player.headingTo = Vector2.right;
        }
        if (Input.GetKey(player.leftmove))
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
            player.headingTo = Vector2.left;
        }

        player.RBody.velocity = Vector2.zero;
        player.RBody.velocity += new Vector2(player.headingTo.x, 0f) * DashSpeed;
        float PrevGravity = player.RBody.gravityScale;
        player.RBody.gravityScale = 0f;
        Dashing = true;
        player.ActionOccupied = true;
        player.Invincible = true;

        yield return new WaitForSeconds(DashTime);
        player.RBody.gravityScale = PrevGravity;
        player.ActionOccupied = false;
        Dashing = false;
        player.Invincible = false;
        CoolDownTimer = 0;
    }
}

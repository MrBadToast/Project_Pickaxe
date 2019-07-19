using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Base_Nangkuri : Enemy
{
    [Space]
    public float PlayerDetactionRange = 5.0f;
    public float WalkSpeed = 2.0f;
    public float RunSpeed = 3.0f;
    public float MoveAcc = 0.1f;
    [Space]
    public float RCDistance_Foot;
    public float RCDistance_Forward;
    public LayerMask TerrainLayer;
    public LayerMask HurtBy;
    [Space]
    public BoxCollider2D PlayerHurt;
    public Transform RCO_Foot;
    public Transform RCO_FrontFoot;
    public Transform RCO_Forward;
    [Space]
    public GameObject DeadParticle;

    [HideInInspector]
    public Vector2 headingTo = Vector2.right;
    [HideInInspector]
    public bool detactChangeDir;
    [HideInInspector]
    public bool detactForward;
    [HideInInspector]
    public bool playerDetacted;
    [HideInInspector]
    public float PlayerLostTime;

    private GameObject player;
    private Animator BSM;
    private Rigidbody2D rBody;
    private BoxCollider2D col;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        BSM = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody2D>();
        Health = MaxHealth;
    }

    void Update()
    {

        Debug.DrawLine(RCO_Foot.position, new Vector2(RCO_Foot.position.x, RCO_Foot.position.y - RCDistance_Foot));
        Debug.DrawLine(RCO_FrontFoot.position, new Vector2(RCO_FrontFoot.position.x, RCO_FrontFoot.position.y - RCDistance_Foot));
        Debug.DrawLine(RCO_Forward.position, new Vector2(RCO_Forward.position.x + headingTo.x * RCDistance_Forward, RCO_Forward.position.y));
        playerDetacted = CheckPlayerInSight();
        BSM.SetBool("PlayerDetact", playerDetacted); // 최적화 경고!

        if (playerDetacted)
            PlayerLostTime = 0;
        else
            PlayerLostTime += Time.deltaTime;
        BSM.SetFloat("PlayerLostTime", PlayerLostTime);
    }

    public override bool CheckPlayerInSight() // 최적화 경고! - 필요할때만 호출하도록 나중에 리펙토링 해보기!
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

#if UNITY_EDITOR
        if (
            !Physics2D.Raycast(transform.position, player.transform.position - transform.position, distance, TerrainLayer) &&
            distance < PlayerDetactionRange &&
            Vector2.Dot(headingTo, player.transform.position - transform.position) > 0f
            )
            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
        else
            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
#endif

        return !Physics2D.Raycast(transform.position, player.transform.position - transform.position, distance, TerrainLayer) &&
            distance < PlayerDetactionRange &&
            Vector2.Dot(headingTo, player.transform.position - transform.position) > 0f;
    }

    public override void Hurt(float damage, Vector2 knockBack)
    {
        base.Hurt(damage,knockBack);

        playerDetacted = true;
        rBody.velocity = knockBack;
        BSM.SetFloat("PlayerLostTime", 0.0f);
        BSM.SetTrigger("Hurt");
    }

    public override void Dead()
    {
        BSM.SetBool("Dead", true);
        PlayerHurt.enabled = false;
    }

    public void UpdateRays()
    {
        detactChangeDir = (Physics2D.Raycast(RCO_Foot.position, Vector2.down, RCDistance_Foot, TerrainLayer) && !Physics2D.Raycast(RCO_FrontFoot.position, Vector2.down, RCDistance_Foot, TerrainLayer));
        detactForward = Physics2D.Raycast(RCO_Forward.position, headingTo, RCDistance_Forward, TerrainLayer);
    }

    public void SetRandomNumber(int max = 1)
    {
        GetComponent<Animator>().SetInteger("RandomNumber", Random.Range(0, max + 1));
    }

    public void SwitchDirection()
    {
        if (headingTo == Vector2.right)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            headingTo = Vector2.left;
        }
        else if (headingTo == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            headingTo = Vector2.right;
        }
    }

    public void SwitchDirToPlayer()
    {
        StopCoroutine("Cor_SwitchDirToPlayer");
        StartCoroutine("Cor_SwitchDirToPlayer");
    }

    public bool DetactGround()
    {
        return Physics2D.Raycast(RCO_Foot.position, Vector2.down, RCDistance_Foot, TerrainLayer);
    }

    private IEnumerator Cor_SwitchDirToPlayer()
    {
        yield return new WaitForSeconds(0.1f);

        if (player.transform.position.x > transform.position.x && headingTo == Vector2.left)
        {
            SwitchDirection();
        }
        else if (player.transform.position.x < transform.position.x && headingTo == Vector2.right)
        {
            SwitchDirection();
        }
    }

}
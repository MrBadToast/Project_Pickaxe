using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(PlayerCore))]
public class PA_Attack : MonoBehaviour
{
    public Vector2 KnockBackForce = new Vector2(1.0f, 1.0f);
    public float AttackTime = 0.1f;
    public float AttackStandbyTime = 0.3f;
    public float MidairAttackVelocity = 10f;
    public float MidairAttackCooldown = 0.5f;


    [Title("프리팹")]
    public GameObject AttackEffect;
    public GameObject AttackEffect2;

    [Title("자식 오브젝트")]
    public Transform AttackEffectOrigin;
    public GameObject AttackHitbox;
    public GameObject AirAttackHitbox;
    public GameObject AirAttackObj;

    [FoldoutGroup("애니메이션 스테이트")]
    public string AniState_NormalAttack1;
    [FoldoutGroup("애니메이션 스테이트")]
    public string AniState_NormalAttack2;
    [FoldoutGroup("애니메이션 스테이트")]
    public string AniState_NormalAttack3;
    [FoldoutGroup("애니메이션 스테이트")]
    public string AniState_WallHang;

    [FoldoutGroup("애니메이션 파라미터")]
    public string AniPar_AttackTimer;
    [FoldoutGroup("애니메이션 파라미터")]
    public string AniPar_WallHangExit;

    private PlayerCore player;

    private int AttackedNumber = 0;
    private float AttackTimer = 0;                  // 공격키를 누르지 않았을 때 증가하는 값입니다.
    private bool AirAttacked = false;
    private float AirAttackTimer = 0;

    void Awake()
    {
        player = GetComponent<PlayerCore>();
    }

    void Start()
    {
        AirAttackObj.SetActive(false);
        AttackHitbox.SetActive(false);
    }

    void Update()
    {
        if (player.WallHanged)
        {
            if (Input.GetKeyDown(player.down))
            {
                StopWallHang();
            }

            if (Input.GetKeyDown(player.jump))
            {
                StopWallHang();
                player.transform.rotation *= Quaternion.Euler(0, 180, 0);
                player.headingTo = -player.headingTo;
                player.RBody.velocity += player.headingTo * 5f;
            }

            if (Input.GetKeyDown(player.dash))
            {
                StopWallHang();
                player.transform.rotation *= Quaternion.Euler(0, 180, 0);
                player.headingTo = -player.headingTo;
            }
        }

        if (!player.ActionOccupied)
        {
            if (AttackTimer > AttackStandbyTime)
            {
                AttackedNumber = 0;
            }
            else
                AttackTimer += Time.deltaTime;

            if (Input.GetKeyDown(player.attack) && !player.WallHanged)
            {
                StopAllCoroutines();
                if (player.GroundDetact)
                    StartCoroutine("Normal_Attack");
                else if (!player.GroundDetact && player.ForwardDetact)
                    WallHang();
                else if (!player.GroundDetact && AirAttackTimer > MidairAttackCooldown)
                {
                    StartCoroutine("MidairAttack");
                    AirAttackTimer = 0f;
                }

                AttackTimer = 0;
            }


            AirAttackTimer += Time.deltaTime;
            player.anim.SetFloat(AniPar_AttackTimer, AttackTimer);
        }

        if(player.GroundDetact == true)
        {
            AirAttacked = false;
        }
    }

    IEnumerator Normal_Attack()
    {
        player.ActionOccupied = true;
        Vector3 Target = player.GetMouseWorldpos();

        if (Target.x > transform.position.x)
        { transform.rotation = Quaternion.Euler(0, 0, 0); player.headingTo = Vector2.right; }
        else
        { transform.rotation = Quaternion.Euler(0, 180, 0); player.headingTo = Vector2.left; }

        player.RBody.velocity = player.headingTo * 5;

        switch (AttackedNumber)
        {
            case 0:
                AttackedNumber = 1;
                player.anim.Play(AniState_NormalAttack1,-1,0f);
                AttackHitbox.SetActive(true);
                Instantiate(AttackEffect, AttackEffectOrigin.position, transform.rotation);
                break;

            case 1:
                AttackedNumber = 2;
                player.anim.Play(AniState_NormalAttack2,-1,0f);
                AttackHitbox.SetActive(true);
                Instantiate(AttackEffect2, AttackEffectOrigin.position, transform.rotation);
                break;

            case 2:

                AttackedNumber = 0;
                player.anim.Play(AniState_NormalAttack3,-1,0f);
                AttackHitbox.SetActive(true);
                Instantiate(AttackEffect, AttackEffectOrigin.position, transform.rotation);
                break;

        }

        yield return new WaitForSeconds(AttackTime);
        AttackHitbox.SetActive(false);
        player.ActionOccupied = false;
        yield return null;

    }

    private IEnumerator MidairAttack()
    {
        if (!AirAttacked)
            player.RBody.velocity = new Vector2(player.RBody.velocity.x, MidairAttackVelocity);
        else
            player.RBody.velocity = new Vector2(player.RBody.velocity.x, 0f);

        AirAttackObj.SetActive(true);
        AirAttackObj.GetComponent<Animator>().Play("AirAttack",-1,0f);
        AirAttackHitbox.SetActive(true);
        AirAttacked = true;

        yield return new WaitForSeconds(0.5f);
        AirAttackObj.SetActive(false);
        AirAttackHitbox.SetActive(false);
    }

    public void CancelAttack()
    {
        StopCoroutine("Normal_Attack");
        StopCoroutine("MidairAttack");
        AttackHitbox.SetActive(false);
        player.ActionOccupied = false;
    }

    private void WallHang()
    {
        player.anim.Play(AniState_WallHang);
        player.WallHanged = true;
        player.AirJumped = false;
        player.RBody.constraints = RigidbodyConstraints2D.FreezePosition;

    }

    private void StopWallHang()
    {
        player.anim.SetTrigger(AniPar_WallHangExit);
        player.RBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.WallHanged = false;
    }
}

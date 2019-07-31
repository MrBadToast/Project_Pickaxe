using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(PlayerCore))]
public class PA_PickaxeThrow : MonoBehaviour
{
    public float PickaxeThrowReach = 5.0f;

    [Title("자식 오브젝트")]
    public GameObject PickAxe;
    public GameObject PickaxeChain;

    [FoldoutGroup("애니메이션 파라미터")]
    public string AniPar_PickaxeReturned;

    [FoldoutGroup("애니메이션 스테이트")]
    public string AniState_ThrowPickaxe;
    [FoldoutGroup("애니메이션 스테이트")]
    public string AniState_ThrowReady;

    private PlayerCore player;

    void Start()
    {
        player = GetComponent<PlayerCore>();
        PickAxe.SetActive(false);
        PickaxeChain.SetActive(false);
    }


    void Update()
    {
        if (!player.ActionOccupied && !player.WallHanged)
        {
            if (Input.GetKeyDown(player.throwP))
            {
                StartCoroutine(ThrowPickaxe());
            }
        }
    }

    IEnumerator ThrowPickaxe()
    {
        player.anim.Play(AniState_ThrowReady);

        player.ActionOccupied = true;
        TimeManager.Instance.SetTimeScale(0.2f);

        Vector3 Target = Vector3.zero;

        while (Input.GetKey(player.throwP))
        {
            Target = player.GetMouseWorldpos();
            if ((Target - transform.position).magnitude > PickaxeThrowReach)
                Target = (Target - transform.position).normalized * PickaxeThrowReach + transform.position;

            if (Target.x > transform.position.x)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);

            Debug.DrawLine(transform.position, Target);

            yield return new WaitForSeconds(0.02f);
        }
        player.anim.Play(AniState_ThrowPickaxe);

        TimeManager.Instance.SetTimeScale(1.0f);

        float Speed = 0.15f;
        float MaxReaching = 0.9f;
        float Reaching = 0.0f;
        bool Reached = false;
        bool Returned = false;

        PickAxe.SetActive(true);
        PickaxeChain.SetActive(true);

        while (!Returned)
        {
            if (!Reached)
            {
                Speed -= Speed / 13f;
                Reaching += Speed;
                if (Reaching >= MaxReaching)
                    Reached = true;

                if (PickAxe.GetComponent<BoxCollider2D>().IsTouchingLayers(player.TerrainLayer))
                    Reached = true;
            }
            else
            {
                Speed += Speed / 13f;
                Reaching -= Speed;
                if (Reaching < 0.1f)
                    Returned = true;
            }

            PickAxe.transform.position = Vector3.Lerp(transform.position, Target, Reaching);

            PickAxe.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(Target.y - transform.position.y, Target.x - transform.position.x) * Mathf.Rad2Deg);
            PickAxe.transform.rotation *= Quaternion.Euler(0, 0, -90);

            Debug.DrawLine(transform.position, Target, Color.red, 1.0f);

            yield return new WaitForSeconds(0.02f);
        }

        player.anim.SetTrigger(AniPar_PickaxeReturned);
        PickAxe.SetActive(false);
        PickaxeChain.SetActive(false);
        player.ActionOccupied = false;
    }

    public void CancelThrow()
    {
        StopAllCoroutines();
        player.anim.SetTrigger(AniPar_PickaxeReturned);
        PickAxe.transform.position = Vector2.zero;
        PickAxe.SetActive(false);
        PickaxeChain.SetActive(false);
        player.ActionOccupied = false;
        TimeManager.Instance.SetTimeScale(1.0f);        
    }
}

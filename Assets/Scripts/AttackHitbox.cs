using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public float NormalDamage = 1.0f;
    public Vector2 KnockBackPower;
    public LayerMask Target;
    public GameObject SlashEffect;

    Vector2 headingTo;
    void Awake()
    {
    }

    void OnEnable()
    {
        if (transform.rotation == Quaternion.Euler(0f, 0f, 0f))
            headingTo = Vector2.right;
        else if (transform.rotation == Quaternion.Euler(0f, 180f, 0f))
            headingTo = Vector2.left;

        Collider2D[] cols = null;

        if (TryGetComponent(out BoxCollider2D boxcol))
            cols = Physics2D.OverlapBoxAll(transform.position + new Vector3(boxcol.offset.x * headingTo.x, boxcol.offset.y), boxcol.size, 0f, Target);

        if (TryGetComponent(out CircleCollider2D cirlecol))
            cols = Physics2D.OverlapCircleAll(transform.position + new Vector3(cirlecol.offset.x * headingTo.x, cirlecol.offset.y), cirlecol.radius, Target);



        foreach (Collider2D col in cols)
        {
            Vector2 force = new Vector2(headingTo.x * KnockBackPower.x, KnockBackPower.y);

            col.GetComponent<Enemy>().Hurt(NormalDamage, force);
            TimeManager.Instance.SetTimeScaleTimered(0.2f, 0.1f);
            if (SlashEffect)
                Instantiate(SlashEffect, col.transform.position,Quaternion.Euler(0,0,Random.Range(0f,359f)));
        }
    }
}

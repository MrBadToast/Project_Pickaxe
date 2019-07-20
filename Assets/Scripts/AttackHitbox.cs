using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    public float NormalDamage = 1.0f;
    public Vector2 KnockBackPower;
    public LayerMask Target;

    private BoxCollider2D boxcol;

    Vector2 headingTo;
    void Awake()
    {
        boxcol = GetComponent<BoxCollider2D>();
    }

    void OnEnable()
    {
        if (transform.rotation == Quaternion.Euler(0f, 0f, 0f))
            headingTo = Vector2.right;
        else if (transform.rotation == Quaternion.Euler(0f, 180f, 0f))
            headingTo = Vector2.left;

        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position + new Vector3(boxcol.offset.x * headingTo.x, boxcol.offset.y), boxcol.size, 0f, Target);

        foreach (Collider2D col in cols)
        {
            col.GetComponent<Enemy>().Hurt(NormalDamage, new Vector2(headingTo.x * KnockBackPower.x, KnockBackPower.y));
        }
    }
}

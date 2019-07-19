using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    public float NormalDamage = 1.0f;
    public Vector2 KnockBackPower;
    public LayerMask Target;

    public PlayerCore player;

    BoxCollider2D boxcol;
    void Awake()
    {
        boxcol = GetComponent<BoxCollider2D>();
    }

    void OnEnable()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, boxcol.size, 0f,Target);

        foreach(Collider2D col in cols)
        {
            col.GetComponent<Enemy>().Hurt(NormalDamage, new Vector2(player.headingTo.x * KnockBackPower.x, KnockBackPower.y));
        }
    }
}

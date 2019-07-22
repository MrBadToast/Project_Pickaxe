using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : MonoBehaviour
{
    public int Damage = 1;
    public LayerMask PlayerLayermask;

    private BoxCollider2D boxcol;

    Vector2 headingTo;
    void Awake()
    {
        boxcol = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Collider2D col = Physics2D.OverlapBox(transform.position + new Vector3(boxcol.offset.x, boxcol.offset.y), boxcol.size, 0f, PlayerLayermask);
        if (col != null)
        {
            col.GetComponent<PlayerCore>().Hurt(Damage, transform.position);
        }
    }
}

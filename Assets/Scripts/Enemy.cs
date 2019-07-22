using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Enemy : MonoBehaviour
{
    protected float Health;
    public float MaxHealth = 3.0f;

    public virtual void Hurt(float damage, Vector2 knockBack)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Health = 0;
            Dead();
        }
    }

    public virtual void Dead() { }

    public virtual bool CheckPlayerInSight() { return false; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Effect_AfterImage : MonoBehaviour
{
    [Range(0,1)]
    public float FadeSpeed = 0.1f;

    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - FadeSpeed);

        if(sprite.color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }
}

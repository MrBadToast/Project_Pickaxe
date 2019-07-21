using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_HpBlockSingle : MonoBehaviour
{
    public float WiggleSpeed = 10f;
    public float WiggleIntensity = 5f;

    private bool Wiggle = false;

    private Animator animator;

    Vector3 Origin;
    float rand;
    bool Filled;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void Start()
    {
        rand = Random.Range(0f,10f);
        Origin = transform.localPosition;
    }

    void OnEnabled()
    {
    }

    void Update()
    {
        if (Wiggle)
        {
            transform.localPosition = Origin;
            transform.localPosition = Origin + new Vector3(Mathf.PerlinNoise(Time.time*WiggleSpeed + rand, 0f)*WiggleIntensity, Mathf.PerlinNoise(0f, Time.time * WiggleSpeed + rand) *WiggleIntensity);
        }
    }

    public void OnHealthChanged(int health,bool _fill)
    {
        Filled = _fill;

        animator.SetInteger("Health", health);

        if (_fill == true)
            animator.Play("Filled_Expanded");

        else
            animator.Play("Empty_Expanded");

        StopAllCoroutines();
        StartCoroutine(Cor_FlatTimer());
    }

    public void SetWiggle(bool val)
    {
        if (val)
            Wiggle = true;
        else
            Wiggle = false;
    }

    IEnumerator Cor_FlatTimer(float time = 2.0f)
    {
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Pass");
    }
}

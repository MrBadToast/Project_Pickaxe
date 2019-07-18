using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public bool TimerOnStart = true;
    public float Duration = 1.0f;

    private void Start()
    {
        if (TimerOnStart)
            StartCoroutine(Cor_StartTimer(Duration));
    }

    public void StartTimer()
    {
        StartCoroutine(Cor_StartTimer(Duration));
    }

    private IEnumerator Cor_StartTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Destroy(gameObject);
    }
}

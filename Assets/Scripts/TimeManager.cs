using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance = null;

    private void Start()
    {
        #region singleton
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        #endregion
    }

    public void SetTimeScaleTimered(float slowFactor = 1.0f, float duration = 1.0f)
    {
        StopAllCoroutines();
        StartCoroutine(Cor_SetTimeScaleTimered(slowFactor, duration));
    }

    public void SetTimeScale(float slowFactor = 1.0f)
    {
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void SetTimeScaleSmooth(float slowFactor=1.0f)
    {

    }

    private IEnumerator Cor_SetTimeScaleTimered(float slowFactor, float duration)
    {
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 1f * 0.02f;
    }

    private IEnumerator Cor_SetTimeScaleSmooth(float From,float To)
    {
        while(Mathf.Abs(From-To) > 0.1f )
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, To, 0.5f);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            yield return new WaitForSecondsRealtime(0.02f);
        }
    }
}

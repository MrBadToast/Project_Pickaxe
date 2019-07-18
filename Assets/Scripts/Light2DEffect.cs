using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
[RequireComponent(typeof(Light2D))]
public class Light2DEffect : MonoBehaviour
{
    public float flikerIntensity = 1.0f;
    public float flikerRange = 1.0f;
    public float flikerSpeed = 1.0f;
    private Light2D _light;


    private void Awake()
    {
        _light = GetComponent<Light2D>();
    }
    void Update()
    {
        _light.intensity = flikerIntensity + Mathf.PerlinNoise(Time.time * flikerSpeed, 0) * flikerRange;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainVirtualCamera : MonoBehaviour
{
    static public MainVirtualCamera Instance;
    private CinemachineVirtualCamera vCam;
    private CinemachineConfiner confiner;

    private void Awake()
    {
        Instance = this;
        vCam = GetComponent<CinemachineVirtualCamera>();
        confiner = GetComponent<CinemachineConfiner>();
    }

    public void SetConfinerColider(Collider2D colider)
    {
        confiner.m_BoundingShape2D = colider;
        confiner.InvalidatePathCache();
    }
}

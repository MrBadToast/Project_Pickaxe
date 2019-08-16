using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{

    public bool StartEnabled = false;
    [HideInInspector]
    public GameObject MapObject;
    public Collider2D camColider;

    private void Awake()
    {
        MapObject = gameObject;
    }

    private void Start()
    {
        if (!StartEnabled)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}

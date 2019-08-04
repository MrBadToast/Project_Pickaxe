using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    [HideInInspector]
    public GameObject MapObject;
    public Collider2D camColider;

    private void Awake()
    {
        MapObject = gameObject;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PickaxeChain : MonoBehaviour
{
    [Required]
    public GameObject Player;
    [Required]
    public GameObject Pickaxe;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, Player.transform.position);
        lineRenderer.SetPosition(1, Player.transform.position);
    }

    void Update()
    {
        lineRenderer.SetPosition(0, Player.transform.position);
        lineRenderer.SetPosition(1, Pickaxe.transform.position);
        float distance = Vector3.Distance(Player.transform.position, Pickaxe.transform.position);

        lineRenderer.material.mainTextureScale = new Vector2(distance*4,1);
    }
}

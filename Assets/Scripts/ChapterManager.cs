using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    static public ChapterManager Instance = null;
    [HideInInspector]
    public PlayerCore player;
    [HideInInspector]
    public MapInfo CurrentMap = null;
    [HideInInspector]
    public SemiCheckpoint LastPoint = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCore>();
    }

    public void SetSemiCheckpoint(SemiCheckpoint scp)
    {
        LastPoint = scp;
    }

    public void PlayerToLastPoint()
    {
        player.transform.position = LastPoint.restartPoint.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    static public HUD Instance = null;

    public HUD_HpBlockManager hpManager;

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

    public void OnHurt(int Health)
    {
        hpManager.SetHealth(Health);
    }
}

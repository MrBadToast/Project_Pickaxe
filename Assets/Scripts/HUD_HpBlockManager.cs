using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_HpBlockManager : MonoBehaviour
{
    public float BlockSpace = 150f;
    [Space]
    public GameObject HealthBlockPrefab;

    [SerializeField]
    private List<HUD_HpBlockSingle> Blocks;

    int MaxHealth = 0;
    public int CurrentHealth = 0;

    private PlayerCore player;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        MaxHealth = Blocks.Count;
        CurrentHealth = MaxHealth;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCore>();
            MaxHealth = player.MaxHealth;
            CurrentHealth = player.CurrentHealth;
        }

        SetHealthBlockNumber(MaxHealth);
        SetHealth(CurrentHealth);
    }

    public void SetHealth(int amount)
    {
        CurrentHealth = amount;

        animator.Play("Bounce");

        for (int i = 0; i < amount; i++)
        {
            if (i < Blocks.Count && i >= 0)
                Blocks[i].OnHealthChanged(CurrentHealth, true);
        }

        for (int i = amount; i < Blocks.Count; i++)
        {
            if (i < Blocks.Count && i >= 0)
                Blocks[i].OnHealthChanged(CurrentHealth, false);
        }

        if (amount <= 1)
        {
            foreach (HUD_HpBlockSingle block in Blocks)
                block.SetWiggle(true);
        }
        else
        {
            foreach (HUD_HpBlockSingle block in Blocks)
                block.SetWiggle(false);
        }

    }

    private void SetHealthBlockNumber(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (i >= Blocks.Count)
            {
                GameObject temp = Instantiate(HealthBlockPrefab, transform);
                temp.transform.position += new Vector3(Blocks.Count * BlockSpace, 0.0f);
                Blocks.Add(temp.GetComponent<HUD_HpBlockSingle>());
            }
        }
    }
}

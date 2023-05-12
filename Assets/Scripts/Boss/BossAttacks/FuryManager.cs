using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuryManager : MonoBehaviour
{
    public Fury furyAttack;
    public BasicAttack basicAttack;

    private void Start()
    {
        basicAttack.damageMultiplier = 1;
        basicAttack.attackDuration = 4;
    }

    void Update()
    {
        furyAttack.UpdateFury();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExplosiveBarrels : IExplode
{
    public override void Explode()
    {
        base.Explode();
        OnExloded?.Invoke();
        if (!GameController.GetGameController().GetSpawner().WaveStarted()) {
            FindObjectOfType<DontExplodeBarrelsUICanvas>().ShowUp();
        }
    }

}

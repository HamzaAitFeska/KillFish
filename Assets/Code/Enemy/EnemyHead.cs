using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    [SerializeField] IEnemyHealth health;
     [SerializeField] float HeadDamageMultiplier = 2.0f;
    public IExplode explosiveComponent;
    public void TakeDamage(float amount, bool exploded, bool collateral)
    {
        health.TakeDamage(amount * HeadDamageMultiplier, exploded, collateral);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedDistanceEnemyAnimatorEventHandler : MonoBehaviour
{
    [SerializeField] GroundedDistanceEnemy parent;

    void StartLeftChargeParticles()
    {
        parent.StartLeftChargeParticles();
    }
    void StartRightChargeParticles()
    {
        parent.StartRightChargeParticles();
    }
    void SetIsAttacking()
    {
        parent.SetIsAttacking();
    }
    void SetIsNOTAttacking()
    {
        parent.SetIsNOTAttacking();
    }

    void InstantiateLeftProjectile()
    {
        parent.InstantiateLeftProjectile();

        parent.StopLeftChargeParticles();
    }
    void InstantiateRightProjectile()
    {
        parent.InstantiateRightProjectile();

        parent.StopRightChargeParticles();
    }
}

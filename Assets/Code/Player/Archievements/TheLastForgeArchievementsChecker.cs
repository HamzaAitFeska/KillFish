using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheLastForgeArchievementsChecker : MonoBehaviour
{
    bool touchedLava;
    bool usedDash;
    void Start()
    {
        touchedLava = false;
        usedDash = false;
    }

    private void OnEnable()
    {
        ILava.OnDamage += LavaDamaged;
        EnemySpawner.OnWaveFinished += ArchievementsWaveFinished;
        EnemySpawner.OnWaveStarted += ArchievementsWaveStarted;
        FPSMoveController.OnDashed += Dashed;
    }
    private void OnDisable()
    {
        ILava.OnDamage -= LavaDamaged;
        EnemySpawner.OnWaveFinished -= ArchievementsWaveFinished;
        EnemySpawner.OnWaveStarted -= ArchievementsWaveStarted;
        FPSMoveController.OnDashed -= Dashed;        
    }
    void Dashed()
    {
        usedDash = true;
    }
    void LavaDamaged()
    {
        touchedLava = true;
    }
    void ArchievementsWaveStarted(int wave)
    {
        if (!usedDash) ArchievementsNameConstantsData.DontNeedToMoveWorldMovesForMeAccomplisher();
    }
    void ArchievementsWaveFinished(int wave)
    {
        if (!touchedLava) ArchievementsNameConstantsData.TheFloorIsLavaAccomplisher();
    }
}

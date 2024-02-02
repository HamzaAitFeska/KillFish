using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilledAllEnemiesUI : MonoBehaviour
{
    private void OnEnable()
    {
        EnemySpawner.OnWaveFinished += KilledAllEnemies;
    }
    private void OnDisable()
    {
        EnemySpawner.OnWaveFinished -= KilledAllEnemies;
    }

    void KilledAllEnemies(int wave)
    {
        GetComponent<Animation>().Play();
    }
}

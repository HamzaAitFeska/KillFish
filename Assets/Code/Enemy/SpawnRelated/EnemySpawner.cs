using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(SlowMoTimer))]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] SlowMoTimer slowMo;
    // List of waves of enemies
    public List<Wave> waves;
    bool gameStarted = false;
    public bool WaveStarted() { return gameStarted; }
    int enemiesAlive;


    // Current wave and horde index
    private int currentWave = 0;
    private int currentHorde = 0;

    public static event Action<int> OnWaveFinished;
    public static event Action<int> OnWaveStarted;

    // Class for a wave of enemies
    [System.Serializable]
    public class Wave
    {
        // List of hordes of enemies
        public List<Horde> hordes;
    }

    // Class for a horde of enemies
    [System.Serializable]
    public class Horde
    {
        // List of enemy game objects
        public List<GameObject> enemies;
        public int EnemiesAliveToSpawnHorde;

        // Enable all the enemies in the horde
        public int EnableEnemies()
        {
            int alive = 0;
            foreach (GameObject enemy in enemies)
            {
                enemy.SetActive(true);
                alive++;
            }
            return alive;
        }
    }


    public void RemoveEnemy(IEnemyHealth enemy)
    {

        Wave currentWaveObj = waves[currentWave];

        for (int i = 0; i <= currentHorde; i++)
        {
            for (int j = 0; j < waves[currentWave].hordes[i].enemies.Count; j++)
            {
                if (currentWaveObj.hordes[i].enemies[j] == enemy.gameObject) currentWaveObj.hordes[i].enemies.RemoveAt(j);
            }
        }
        enemiesAlive--;
    }

    private void Start()
    {
        GameController.GetGameController().SetEnemySpawner(this);
    }

    private void Update()
    {

        // Check if all waves and hordes have been completed
        if (currentWave >= waves.Count || !gameStarted)
        {
            // All waves and hordes have been completed
            return;
        }

        // Get current wave and horde
        Wave currentWaveObj = waves[currentWave];
        Horde currentHordeObj = currentWaveObj.hordes[currentHorde];

        // Check if all enemies in the current horde have been killed
        if (currentHorde + 1 < currentWaveObj.hordes.Count && currentWaveObj.hordes[currentHorde + 1] != null)
        {
            if (enemiesAlive <= currentWaveObj.hordes[currentHorde+1].EnemiesAliveToSpawnHorde)
            {
                // All enemies in the current horde have been killed
                // Move on to the next horde or wave
                if (currentHorde + 1 < currentWaveObj.hordes.Count)
                {
                    // Move on to the next horde
                    currentHorde++;
                    currentHordeObj = currentWaveObj.hordes[currentHorde];
                    enemiesAlive += currentHordeObj.EnableEnemies(); // Enable the enemies in the new horde
                    return; //For now it works as expected, if need to take it off, put in the if(enemiesAlive== 0) to recalculate enemiesAlive and if there is not 0, break the if
                }
            }
        }
        if (enemiesAlive == 0)
        {
            // Move on to the next wave
            OnWaveFinished?.Invoke(currentWave);

            //Starts slow mo
            slowMo.StartSlowMo();

            currentWave++;
            currentHorde = 0;
        }
    }

    public void StartNewWave()
    {
        // Enable the enemies in the first horde of the new wave
        if (currentWave < waves.Count)
        {
            enemiesAlive += waves[currentWave].hordes[0].EnableEnemies();
            OnWaveStarted?.Invoke(currentWave);
            gameStarted = true;
        }
    }

    public void StartWaveOrb()
    {
        // Enable the enemies in the first horde
        StartNewWave();
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.StartWave, transform.position);
    }
}

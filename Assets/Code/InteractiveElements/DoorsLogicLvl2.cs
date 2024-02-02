using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsLogicLvl2 : MonoBehaviour
{
    [SerializeField] AnimDoor doorWaveFinish;
    private void OnEnable()
    {
        EnemySpawner.OnWaveFinished += UnlockDoor;
    }
    private void OnDisable()
    {
        EnemySpawner.OnWaveFinished -= UnlockDoor;
    }

    void UnlockDoor(int wave)
    {
        Debug.Log("Opening door on " + name);
        doorWaveFinish.OpenDoor();
    }
}

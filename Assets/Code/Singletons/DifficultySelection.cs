using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty Selection", menuName = "Data/Difficulty"), System.Serializable]
public class DifficultySelection : ScriptableObject
{
    public DifficultyType currentDifficulty;
    [Header("Damages")]
    //DOG
    public float DogDamageEasyMode;
    public float DogDamageMediumMode;
    public float DogDamageHardMode;
    //TURRET
    public float TurretDamageEasyMode;
    public float TurretDamageMediumMode;
    public float TurretDamageHardMode;


}
public enum DifficultyType { EASY, MEDIUM, HARD }

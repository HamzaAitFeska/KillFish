using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "FMOD Events", menuName = "Data/FMOD"), System.Serializable]
public class FMODEvents : ScriptableObject
{
    [field: Header("Player")]
    [field:SerializeField] public EventReference styleMeterChange { get; private set; }  
    [field: Header("Player Hook")]
    [field:SerializeField] public EventReference hookShot { get; private set; }  
    [field:SerializeField] public EventReference hookReturned { get; private set; }  
    [field:SerializeField] public EventReference hookReleased { get; private set; }  
    [field: Header("Player Weapon")]
    [field:SerializeField] public EventReference shotgunShot { get; private set; }  
    [field:SerializeField] public EventReference shotgunShotRecharge { get; private set; }  
    [field:SerializeField] public EventReference rifleShot { get; private set; }
    [field:SerializeField] public EventReference chargeSniper { get; private set; }
    [field:SerializeField] public EventReference sniperRecharge { get; private set; }
    [field: Header("Player Movement")]
    [field:SerializeField] public EventReference run { get; private set; }
    [field:SerializeField] public EventReference jump { get; private set; }
    [field:SerializeField] public EventReference doubleJump { get; private set; }
    [field:SerializeField] public EventReference land { get; private set; }
    [field:SerializeField] public EventReference stomp { get; private set; }
    [field:SerializeField] public EventReference dash { get; private set; }
    [field: Header("Player Health")]
    [field:SerializeField] public EventReference playerHit { get; private set; }
    [field:SerializeField] public EventReference playerDie { get; private set; }

    [field: Header("Enemies")]
    [field:SerializeField] public EventReference ShotgunHit { get; private set; }
    
    [field: Header("DOG Enemy")]
    [field:SerializeField] public EventReference dogAttack { get; private set; }
    [field:SerializeField] public EventReference dogDeath { get; private set; }
    [field:SerializeField] public EventReference dogRun { get; private set; }
    [field:SerializeField] public EventReference dogSpawn { get; private set; }
    [field: Header("TURRET Enemy")]

    [field: SerializeField] public EventReference turretAttack { get; private set; }
    [field: SerializeField] public EventReference turretDeath { get; private set; }
    [field:SerializeField] public EventReference turretSpawn { get; private set; }

    [field:Header("UI")]
    [field: SerializeField] public EventReference UIClick { get; private set; }
    [field: SerializeField] public EventReference UIHover { get; private set; }
    [field: SerializeField] public EventReference UIStart { get; private set; }
    [field: SerializeField] public EventReference TutorialAppear { get; private set; }
    [field: SerializeField] public EventReference VideoIntroAudio { get; private set; }
    [field: SerializeField] public EventReference VideoCreditsAudio { get; private set; }
    [field: SerializeField] public EventReference YouAreTheFishAudio { get; private set; }
    [field: SerializeField] public EventReference StyleFinalAppear { get; private set; }
    [field: SerializeField] public EventReference StyleFinalFramesLetters { get; private set; }
    [field: SerializeField] public EventReference StyleFinalTimeAndPoints { get; private set; }
    [field: SerializeField] public EventReference StyleFinalStyleChange { get; private set; }
    [field: SerializeField] public EventReference HitmarkerAudio { get; private set; }

    [field:Header("Ambience")]
    [field: SerializeField] public EventReference DoorOpen { get; private set; }
    [field: SerializeField] public EventReference DoorClose { get; private set; }
    [field: SerializeField] public EventReference Machinery { get; private set; }
    [field: SerializeField] public EventReference MachineryBroken { get; private set; }
    [field: SerializeField] public EventReference IndustrialFan { get; private set; }
    [field: SerializeField] public EventReference AirConditioner { get; private set; }
    [field: SerializeField] public EventReference WhiteNoise { get; private set; }
    [field: SerializeField] public EventReference Lava { get; private set; }
    [field: SerializeField] public EventReference Explosion { get; private set; }
    [field: SerializeField] public EventReference Lever { get; private set; }
    [field: SerializeField] public EventReference OrbLoop { get; private set; }
    [field: SerializeField] public EventReference Water { get; private set; }
    [field: SerializeField] public EventReference StartWave { get; private set; }

    [field:Header("Music")]
    [field: SerializeField] public EventReference LevelOneMusic { get; private set; }
    [field: SerializeField] public EventReference LevelTwoMusic { get; private set; }
    [field: SerializeField] public EventReference MenuMusic { get; private set; }




}

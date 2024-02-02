using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
public class MusicLevel : MonoBehaviour
{
    [SerializeField] EventReference levelMusic;
    public EventInstance musicReference;
    private void OnEnable()
    {
        EnemySpawner.OnWaveStarted += SetCombatPhase;
        EnemySpawner.OnWaveFinished += SetAmbiencePhase;
    }
    private void OnDisable()
    {
        EnemySpawner.OnWaveStarted -= SetCombatPhase;
        EnemySpawner.OnWaveFinished -= SetAmbiencePhase;
    }
    private void OnDestroy()
    {
        musicReference.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void Start()
    {
        musicReference = AudioManager.GetAudioManager().CreateEventInstance(levelMusic);
        musicReference.start();
        AudioManager.GetAudioManager().SetMusicLevelEvent(musicReference);
    }
    public void SetAlertPhase()
    {
        AudioManager.GetAudioManager().SetParameter(musicReference, "Phase", 1);
    }

    public void SetCombatPhase(int wave)
    {
        AudioManager.GetAudioManager().SetParameter(musicReference, "Phase", 2);
    }

    public void SetAmbiencePhase(int wave)
    {
        AudioManager.GetAudioManager().SetParameter(musicReference, "Phase", 0);
    }

}

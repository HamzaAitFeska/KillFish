using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;
public class AudioManager : MonoBehaviour
{
    static AudioManager audioManager = null;
    public FMODEvents Events;

    private List<EventInstance> eventsList;
    private List<StudioEventEmitter> eventsEmitterList;

    bool audioPaused = false;
    EventInstance musicLevel;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += CleanEventsUp;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= CleanEventsUp;
    }


    public static AudioManager GetAudioManager()
    {
        if (audioManager == null)
        {
            audioManager = new GameObject("AudioManager").AddComponent<AudioManager>();

            audioManager.Events = Resources.Load<FMODEvents>("Sound/FMOD Events");
            audioManager.eventsList = new List<EventInstance>();
            audioManager.eventsEmitterList = new List<StudioEventEmitter>();
        }

        return audioManager;
    }
    public void SetAudioManager()
    {
        audioManager = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void SetMusicLevelEvent(EventInstance refe)
    {
        musicLevel = refe;
    }
    private void Update()
    {
        if(audioPaused && Time.timeScale == 1)
        {

            RuntimeManager.GetBus("bus:/SFX").setPaused(false);
            SetParameter(musicLevel, "MusicFilter", 0f);
            audioPaused = false;
        }
        if (!audioPaused && Time.timeScale == 0)
        {
            RuntimeManager.GetBus("bus:/SFX").setPaused(true);
            SetParameter(musicLevel, "MusicFilter", 10f);
            //SFXNoPause remains Unpaused //This is for UI Elements like buttons
            audioPaused =true;
        }
    }
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
    public EventInstance CreateEventInstance(EventReference sound)
    {
        EventInstance EventInstance = RuntimeManager.CreateInstance(sound);
        eventsList.Add(EventInstance);
        return EventInstance;
    }
    public StudioEventEmitter InitializeEventEmitter(EventReference sound, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = sound;
        eventsEmitterList.Add(emitter);
        return emitter;
    }

    public void SetParameter(EventInstance sound, string parameterName, float parameterValue)
    {
        sound.setParameterByName(parameterName, parameterValue);
    }

     void CleanEventsUp(Scene sc, LoadSceneMode mode)
    {
        foreach (EventInstance eventInst in eventsList)
        {
            eventInst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInst.release();
        }
        foreach (StudioEventEmitter emitter in eventsEmitterList)
        {
            emitter.Stop();
        }
    }
}

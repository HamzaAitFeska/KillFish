using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CreditScene : MonoBehaviour
{
    EventInstance audioVideo;
    [SerializeField] Image blackScreenOnSkipAnimation;
    [SerializeField] VideoPlayer creditVideo;
    // Start is called before the first frame update
    void Start()
    {
        audioVideo = AudioManager.GetAudioManager().CreateEventInstance(AudioManager.GetAudioManager().Events.VideoCreditsAudio);
        audioVideo.start();
        creditVideo.loopPointReached += OnVideoEnd;
    }

    public void SkipVideoPress()
    {
       SkipAnimation();
    }

    public void SkipAnimation()
    {
        //blackScreenOnSkipAnimation.color = new Color(0, 0, 0, 1);
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.UIHover, transform.position);
        audioVideo.stop(STOP_MODE.ALLOWFADEOUT);
        LoadMainScene();
    }

    public void LoadMainScene()
    {
        GameController.GetGameController().ChangeScene(0);
    }

    private void OnVideoEnd(VideoPlayer source)
    {
        LoadMainScene();
    }
}

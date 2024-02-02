using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class FishFinal : MonoBehaviour
{
    public static Action OnGameFinished;
    public void GotFish()
    {
        Invoke("LoadScene", 1f);
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.YouAreTheFishAudio, transform.position);
    }
    public void LoadScene()
    {
        GameController.GetGameController().ChangeScene(3);
        OnGameFinished?.Invoke();
    }
}

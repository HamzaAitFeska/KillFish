using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    [SerializeField] Button ResumeButton;
    public void UnpauseGame()
    {
        GameController.GetPause().UnpauseGame();
    }
    public void IPauseGame()
    {
        if(GameController.GetGameController().GetPlayer().ActionManager.GamePad()) ResumeButton.Select();
    }
    public void GoMainMenu()
    {

    }
    public void QuitApp()
    {
        Application.Quit();
    }
    public void OpenSettings()
    {
        GameController.GetPause().OpenSettings();
    }

    public void ClickSound()
    {
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.UIClick, transform.position);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using TMPro;
using UnityEngine.Localization.Settings;
using Unity.VisualScripting;

public class MainMenu : MonoBehaviour
{
    SettingsMenu settingsMenu;
    [Header("Buttons Gameobjects")]
    [SerializeField] GameObject buttonsContainer;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject easyButton;
    [SerializeField] GameObject mediumButton;
    [SerializeField] GameObject hardButton;
    [SerializeField] GameObject optionsButton;
    [SerializeField] GameObject creditsButton;
    [SerializeField] GameObject leaveGameButton;
    [SerializeField] GameObject SocialsGameObject;

    [Header("Others")]
    [SerializeField] LocalizationSelector selector;
    [SerializeField] RawImage imgVideoPlayer;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] Image blackScreenOnSkipAnimation;
    [SerializeField] GameObject lavaSoundEmitter;
    [HideInInspector] public bool OptionsOpened = false;
    bool hasStarted = false;
    bool hasSkippedAnimation = false;
    bool PlayButtonPressed = false;
    EventInstance audioVideo;
    EventInstance mainMenuMusic;

    private void Awake()
    {
        selector.ChangeLocalID(PlayerPrefs.GetInt("Settings_LanguageID"));
    }
    void Start()
    {
        settingsMenu = Resources.Load<SettingsMenu>("Prefabs/Menus/OptionsMenuPrefabCanvas");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        mainMenuMusic = AudioManager.GetAudioManager().CreateEventInstance(AudioManager.GetAudioManager().Events.MenuMusic);
        mainMenuMusic.start();
        hasStarted = false;
        hasSkippedAnimation = false;
        GameController.GetGameController();
    }

    public void SelectFirstButton()
    {
        playButton.GetComponent<Button>().Select();
    }
    public void StartEasyGame()
    {
        GameController.GetGameController().GetDifficulty().currentDifficulty = DifficultyType.EASY;
        StartGame();
    }
    public void StartMediumGame()
    {
        GameController.GetGameController().GetDifficulty().currentDifficulty = DifficultyType.MEDIUM;
        StartGame();
    }
    public void StartHardGame()
    {
        GameController.GetGameController().GetDifficulty().currentDifficulty = DifficultyType.HARD;
        StartGame();
    }
    void StartGame()
    {
        if (hasStarted) return;
        GameController.GetGameController().ResetGameInfo();
        lavaSoundEmitter.SetActive(false);
        hasStarted = true;
        videoPlayer.Play();
        mainMenuMusic.stop(STOP_MODE.ALLOWFADEOUT);
        audioVideo = AudioManager.GetAudioManager().CreateEventInstance(AudioManager.GetAudioManager().Events.VideoIntroAudio);
        audioVideo.start();
        imgVideoPlayer.color = new Color(1, 1, 1, 1);
        Invoke("LoadScene", ((float)videoPlayer.clip.length)-1f);
    }
    
    public void SkipVideoPress()
    {
        if(hasStarted){
            SkipAnimation();
        }
    }

    public void LoadScene()
    {
        GameController.GetGameController().ChangeScene(1);
    }

    public void SkipAnimation()
    {
        if (hasSkippedAnimation) return;
        hasSkippedAnimation = true;
        blackScreenOnSkipAnimation.color = new Color(0, 0, 0, 1);
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.UIHover, transform.position);
        audioVideo.stop(STOP_MODE.ALLOWFADEOUT);
        LoadScene();
    }
    public void OpenOptions()
    {
        if (OptionsOpened) return;
        OptionsOpened = true;
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.UIClick, transform.position);
        Instantiate(settingsMenu);
    }
    public void ShowCredits()
    {
        GameController.GetGameController().ChangeScene(3);
    }
    public void PressPlayButton()
    {
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.UIClick, transform.position);

        if (PlayButtonPressed)
        {
            PlayButtonPressed = false;
            easyButton.SetActive(false);
            mediumButton.SetActive(false);
            hardButton.SetActive(false);

            optionsButton.SetActive(true);
            creditsButton.SetActive(true);
            leaveGameButton.SetActive(true);
        }
        else
        {
            PlayButtonPressed = true;

            easyButton.SetActive(true);
            mediumButton.SetActive(true);
            hardButton.SetActive(true);

            optionsButton.SetActive(false);
            creditsButton.SetActive(false);
            leaveGameButton.SetActive(false);
        }
    }
    public void GameExit()
    {
        Application.Quit();
    }

    public void OpenCloseSocials()
    {
        SocialsGameObject.SetActive(!SocialsGameObject.activeSelf);
    }

}

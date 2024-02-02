using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Localization.Settings;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Slider SensibilityXSlider;
    [SerializeField] Slider SensibilityYSlider;
    [SerializeField] Slider FovSlider;

    [SerializeField] Toggle CameraTilt;
    [SerializeField] Toggle FOVChanges;
    [SerializeField] Toggle ScreenShake;
    [SerializeField] Toggle FullScreen;
    [SerializeField] Toggle MouseYInverted;

    [SerializeField] TMP_Dropdown ResolutionsDropdown;
    private Resolution[] resolutions;
    private List<Resolution> filterResolutions;
    float currentRefreshRate;
    int currentResolutionIndex;

    [SerializeField] TMP_Dropdown FPSLimitDropdown;
    FPSLimits currentFPSLimit;

    [SerializeField] TMP_Dropdown QualityDropdown;

    [SerializeField] Slider MasterVolume;
    [SerializeField] Slider MusicVolume;
    [SerializeField] Slider SFXVolume;

    int lastSettingTypeOpened = 0; //0 is gameplay, 1 graphics, 2 audio, 3 language
    [SerializeField] GameObject GameplayContainerSettingType;
    [SerializeField] GameObject GraphicsContainerSettingType;
    [SerializeField] GameObject AudioContainerSettingType;
    [SerializeField] GameObject LanguageContainersSettingType;

    [SerializeField] Button GameplayButton;
    [SerializeField] Button GraphicsButton;
    [SerializeField] Button AudioButton;
    [SerializeField] Button LanguageButton;
    private void Start()
    {
        UpdateInfo();
    }

    public void BackButton()
    {
        BackToPauseMenu();
    }

    public void BackToPauseMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) { BackToMainMenu();
            FindObjectOfType<MainMenu>().SelectFirstButton();
            return; 
        }
        GameController.GetPause().PauseGame();
        Destroy(gameObject);
    }
    public void BackToMainMenu()
    {
        FindObjectOfType<MainMenu>().OptionsOpened = false;
        Destroy(gameObject);
    }
    public void ResetValues()
    {
        GameController.GetGameController().GetSettingsData().ResetValuesButton();
        UpdateInfo();
    }
    void UpdateInfo()
    {
        SensibilityXSlider.value = GameController.GetGameController().GetSettingsData().SensibilityXMultiplier;
        SensibilityYSlider.value = GameController.GetGameController().GetSettingsData().SensibilityYMultiplier;
        FovSlider.value = GameController.GetGameController().GetSettingsData().CameraFOVMultiplier;
        CameraTilt.isOn = GameController.GetGameController().GetSettingsData().CameraTilt;
        FOVChanges.isOn = GameController.GetGameController().GetSettingsData().FovChanges;
        FullScreen.isOn = GameController.GetGameController().GetSettingsData().FullScreen;
        ScreenShake.isOn = GameController.GetGameController().GetSettingsData().ScreenShake;
        MouseYInverted.isOn = GameController.GetGameController().GetSettingsData().MouseYInverted;

        //Resolutions
        resolutions = Screen.resolutions;
        filterResolutions = new List<Resolution>();
        ResolutionsDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filterResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filterResolutions.Count; i++)
        {
            string resolutionOption = filterResolutions[i].width + "x" + filterResolutions[i].height;
            options.Add(resolutionOption);
            if (filterResolutions[i].width == Screen.width && filterResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        ResolutionsDropdown.AddOptions(options);
        ResolutionsDropdown.value = currentResolutionIndex;
        ResolutionsDropdown.RefreshShownValue();

        //FPS
        FPSLimitDropdown.value = ParseFPSToIndex();

        //Quality
        QualityDropdown.value = GameController.GetGameController().GetSettingsData().QualityIndex;
        QualityDropdown.RefreshShownValue();


        //Volume
        MasterVolume.value = GameController.GetGameController().GetSettingsData().MasterVolume;
        MusicVolume.value = GameController.GetGameController().GetSettingsData().MusicVolume;
        SFXVolume.value = GameController.GetGameController().GetSettingsData().SfxVolume;

        //Open Setting Type
        OpenSettingType(PlayerPrefs.GetInt("SettingType"));


    }
    public void SetSensibilityX(float sensibility)
    {
        GameController.GetGameController().GetSettingsData().SaveSensibilityXMultiplier(sensibility);
        UpdateInfo();
    }
    public void SetSensibilityY(float sensibility)
    {
        GameController.GetGameController().GetSettingsData().SaveSensibilityYMultiplier(sensibility);
        UpdateInfo();
    }
    public void SetFOV(float fov)
    {
        GameController.GetGameController().GetSettingsData().SaveCameraFOV(fov);
        UpdateInfo();
    }
    public void SetCameraTilt(bool tilt)
    {
        GameController.GetGameController().GetSettingsData().SaveCameraTilt(tilt);
    }
    public void SetScreenShake(bool shake)
    {
        GameController.GetGameController().GetSettingsData().SaveScreenShake(shake);
        Debug.Log(shake);
    }
    public void SetFovChanges(bool fovchanges)
    {
        GameController.GetGameController().GetSettingsData().SaveFOVChanges(fovchanges);
    }
    public void SetFullScreen(bool fullscreen)
    {
        GameController.GetGameController().GetSettingsData().SaveFullScreen(fullscreen);
    }
    public void SetMouseYInverted(bool inversion)
    {
        GameController.GetGameController().GetSettingsData().SaveMouseYInverted(inversion);
    }
    public void SetResolution(int resIndex)
    {
        Resolution resolution = filterResolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, GameController.GetGameController().GetSettingsData().FullScreen);
        GameController.GetGameController().GetSettingsData().SaveResolution(resolution.width, resolution.height);
    }
    public void SetMasterVolume(float masterVol)
    {
        GameController.GetGameController().GetSettingsData().SaveMasterVolume(masterVol);
    }
    public void SetMusicVolume(float musicVol)
    {
        GameController.GetGameController().GetSettingsData().SaveMusicVolume(musicVol);
    }
    public void SetSFXVolume(float sfxVol)
    {
        GameController.GetGameController().GetSettingsData().SaveSFXVolume(sfxVol);
    }
    public void SetQuality(int index)
    { 
        GameController.GetGameController().GetSettingsData().SaveQualityIndex(index);
    }

    public void ClickSound()
    {
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.UIClick, transform.position);
    }
    public void HoverSound()
    {
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.UIHover, transform.position);
    }
    public void SetFPSLimit(int fpsIndex)
    {
        switch (fpsIndex)
        {
            case 0:
                currentFPSLimit = FPSLimits.nolimit;
                break;
            case 1:
                currentFPSLimit = FPSLimits.limit30;
                break;
            case 2:
                currentFPSLimit = FPSLimits.limit60;
                break;
            case 3:
                currentFPSLimit = FPSLimits.limit144;
                break;
            case 4:
                currentFPSLimit = FPSLimits.limit240;
                break;
        }
        Debug.Log((int)currentFPSLimit);
        Application.targetFrameRate = (int)currentFPSLimit;
        GameController.GetGameController().GetSettingsData().SaveFPSLimit((int)currentFPSLimit);
    }
    int ParseFPSToIndex()
    {
        switch (GameController.GetGameController().GetSettingsData().FPSLimit)
        {
            case 0:
                return 0;
            case 30:
                return 1;
            case 60:
                return 2;
            case 144:
                return 3;
            case 240:
                return 4;
        }
        return 0;
    }

    //Opening type menu
    public void OpenTypeMenu(int type)
    {
        //0 is Gameplay
        //1 is Graphics
        //2 is Audio
        //3 is Language

        lastSettingTypeOpened = type;
        PlayerPrefs.SetInt("SettingType", lastSettingTypeOpened);
        OpenSettingType(lastSettingTypeOpened);
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.UIClick, transform.position);
    }
    void OpenSettingType(int settingType)
    {
        GameplayContainerSettingType.SetActive(false);
        GraphicsContainerSettingType.SetActive(false);
        AudioContainerSettingType.SetActive(false);
        LanguageContainersSettingType.SetActive(false);


        switch (settingType)
        {
            case 0:
                GameplayContainerSettingType.SetActive(true);
                GameplayButton.Select();
                return;
            case 1:
                GraphicsContainerSettingType.SetActive(true);
                GraphicsButton.Select();
                return;
            case 2:
                AudioContainerSettingType.SetActive(true);
                AudioButton.Select();
                return;
            case 3:
                LanguageContainersSettingType.SetActive(true);
                LanguageButton.Select();
                return;

        }
    }
    public void SaveLanguage(int id)
    {
       GameController.GetGameController().GetSettingsData().SaveLanguage(id);
    }
}

public enum FPSLimits
{
    nolimit = 0, limit30 = 30, limit60 = 60, limit144 = 144, limit240 = 240
}


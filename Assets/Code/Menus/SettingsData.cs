using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Localization.Settings;

[CreateAssetMenu(fileName = "Settings Data", menuName = "Data/Settings"), System.Serializable]
public class SettingsData : ScriptableObject
{

    float sensibilityXMultiplier;
    public float SensibilityXMultiplier { get { return sensibilityXMultiplier; }}

    float sensibilityYMultiplier;
    public float SensibilityYMultiplier { get { return sensibilityYMultiplier; } }

    float cameraFOVMultiplier;
    public float CameraFOVMultiplier { get { return cameraFOVMultiplier; }}

    bool cameraTilt;
    public bool CameraTilt { get { return cameraTilt; }}

    bool screenShake;
    public bool ScreenShake { get { return screenShake; }}

    bool fovChanges;
    public bool FovChanges { get { return fovChanges; }}

    bool fullScreen;
    public bool FullScreen { get { return fullScreen; }}

    int widthResolution;
    public int WidthResolution { get { return widthResolution; }}

    int heightResolution;
    public int HeightResolution { get { return heightResolution; }}

    int fpsLimit;
    public int FPSLimit { get { return fpsLimit; }}

    float masterVolume;
    public float MasterVolume { get { return masterVolume; }}

    float musicVolume;
    public float MusicVolume { get { return musicVolume; }}

    float sfxVolume;
    public float SfxVolume { get { return sfxVolume; } }

    int qualityIndex;
    public int QualityIndex { get { return qualityIndex; }}

    bool mouseYInverted;
    public bool MouseYInverted { get { return mouseYInverted; }}

    int languageID;
    public int LanguageID { get { return languageID; }}
    public void LoadData()
    {
        if (!PlayerPrefs.HasKey("Settings_QualityIndex"))
        {
            ResetValuesButton();
        }
        else { 
            sensibilityXMultiplier = PlayerPrefs.GetFloat("Settings_SensibilityXMultiplier");
            sensibilityYMultiplier = PlayerPrefs.GetFloat("Settings_SensibilityYMultiplier");
            cameraFOVMultiplier = PlayerPrefs.GetFloat("Settings_CameraFOVMultiplier");
            cameraTilt = PlayerPrefs.GetInt("Settings_CameraTilt") == 1 ? true : false ;
            screenShake = PlayerPrefs.GetInt("Settings_ScreenShake") == 1 ? true : false ;
            fovChanges = PlayerPrefs.GetInt("Settings_FovChanges") == 1 ? true : false ;
            fullScreen = PlayerPrefs.GetInt("Settings_FullScreen") == 1 ? true : false;
            SaveFullScreen(FullScreen);
            widthResolution = PlayerPrefs.GetInt("Settings_WidthResolution");
            heightResolution = PlayerPrefs.GetInt("Settings_HeightResolution");
            SaveResolution(widthResolution, heightResolution);
            fpsLimit = PlayerPrefs.GetInt("Settings_FPSLimit");
            SaveFPSLimit(fpsLimit);
            masterVolume = PlayerPrefs.GetFloat("Settings_Volume_Master");
            SaveMasterVolume(masterVolume);
            musicVolume = PlayerPrefs.GetFloat("Settings_Volume_Music");
            SaveMusicVolume(musicVolume);
            sfxVolume = PlayerPrefs.GetFloat("Settings_Volume_SFX");
            SaveSFXVolume(sfxVolume);
            qualityIndex = PlayerPrefs.GetInt("Settings_QualityIndex");
            SaveQualityIndex(qualityIndex);
            mouseYInverted = PlayerPrefs.GetInt("Settings_MouseYInverted") == 1 ? true : false;
            languageID = PlayerPrefs.GetInt("Settings_LanguageID");
        }
    }
    public void ResetValuesButton()
    {
        SaveSensibilityXMultiplier(1);
        SaveSensibilityYMultiplier(1);
        SaveCameraFOV(1);
        SaveCameraTilt(true);
        SaveScreenShake(true);
        SaveFOVChanges(true);
        SaveFullScreen(true);
        SaveFullScreen(fullScreen);
        SaveResolution(1920, 1080);
        SaveFPSLimit(0);
        //Volume
        SaveMasterVolume(1);
        SaveMusicVolume(1);
        SaveSFXVolume(1);
        SaveQualityIndex(2);
        SaveMouseYInverted(false);
        //Language
        SaveLanguage(0);

    }
    private void Reset()
    {
        ResetValuesButton();
    }
    public void SaveSensibilityXMultiplier(float multiplier)
    {
        sensibilityXMultiplier = multiplier;
        PlayerPrefs.SetFloat("Settings_SensibilityXMultiplier", sensibilityXMultiplier);
    }
    public void SaveSensibilityYMultiplier(float multiplier)
    {
        sensibilityYMultiplier = multiplier;
        PlayerPrefs.SetFloat("Settings_SensibilityYMultiplier", sensibilityYMultiplier);
    }
    public void SaveCameraFOV(float fov)
    {
        cameraFOVMultiplier = fov;
        PlayerPrefs.SetFloat("Settings_CameraFOVMultiplier", cameraFOVMultiplier);
    }
    public void SaveCameraTilt(bool tilt)
    {
        cameraTilt = tilt;
        int tiltToInt = cameraTilt ? 1 : 0;
        PlayerPrefs.SetInt("Settings_CameraTilt", tiltToInt);
    }
    public void SaveScreenShake(bool shake)
    {
        screenShake = shake;
        int shakeToInt = screenShake ? 1 : 0;
        Debug.Log(shakeToInt);
        PlayerPrefs.SetInt("Settings_ScreenShake", shakeToInt);
    }
    public void SaveFOVChanges(bool fovchange)
    {
        fovChanges = fovchange;
        int fovChangeToInt = fovChanges ? 1 : 0;
        PlayerPrefs.SetInt("Settings_FovChanges", fovChangeToInt);
    }
    public void SaveFullScreen(bool fullscreen)
    {
        fullScreen = fullscreen;
        int FullScreenToInt = fullScreen ? 1 : 0;
        PlayerPrefs.SetInt("Settings_FullScreen", FullScreenToInt);

        Screen.fullScreen = fullscreen;
    } 

    public void SaveResolution(int width, int height)
    {
        widthResolution = width;
        PlayerPrefs.SetInt("Settings_WidthResolution", widthResolution);
        heightResolution = height;
        PlayerPrefs.SetInt("Settings_HeightResolution", heightResolution);
    }

    public void SaveFPSLimit(int limit)
    {
        fpsLimit = limit;
        PlayerPrefs.SetInt("Settings_FPSLimit", fpsLimit);
    }

    public void SaveMasterVolume(float masterVol)
    {
        masterVolume = masterVol;
        PlayerPrefs.SetFloat("Settings_Volume_Master", masterVolume);

        RuntimeManager.GetBus("bus:/").setVolume(MasterVolume);
    }
    public void SaveMusicVolume(float musicVol)
    {
        musicVolume = musicVol;
        PlayerPrefs.SetFloat("Settings_Volume_Music", musicVolume);

        RuntimeManager.GetBus("bus:/Music").setVolume(musicVolume);
    }
    public void SaveSFXVolume(float sfxVol)
    {
        sfxVolume = sfxVol;
        PlayerPrefs.SetFloat("Settings_Volume_SFX", sfxVolume);

        RuntimeManager.GetBus("bus:/SFX").setVolume(sfxVolume);
        RuntimeManager.GetBus("bus:/SFXNoPause").setVolume(sfxVolume);
    }
    public void SaveQualityIndex(int index)
    {
        qualityIndex = index;
        PlayerPrefs.SetInt("Settings_QualityIndex", qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex, false);
    }

    public void SaveMouseYInverted(bool mouseInverted)
    {
        mouseYInverted = mouseInverted;
        int mouseYInvertedInt = mouseYInverted ? 1 : 0;
        PlayerPrefs.SetInt("Settings_MouseYInverted", mouseYInvertedInt);
    }

    public void SaveLanguage(int id)
    {
        // 0 is english, 1 is french, 2 is spanish, 3 is russian, 4 is chinese
        languageID = id;
        PlayerPrefs.SetInt("Settings_LanguageID", languageID);

        //Need to set it
        SetSelectedLocal();
    }

    void SetSelectedLocal()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageID];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGameController : MonoBehaviour
{
    PauseGame PauseCanvasPrefab;
    PauseGame PauseCanvasInstance;
    SettingsMenu settingsMenu;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        settingsMenu = Resources.Load<SettingsMenu>("Prefabs/Menus/OptionsMenuPrefabCanvas");
    }
    public void PauseButton()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 3) PauseKeyPressed();
    }
    public void BackButton()
    {
        if (Time.timeScale == 0.0f) //if its paused
        {
            UnpauseGame();
        }
    }
    void PauseKeyPressed()
    {

        if (Time.timeScale == 0.0f) //if its paused
        {
            UnpauseGame();
        }
        else //if not paused
        {
            PauseGame();
        }
    }
    public void PauseGame()
    {
        GetPauseCanvasPrefab().gameObject.SetActive(true);
        PauseCanvasInstance.IPauseGame();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void UnpauseGame()
    {
        GetPauseCanvasPrefab().gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    PauseGame GetPauseCanvasPrefab()
    {
        if(PauseCanvasInstance == null)
        {
            PauseCanvasPrefab = Resources.Load<PauseGame>("Prefabs/Menus/PauseGamePrefabCanvas");
            PauseCanvasInstance = Instantiate(PauseCanvasPrefab, transform);
        }
        return PauseCanvasInstance;
    }

    public void OpenSettings()
    {
        GetPauseCanvasPrefab().gameObject.SetActive(false);
        Instantiate(settingsMenu);
    }
}

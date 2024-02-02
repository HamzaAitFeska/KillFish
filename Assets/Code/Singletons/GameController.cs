using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    static GameController m_GameController = null;
    static PauseGameController m_PauseGame = null;

    public static GameObject fades;
    int m_SceneIndex;

    static PlayerVariableContainer playerVariableContainer;
    static EnemySpawner LevelEnemySpawner;

    UpgradeManager upgradeManager;
    SettingsData settingsManager;
    RespawnData respawnManager;
    DifficultySelection difficultyManager;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        GetPause();
        GetSettingsData();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        SetFadeOutInCanvas();
    }

    #region SingletonStuff
    public static GameController GetGameController()
    {
        if(m_GameController == null)
        {
            m_GameController = new GameObject("GameController").AddComponent<GameController>();
            m_GameController.gameObject.AddComponent<SteamManager>();
            fades = Resources.Load<GameObject>("Prefabs/Fades");

        }
        return m_GameController;
    }
    public void SetPlayer(PlayerVariableContainer Player)
    {
        playerVariableContainer = Player;
    }
    public PlayerVariableContainer GetPlayer()
    {
        return playerVariableContainer;
    }
    public void SetEnemySpawner(EnemySpawner Spawner)
    {
        LevelEnemySpawner = Spawner;
    }
    public EnemySpawner GetSpawner()
    {
        return LevelEnemySpawner;
    }
    public static PauseGameController GetPause()
    {
        if (m_PauseGame == null)
        {
            PauseGameController go = Resources.Load<PauseGameController>("Prefabs/Controllers/PauseController");
            m_PauseGame = Instantiate(go);
        }
        return m_PauseGame;
    }
    public static void DestroySingleton()
    {
        if (m_GameController != null)
        {
            GameObject.Destroy(m_GameController.gameObject);
        }
        m_GameController = null;
    }
    #endregion

    
    public void ChangeScene(int index)
    {
        m_SceneIndex = index;
        SetFadeInCanvas();
    }

    #region FadeInOutSetters
    public void SetFadeInCanvas()
    {
        GameObject go = Instantiate(fades);

        float blackColor = 0.0f;
        Image img = go.GetComponentInChildren<Image>();
        img.color = new Color(blackColor, blackColor, blackColor, 0f);

        StartCoroutine(SetFadeIn(blackColor, img));
    }

    IEnumerator SetFadeIn(float blackColor, Image img)
    {
        float fadeDuration = 1f;

        while (img.color.a < 0.99f)
        {
            img.color = new Color(blackColor, blackColor, blackColor, Mathf.MoveTowards(img.color.a, 1, Time.unscaledDeltaTime / fadeDuration));
            yield return new WaitForEndOfFrame();
        }
        img.color = new Color(blackColor, blackColor, blackColor, 1);

        SceneLoads();
    }
    private async void SceneLoads()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(m_SceneIndex);
        // Wait until the scene is fully loaded
        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }
        GetRespawnData().Respawn(m_SceneIndex);
        SetFadeOutInCanvas();
    }

    public void SetFadeOutInCanvas()
    {
        GameObject go = Instantiate(fades);

        float blackColor = 0.0f;
        Image img = go.GetComponentInChildren<Image>();
        img.color = new Color(blackColor, blackColor, blackColor, 1.0f);

        StartCoroutine(SetFadeOut(blackColor, img));
    }

    IEnumerator SetFadeOut(float blackColor, Image img)
    {
        float fadeDuration = 1f;

        while (img.color.a > 0.02f)
        {
            img.color = new Color(blackColor, blackColor, blackColor, Mathf.MoveTowards(img.color.a, 0, Time.unscaledDeltaTime / fadeDuration));
            yield return new WaitForEndOfFrame();
        }
        Destroy(img.gameObject);
    }
    #endregion


    #region UpgradeManagerData
    public UpgradeManager GetUpgradeManagerData()
    {
        if(upgradeManager == null)
        {
            upgradeManager = ScriptableObject.CreateInstance<UpgradeManager>();
        }
        return upgradeManager;
    }
    #endregion

    public SettingsData GetSettingsData()
    {
        if(settingsManager == null)
        {
            settingsManager = ScriptableObject.CreateInstance<SettingsData>();
        }
        settingsManager.LoadData();
        return settingsManager;
    }


    public RespawnData GetRespawnData()
    {
        if(respawnManager == null)
        {
            respawnManager = ScriptableObject.CreateInstance<RespawnData>();
        }
        return respawnManager;
    }

    public DifficultySelection GetDifficulty()
    {
        if(difficultyManager == null)
        {
            difficultyManager = Resources.Load<DifficultySelection>("Scriptable/Difficulty");
        }
        return difficultyManager;
    }
    public void ResetGameInfo()
    {
        GameController.GetGameController().GetRespawnData().UnsetRespawnInfo();
        GameController.GetGameController().GetUpgradeManagerData().ResetValues();
    }

    private void OnApplicationQuit()
    {
        upgradeManager.ResetValues();
    }
}

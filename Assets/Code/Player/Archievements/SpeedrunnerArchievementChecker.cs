using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedrunnerArchievementChecker : MonoBehaviour
{
    float startTime;
    float endTime;
    float totalTime;

    float wantedTimeToArchievement; 
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        startTime = Time.time; 
        wantedTimeToArchievement = 600f; //10min
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += CheckDestroy;
        FishFinal.OnGameFinished += CheckFinished;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= CheckDestroy;
        FishFinal.OnGameFinished -= CheckFinished;
    }

    void CheckDestroy(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) {
            Destroy(this.gameObject);
        }
    }
    void CheckFinished()
    {
       endTime = Time.time;
       
       totalTime = endTime - startTime;
       
       if (totalTime < wantedTimeToArchievement) ArchievementsNameConstantsData.SpeedRunnerAccomplisher();
    }
}

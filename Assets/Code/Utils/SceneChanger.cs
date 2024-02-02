using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void LoadScene(int sceneNumber)
    {
        GameController.GetGameController().ChangeScene(sceneNumber);
    }
    public void SetIfRespawnPos(bool inRespawnPos = false)
    {
        GameController.GetGameController().GetRespawnData().respawnInfo = inRespawnPos;
    }

    public void ReloadSameScene(bool inRespawnPos = false)
    {

        GameController.GetGameController().GetRespawnData().respawnInfo = inRespawnPos;

        GameController.GetGameController().ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }
}

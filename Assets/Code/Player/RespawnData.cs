using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RespawnData : ScriptableObject
{

    public bool respawnInfo;

    public Vector3 lastRespawnPos;
    float lastYaw;
    float lastPitch;

    public void SetRespawnData(Vector3 position, float yaw, float pitch)
    {
        lastRespawnPos = position;
        lastYaw = yaw;
        lastPitch = pitch;
        respawnInfo = true;
    }

    public void UnsetRespawnInfo()
    {
        respawnInfo = false;
    }
    public void Respawn(int index)
    {
        PlayerVariableContainer playerVariableContainer = GameController.GetGameController().GetPlayer();

        if (!respawnInfo)
        {
            if (index == 1)
            {
                AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.UIStart, new Vector3(0, 0, 0));
                playerVariableContainer.PlayerAnimationSystem.GameStarts();
                playerVariableContainer.CameraController.SetStartYawnPitch();
                playerVariableContainer.SetCanMove(false);
            }
            return;
        }        

        //Cant move
        playerVariableContainer.CharacterController.enabled = false;
        playerVariableContainer.SetCanMove(false);

        //Change variables
        playerVariableContainer.transform.position = new Vector3(lastRespawnPos.x, lastRespawnPos.y, lastRespawnPos.z);
        playerVariableContainer.CameraController.SetYaw(lastYaw);
        playerVariableContainer.CameraController.SetPitch(lastPitch);

        //Movable again
        playerVariableContainer.CharacterController.enabled = true;
        playerVariableContainer.SetCanMove(true);
    }
    public void RawRespawn(float posX, float posY, float posZ)
    {

        PlayerVariableContainer playerVariableContainer = GameController.GetGameController().GetPlayer();
        //Cant move
        playerVariableContainer.CharacterController.enabled = false;
        playerVariableContainer.SetCanMove(false);

        //Change variables
        playerVariableContainer.transform.position = new Vector3(posX, posY, posZ);
        playerVariableContainer.CameraController.SetYaw(0);
        playerVariableContainer.CameraController.SetPitch(0);

        //Movable again
        playerVariableContainer.CharacterController.enabled = true;
        playerVariableContainer.SetCanMove(true);
    }
}

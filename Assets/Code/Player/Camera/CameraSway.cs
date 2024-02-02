using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSway : MonoBehaviour
{
    [SerializeField] bool WantsCameraSway = true;
    [SerializeField] bool InverseDirectionSway;
    [SerializeField] float movementSwayMultiplier;
    [SerializeField] [Range(0f,1f)] float swayMultiplierOnSniperMode;
    [SerializeField] float smooth;

    Quaternion initialRotation;
    private void Start()
    {
        initialRotation = transform.localRotation;
    }

    public void SwayUpdate()
    {
        if (!GameController.GetGameController().GetSettingsData().CameraTilt) return; //This for player
        if (!WantsCameraSway) return; //This for designer
        //calculate camera rotation

        float direction = InverseDirectionSway ? -1f : 1f;
        if (GameController.GetGameController().GetPlayer().WeaponController.IsInSniperMode()) direction *= swayMultiplierOnSniperMode;

            Quaternion targetRotation = Quaternion.Euler(0, 0, GameController.GetGameController().GetPlayer().MovementController.FPSMoveController.CurrentSpeedYAxis()* direction * movementSwayMultiplier) * initialRotation;

        //rotate
        targetRotation = Quaternion.Normalize(targetRotation);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}

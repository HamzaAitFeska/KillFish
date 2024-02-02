using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("If you want to unable some sway to test")]
    [SerializeField] bool WantsCameraSway = true;
    [SerializeField] bool WantsMovementSway = true;

    [Header("Weapon Sway Camera Settings")]
    [SerializeField] float cameraSwayMultiplier;
    [SerializeField][Range(0,1)] float cameraSwayMultiplierOnSniperMode;

    [Header("Weapon Sway Movement Settings")]
    [SerializeField] float movementSwayMultiplier;
    [SerializeField] bool InverseDirectionSway;
    [SerializeField][Range(0,1)] float movementSwayMultiplierOnSniperMode;

    [Header("Overall")]
    [SerializeField] float smooth;

    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        if (!GameController.GetGameController().GetPlayer().CanMove()) return;
        //input
        float mouseX = Input.GetAxis("Mouse X") * cameraSwayMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSwayMultiplier;

        if (GameController.GetGameController().GetPlayer().WeaponController.IsInSniperMode())
        {
            mouseX *= cameraSwayMultiplierOnSniperMode;
            mouseY *= cameraSwayMultiplierOnSniperMode;
        }

        //calculate CAMERA 
        Quaternion xRotation = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion yRotation = Quaternion.AngleAxis(mouseX, Vector3.up);

        //calculate MOVEMENT
        float direction = InverseDirectionSway ? -1f : 1f;
        if (GameController.GetGameController().GetPlayer().WeaponController.IsInSniperMode()) direction *= movementSwayMultiplierOnSniperMode;
            Quaternion zRotation = Quaternion.AngleAxis(GameController.GetGameController().GetPlayer().MovementController.FPSMoveController.CurrentSpeedYAxis() * direction * movementSwayMultiplier, Vector3.forward);

        //calculate FINAL ROTATION
        Quaternion targetRotation = initialRotation;
        if (WantsCameraSway) targetRotation = targetRotation * xRotation * yRotation;   
        if(WantsMovementSway) targetRotation = targetRotation * zRotation;

        //rotate
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}

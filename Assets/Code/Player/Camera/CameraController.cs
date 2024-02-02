using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    PlayerVariableContainer playerVariableContainer;
    [SerializeField] Transform pitchController;

    [Header("Camera")]
    public Camera playerCamera;
    public Camera weaponCamera;

    float m_Yaw; // Yaw angle: rotation of player in horizontal axis (X)
    public float GetYaw() { return m_Yaw; }
    public void SetYaw(float newyaw) { m_Yaw = newyaw; }
    float m_Pitch; //Pitch angle: rotation of player in vertical axis (Y)
    public float GetPitch() { return m_Pitch; }
    public void SetPitch(float newPitch) { m_Pitch = newPitch;}

    [Header("Rotation Speed")] //Controls the sensibility of the mouse for the player
    [SerializeField][Range(180, 540)] float m_YawRotationalSpeed;
    [SerializeField][Range(0, 360)] float m_PitchRotationalSpeed;

    [Header("Pitch Variables")] //Controls Pitch variables as how up/down can player look
    [SerializeField][Range(-100, -50)] float m_MinPitch;
    [SerializeField][Range(45, 90)] float m_MaxPitch;
    [SerializeField] float startPitchAngle;//Look pitchcontroller.x
    [SerializeField] float startYawAngle;//Look transform.y

    [Header("FOV related. As multipliers of normalFOV")]
    //FOVsRelated
    [SerializeField] float normalFOV = 70;
    [SerializeField] float hookFOV;
    [SerializeField] float dashFOV;
    //Shoot Rifle FOV
    float shootRifleFOV;
    public void SetShootRifleFOV(float fov) { shootRifleFOV = fov; }

    public bool RifleFOVEnabled { get; set; }
    public bool DashFOVEnabled { get; set; }
    public bool HookFOVEnabled { get; set; }

    //FOV
    float fovSmoothness;
    [SerializeField][Range(0f, 20f)] float normalSmoothFOVChange;
    [SerializeField][Range(0f, 20f)] float hookSmoothFOVChange;
    [SerializeField][Range(0, 20f)] float dashSmoothFOVChange;
    float targetFOV;

    //WeaponCameraFOV
    [SerializeField][Range(0f, 20f)] float weaponCamFOVSmoothness;
    float wNormalFOV;
    float weaponCamShootRifleFOV;
    public void SetWeaponCamShootRifleFOV(float fov) { weaponCamShootRifleFOV = fov; }

    [Header("Looking Down")]
    [SerializeField] float downAngle;

    [Header("LifeHacks for Unity Editor")]
    bool m_AimLocked = true;
    bool m_AngleLocked = false;

    public KeyCode m_DebugLockAngleKeyCode = KeyCode.I;
    public KeyCode m_DebugLockKeyCode = KeyCode.O;
    public KeyCode m_DebugPauseKeyCode = KeyCode.P;

    //Freeze
    bool freezeCamera;
    public void SetFreezeCamera(bool value) { freezeCamera = value; }


    [SerializeField] CameraShaker cameraShaker;
    public CameraShaker GetCameraShaker() { return cameraShaker; }

    [SerializeField] CameraShootingRecoilShake cameraShootRecoilShake;
    public CameraShootingRecoilShake GetCameraShootRecoilShake() { return cameraShootRecoilShake; }

    [SerializeField] CameraSway cameraSway;

    public void SetStartYawnPitch()
    {
        m_Yaw = startYawAngle;
        m_Pitch = startPitchAngle;

    }
    void Awake()
    {
        playerVariableContainer = GameController.GetGameController().GetPlayer();

        SetStartYawnPitch();
        targetFOV = GameController.GetGameController().GetSettingsData().CameraFOVMultiplier;
        wNormalFOV = weaponCamera.fieldOfView;


        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Start()
    {
        playerVariableContainer = GameController.GetGameController().GetPlayer();
    }
    // Update is called once per frame
    public void CameraUpdate()
    {
        // Player and Camera rotation
        //Get Input
        float l_mouseX = playerVariableContainer.ActionManager.CameraMovement().x;
        float l_mouseY = playerVariableContainer.ActionManager.CameraMovement().y;

            //Frezze only the angle of the camera
            if (freezeCamera)
            {
                l_mouseX = 0.0f;
                l_mouseY = 0.0f;
            }
            //If it's angle locked don't set input to 0
#if UNITY_EDITOR
            if (m_AngleLocked)
            {
                l_mouseX = 0.0f;
                l_mouseY = 0.0f;
            }
#endif
        float sensibilityX = GameController.GetGameController().GetSettingsData().SensibilityXMultiplier;
        float sensibilityY = GameController.GetGameController().GetSettingsData().SensibilityYMultiplier;
        //Update camera variables depending on input
        m_Yaw += l_mouseX * m_YawRotationalSpeed * sensibilityX * Time.deltaTime;
        if (GameController.GetGameController().GetSettingsData().MouseYInverted)
        {
            m_Pitch += l_mouseY * m_PitchRotationalSpeed * sensibilityY * Time.deltaTime;
        }
        else
        {
            m_Pitch -= l_mouseY * m_PitchRotationalSpeed * sensibilityY * Time.deltaTime;
        }
            m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch); //clamps how up/down can player looks

            //Rotates player's
            transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);//rotation of the player horizonal axis
            pitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f); //rotation of the weapon's vertical axis


            //Input for comfortable debugging
#if UNITY_EDITOR
            UpdateCameraInputDebug();
#endif
            FovFSM();
            WeaponFovFSM();
            cameraSway.SwayUpdate();
        
    }

    void FovFSM()
    {
        if (!GameController.GetGameController().GetSettingsData().FovChanges) //If doesnt want fov changes in movement
        {
            if (RifleFOVEnabled)
            {
                targetFOV = normalFOV * GameController.GetGameController().GetSettingsData().CameraFOVMultiplier * shootRifleFOV;
                fovSmoothness = normalSmoothFOVChange;
            }
            else
            {
                targetFOV = normalFOV * GameController.GetGameController().GetSettingsData().CameraFOVMultiplier;
                fovSmoothness = normalSmoothFOVChange;
            }
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fovSmoothness * Time.deltaTime);
            return;
        }


        if (RifleFOVEnabled)
        {
            targetFOV = normalFOV * GameController.GetGameController().GetSettingsData().CameraFOVMultiplier * shootRifleFOV;
            fovSmoothness = normalSmoothFOVChange;
        }
        else if (HookFOVEnabled)
        {
            fovSmoothness = hookSmoothFOVChange;
            targetFOV = normalFOV * GameController.GetGameController().GetSettingsData().CameraFOVMultiplier * hookFOV;
        }
        else if (DashFOVEnabled)
        {
            fovSmoothness = dashSmoothFOVChange;
            targetFOV = normalFOV * GameController.GetGameController().GetSettingsData().CameraFOVMultiplier * dashFOV;
        }
        else
        {
            targetFOV = normalFOV * GameController.GetGameController().GetSettingsData().CameraFOVMultiplier;
            fovSmoothness = normalSmoothFOVChange;
        }

        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fovSmoothness * Time.deltaTime);
    }

    void WeaponFovFSM()
    {
        if (weaponCamera.fieldOfView == wNormalFOV && !RifleFOVEnabled) return;

        float targetFOV = RifleFOVEnabled ? wNormalFOV * weaponCamShootRifleFOV : wNormalFOV;

        weaponCamera.fieldOfView = Mathf.Lerp(weaponCamera.fieldOfView, targetFOV, weaponCamFOVSmoothness * Time.deltaTime);
    }


    private void UpdateCameraInputDebug()
    {
        if (Input.GetKeyDown(m_DebugLockAngleKeyCode))
            m_AngleLocked = !m_AngleLocked;
        if (Input.GetKeyDown(m_DebugLockKeyCode))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
            m_AimLocked = Cursor.lockState == CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(m_DebugPauseKeyCode))
        {
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        }
    }

    public bool IsLookingDown()
    {
        Vector3 cameraForward = playerCamera.transform.forward; // Get the forward direction of the camera
        Vector3 downVector = -Vector3.up; // Define the down vector
        float angle = Vector3.Angle(cameraForward, downVector); // Calculate the angle between the camera forward vector and the down vector
        return angle <= downAngle; // Return true if the angle is less than or equal to the specified down angle
    }
}

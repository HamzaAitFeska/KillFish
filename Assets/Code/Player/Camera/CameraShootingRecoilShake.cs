using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShootingRecoilShake : MonoBehaviour
{
    Vector3 m_CurrentRotation;
    Vector3 m_TargetRotation;

    [Header("Movement Values")]
    [SerializeField] float m_Snapiness;
    [SerializeField] float m_ReturnSpeed;

    [Header("Recoils for weapons")]
    [SerializeField] Vector3 shotgunRecoil = new Vector3(-10, 0, 0);
    public Vector3 ShotgunRecoil { get { return shotgunRecoil; } }

    [SerializeField] Vector3 minRifleRecoil;
    public Vector3 MinRifleRecoil { get { return minRifleRecoil; } }
    [SerializeField] Vector3 maxRifleRecoil;
    public Vector3 MaxRifleRecoil { get { return maxRifleRecoil; } }
    Vector3 rifleRecoil;
    public Vector3 RifleRecoil { get { return rifleRecoil; } set { rifleRecoil = value; } }

    [Header("Shakes for weapons")]
    [SerializeField] CameraShakeInstanceVariables shotgunCameraShake;
    public CameraShakeInstanceVariables ShotgunCameraShake { get { return shotgunCameraShake; } }

    [SerializeField] CameraShakeInstanceVariables minRifleShake;
    public CameraShakeInstanceVariables MinRifleShake { get { return minRifleShake; } }
    [SerializeField] CameraShakeInstanceVariables maxRifleShake;
    public CameraShakeInstanceVariables MaxRifleShake { get { return maxRifleShake; } }
    CameraShakeInstanceVariables rifleShake;
    public CameraShakeInstanceVariables RifleShake { get { return rifleShake; } set { RifleShake = value; } }

    [Header("Move camera weapon")]
    [SerializeField] Transform weaponTransform;
    [SerializeField] bool moveCameraWeaponByCodeRecoil = false;
    [SerializeField] float recoilCameraWeaponMultiplier = 1.5f;

    void Update()
    {
        //always trying to get back to the center
        m_TargetRotation = Vector3.Lerp(m_TargetRotation, Vector3.zero, m_ReturnSpeed * Time.deltaTime);
        m_CurrentRotation = Vector3.Slerp(m_CurrentRotation, m_TargetRotation, m_Snapiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(m_CurrentRotation);
        if (moveCameraWeaponByCodeRecoil) weaponTransform.localRotation = Quaternion.Euler(-m_CurrentRotation * recoilCameraWeaponMultiplier);

    }
    public void RecoilFire(Vector3 recoil)
    {
        //sets instant offset every shot
        m_TargetRotation += recoil;

    }
}

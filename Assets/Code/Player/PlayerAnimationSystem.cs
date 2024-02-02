using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationSystem : MonoBehaviour
{
    PlayerVariableContainer playerVariableContainer;
    [Header("AnimationClips for checking whether is the current info or not (only using the name)")]
    [SerializeField] AnimationClip AirAnimation;
    [SerializeField] AnimationClip IdleAnimation;
    [SerializeField] AnimationClip RunAnimation;
    [SerializeField] AnimationClip GunTransform;
    [SerializeField] AnimationClip GunRifleIdle;


    [SerializeField] GameObject normalScope;
    [SerializeField] GameObject sniperScope;
    private void Start()
    {
        playerVariableContainer = GameController.GetGameController().GetPlayer();
    }

    [SerializeField] Animator weaponAnimation;

    public void GameStarts()
    {
        weaponAnimation.SetTrigger("GameStart");
    }
    public void StartsSniperMode()
    {
        weaponAnimation.SetBool("Aiming", true);
    }
    public void StopsSniperMode()
    {
        weaponAnimation.SetBool("Aiming", false);
    }
    public void ShootRechargeShotgun()
    {
        weaponAnimation.SetTrigger("ShootRechargeShotgun");
    }
    public void ShootShotgun()
    {
        weaponAnimation.SetTrigger("ShootShotgun");
    }
    public void ShootRifle()
    {
        weaponAnimation.SetTrigger("ShootRifle");
    }
    public void HookThrow()
    {
        weaponAnimation.SetTrigger("HookThrow");
    }
    public void HookEnd()
    {
       weaponAnimation.SetTrigger("HookEnd");
    }
    public void Jump()
    {
        string currentAnim = weaponAnimation.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (currentAnim != AirAnimation.name && currentAnim != IdleAnimation.name && currentAnim != RunAnimation.name) return;
        weaponAnimation.SetTrigger("Jump");
    }
    public void Land()
    {
        string currentAnim = weaponAnimation.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        if (currentAnim != AirAnimation.name) return;
        weaponAnimation.SetTrigger("Land");
    }
    void CantShoot()
    {
        playerVariableContainer.WeaponController.ShootingAnimation = true;
    }
    void CanShoot()
    {
        playerVariableContainer.WeaponController.ShootingAnimation = false;
    }
    public void TransformingWeaponProcess()
    {
        playerVariableContainer.WeaponController.TransformingWeapon = true;
    }
    public void StoppedTransformationProcess()
    {
        playerVariableContainer.WeaponController.TransformingWeapon = false;
    }
   private void Update()
   {

        float playerSpeed = Mathf.Abs(playerVariableContainer.MovementController.FPSMoveController.CurrentSpeedXAxis()) + Mathf.Abs(playerVariableContainer.MovementController.FPSMoveController.CurrentSpeedYAxis());
        if (!GameController.GetGameController().GetPlayer().CanMove()) playerSpeed = 0;

        weaponAnimation.SetFloat("PlayerSpeed", playerSpeed);

        weaponAnimation.SetBool("OnAir", !playerVariableContainer.MovementController.FPSMoveController.IsCoyoteGrounded());


        string currentAnim = weaponAnimation.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        if(currentAnim != GunTransform.name && currentAnim != GunRifleIdle.name && playerVariableContainer.CameraController.RifleFOVEnabled) { StopFOVRifle(); }
    }

   void StartFOVRifle(float FOVMultiplier)
    {
        playerVariableContainer.CameraController.SetShootRifleFOV(FOVMultiplier);
        playerVariableContainer.CameraController.RifleFOVEnabled = true;
        playerVariableContainer.PlayerParticlesSystem.EnableSniperChargeParticles();
    }
    void StartsWeaponCamFOVRifle(float weaponCamFOVMultiplier)
    {
        playerVariableContainer.CameraController.SetWeaponCamShootRifleFOV(weaponCamFOVMultiplier);
    }
    void StopFOVRifle()
    {
        playerVariableContainer.CameraController.RifleFOVEnabled = false;
        playerVariableContainer.PlayerParticlesSystem.DisableSniperChargeParticles();

    }

    void StartSniperScope()
    {
        normalScope.SetActive(false);
        sniperScope.SetActive(true);
    }
    void StopSniperScope()
    {
        normalScope.SetActive(true);
        sniperScope.SetActive(false);
    }

    void CanMoveGunInspect()
    {
        playerVariableContainer.SetCanMove(true);
    }

}

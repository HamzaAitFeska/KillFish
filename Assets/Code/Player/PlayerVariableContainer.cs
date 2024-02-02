using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerVariableContainer : MonoBehaviour
{
    [Tooltip("Can Move will able Player to read input or freeze it")]
    [SerializeField] private bool canMove;
    public bool CanMove() { return canMove; }
    public void SetCanMove(bool l_canMove) { canMove = l_canMove; }

    #region Player Key Scripts
    [Header("Player's key scripts")]
    [SerializeField] CharacterController characterController;
    public CharacterController CharacterController { get { return characterController; } }

    [SerializeField] ActionManager actionManager;
    public ActionManager ActionManager { get { return actionManager; } }

    [SerializeField] CameraController cameraController;
    public CameraController CameraController { get { return cameraController; } }

    [SerializeField] MovementController movementController;
    public MovementController MovementController { get { return movementController; } }

    [SerializeField] WeaponController weaponController;
    public WeaponController WeaponController { get { return weaponController; } }

    [SerializeField] PlayerHealth healthSystem;
    public PlayerHealth HealthSystem { get { return healthSystem; } }

    [SerializeField] PlayerStyleSystem styleSystem;
    public PlayerStyleSystem StyleSystem { get { return styleSystem; } }

    [SerializeField] PlayerAnimationSystem playerAnimationSystem;
    public PlayerAnimationSystem PlayerAnimationSystem { get { return playerAnimationSystem; } }

    [SerializeField] PlayerParticles playerParticles;
    public PlayerParticles PlayerParticlesSystem { get { return playerParticles; } }

    [SerializeField] TutorialScript tutorialScript;
    public TutorialScript PlayerTutorialScript { get { return tutorialScript; } }
    [SerializeField] PlayerUIVariableContainer playerUIVariableContainer;
    public PlayerUIVariableContainer PlayersUIVariableContainer { get { return playerUIVariableContainer; } }
    
    #endregion

    private void Awake()//Sets the player to the GameController
    {
        GameController.GetGameController().SetPlayer(this);
    }
  
    void Update() // Updates Player Dependencies such as the Input of the movement and the Shooting class.
    {
        if (!CanMove()) return;
        weaponController.ShootingUpdate();
        movementController.InputUpdate();
        movementController.MovementTimersUpdate();

    }
    
    private void FixedUpdate() // Updates Player Dependencies with physics such as the Movement Update
    {
        movementController.MovementUpdate();
    }
    
    void LateUpdate() //Updates Player Dependencies such as the Camera
    {
        if (!CanMove()) return;
        cameraController.CameraUpdate();
    }

    #region Variables can be used for information such as: IsInGround, IsUsingHook, IsAiming, IsRunning, IsDashing...
    public bool IsInGround() { return movementController.FPSMoveController.IsCoyoteGrounded(); }
    public bool IsUsingHook() { return movementController.GrapplingController.IsUsingHook(); }
    public bool IsMovingWithTheHook() { return movementController.GrapplingController.IsMovingWithTheHook(); }
    public bool IsAiming() { return weaponController.IsAiming(); }
    public bool IsRunning() { return movementController.FPSMoveController.IsRunning();}
    public bool IsDashing() { return movementController.FPSMoveController.IsDashing(); }
    #endregion
}

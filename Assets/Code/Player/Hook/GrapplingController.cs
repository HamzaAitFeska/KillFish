using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public enum HookShotType{ hooked, enemyHooked, cancelled, hitOnSurface, noHit }
public class GrapplingController : MonoBehaviour
{
    #region Hook Variables
    
    PlayerVariableContainer playerVariableContainer;

    #region Using Hook State
    //Starts at the beginning and stops when starts to go back
    bool isUsingHook = false;
    public bool IsUsingHook() { return isUsingHook; }
    bool isHookReturning;

    public bool IsMovingWithTheHook() { return lastHookShot == HookShotType.hooked || lastHookShot == HookShotType.enemyHooked; }
    #endregion

    #region Transforms
    [Header("Transforms")]
    [Tooltip("Parent of Point of the hookshot, use for resetting the transform point after a use")]
    [SerializeField] Transform hookShotTransformPointParent;
    [Tooltip("The Point where the hookshot goes")]
    [SerializeField] Transform hookshotTransformPoint;
    [Tooltip("The parent of the Hook GameObject")]
    [SerializeField] Transform hookshotTransformParent;
    [Tooltip("It is setted at the Start, it will be the Hook Gameobject")]
    [SerializeField] Transform hookshotTransform;
    public Transform HookshotTransformParent() { return hookshotTransformParent; } 
    public void SetHookShotTransform(Transform hookshot) { hookshotTransform = hookshot; }
    #endregion

    #region Chain Related Information
    [Header("Chain Related")]
    [Tooltip("Line Renderer that shows up as the chain")]
    [SerializeField] LineRenderer chain;
    [Tooltip("The object where the last piece of chain is, the end position of the Line Renderer")]
    [SerializeField] GameObject chainObj;
    public void SetChainObj(GameObject obj) { chainObj = obj; } //This is for setting it in the start
    #endregion

    #region Current/Last HookShoot information
    //The last hook point player was hooked
    IHookPoint currentHookPoint;
    //The last GameObject player was hooked
    GameObject lastHookedGameObject;
    //The last LayerMask player was hooked
    LayerMask lastHookedObjectMask;
    //The end of the situation of the hookshot
    HookShotType lastHookShot;
    #endregion

    #region HookShot Throw Variables
    [Header("Hook Shot Throw variables")]
    [Tooltip("The speed of the chain moving in the throw")]
    [SerializeField] float hookshotThrowSpeed;
    [Tooltip("The maximum distance hook can reach")]
    [SerializeField] float maxDistanceHookThrow;
    [Tooltip("The distance to hook the hookTransform into the hookPoint")]
    [SerializeField] float distanceToHookTransformIntoHookPoint = 1f;
    #endregion

    #region LayerMask Information related
    [Header("LayerMask Information related")]
    [Tooltip("The Layer Masks player can hook into; Should always be something related to Hookable Surface")]
    [SerializeField] List<LayerMask> LayerMasksCanHook;
    [Tooltip("So it does not take into accountant some -invisible- layers, such as the player")]
    [SerializeField] LayerMask masksToIgnore;
    [Tooltip("Have the information available for the enemy hook kill for style")]
    [SerializeField] LayerMask enemyMask;
    public void AddLayerCanHook(LayerMask newlayerMask) { //Add the layer so player can hook into this layer. Example: EnemieHookableSurface
        if(LayerMasksCanHook.Contains(newlayerMask)) return; //If it already contains the Layer, return
        LayerMasksCanHook.Add(newlayerMask); //Add the layer
    }
    #endregion

    #region HookShot Return Variables
    [Header("HookShot Return variables")]
    [Tooltip("The fast it lerps back to the weapon point")]
    [SerializeField] float smoothnessReturnHook;
    [Tooltip("At this distance, it sets into the position in one frame")]
    [SerializeField] float distanceToAttachHookAgain = 3.0f;
    [Tooltip("At this distance, Player will unhook to the Hookable Surface")]
    [SerializeField] float distanceToUnhookNormal = 1;
    [Tooltip("At this distance, Player will unhook to the Enemie")]
    [SerializeField] float distanceToUnhookEnemy = 5;
    [Tooltip("The distance in which it unhooks, it updates if it's into a normal Hookable Surface or an enemie")]
    [SerializeField] float reachedHookShotPositionDistance;
    #endregion

    #region Hook Movement Related Variables
    [Header("Hook Movement Related Variables")]
    [Tooltip("Multiplies the speed at which player moves while hooking")]
    [SerializeField] float hookMoveSpeedMultiplier;
    [Tooltip("The speed Up Vector on pressing Jump")]
    [SerializeField] float speedOnJump;
    [Tooltip("The speed Up Vector on touching a surface if the hook point has jump info")]
    [SerializeField] float speedOnJumpAtTouchingSurface;
    [Tooltip("The speed Forward Vector on touching a surface")]
    [SerializeField] float speedOnForwardAtTouchingSurface;
    #endregion

    #region Hook Momentum Related Variables 
    [Header("Hook Momentum Related")]
    [Tooltip("The speed multiplied on the hook momentum")]
    [SerializeField] float momentumSpeedMultiplier;
    [Tooltip("The speed multiplied of the momentum when the hooked object is an enemy")]
    [SerializeField] float momentumSpeedDividerForEnemyHooks = 1.5f;
    [Tooltip("The direction of the movement while hooking, it is updated")]
    Vector3 hookShotDir;
    [Tooltip("The speed by the distance to the point")]
    float hookShotMoveSpeedByDistance;
    [Tooltip("The minimum clamped Speed")]
    [SerializeField] float minHookMoveSpeed;
    [Tooltip("The maximum clamped Speed")]
    [SerializeField] float maxHookMoveSpeed;
    #endregion

    #region Signifiers (UI Hook Crosshair, CameraShakes, sounds)
    //For User Interface Hook Crosshair
    bool isAbleToShootHook;
    public bool IsAbleToShootHook() { return isAbleToShootHook; }

    [Header("CameraShakes")]
    [SerializeField] CameraShakeInstanceVariables camShakeOnHookFailReturn;
    [SerializeField] CameraShakeInstanceVariables camShakeOnHookCancellingInput;

    EventInstance hookLoopSound;
    #endregion

    #endregion

    #region Start, OnEnable and OnDisable
    private void Start() 
    {
        playerVariableContainer = GameController.GetGameController().GetPlayer();
        lastHookShot = HookShotType.noHit;

        //Creates the instance of the audio loop
        hookLoopSound = AudioManager.GetAudioManager().CreateEventInstance(AudioManager.GetAudioManager().Events.hookReleased);

        //Add layer can hook enemies if doesnt have and should have
        if (GameController.GetGameController().GetUpgradeManagerData().HasHookToEnemies) AddLayerCanHook(LayerMask.GetMask("EnemieHookableSurface"));
    }

    #endregion

    private void Update() // Updates if player is able to hook to a surface so it can be desplayed in the UI
    {
        isAbleToShootHook = false;
        if (isHookReturning) return; 

        if (Physics.Raycast(playerVariableContainer.CameraController.playerCamera.transform.position, //Shoot raycast
           playerVariableContainer.CameraController.playerCamera.transform.forward, 
           out RaycastHit hit, maxDistanceHookThrow, ~masksToIgnore))
        {
            foreach (LayerMask mask in LayerMasksCanHook)
            {
                // Check if the hit object's layer is included in the current LayerMask player can hook to
                if (mask == (mask | (1 << hit.collider.gameObject.layer)))
                {
                    isAbleToShootHook = true;  //Can shoot hook again
                }
            }
        }

    }

    void ResetVerticalVelocity() // Resets the vertical velocity so is 0
    {
        playerVariableContainer.MovementController.FPSMoveController.ResetCharacterVelocityY();
        playerVariableContainer.MovementController.CharacterVelocity.y = 0;
    }

    #region HookshotStart, SetHookPoint
    public void HookshotStart() //When hook thrown can start
    {
        if (isUsingHook || isHookReturning) return;

        bool isHookableSurface = false;

        if (playerVariableContainer.ActionManager.HookKeyStart())
        {
            if (Physics.Raycast(playerVariableContainer.CameraController.playerCamera.transform.position,  // If raycasts collides
                playerVariableContainer.CameraController.playerCamera.transform.forward,
                out RaycastHit hit, maxDistanceHookThrow, ~masksToIgnore))
            {
                foreach (LayerMask mask in LayerMasksCanHook)
                {
                    // Check if the hit object's layer is included in the current LayerMask player can hook
                    if (mask == (mask | (1 << hit.collider.gameObject.layer)))// The hit object's layer is included in the current LayerMask can hook
                    {
                        SetHookPoint(hit); //Sets info about the point hook will go
                        isUsingHook = true;  //Set is using hook
                        isHookableSurface = true; 
                        lastHookedGameObject = hit.collider.gameObject; //Sets that this is the lastHookedGameObject
                        hookshotTransform.SetParent(null); //Sets null the transform of the gameobject, so it will move freely without being attached

                        //Set last hooked object mask
                        lastHookedObjectMask = hit.collider.gameObject.layer;
                        //And the type of last hook shoot depending on the last hooked object mask
                        lastHookShot = ((enemyMask.value & (1 << lastHookedObjectMask)) != 0) ? HookShotType.enemyHooked : HookShotType.hooked;

                        // Set the player state to HookshotThrown
                        SetHookThrown();

                        // Exit the foreach loop since we found a matching LayerMask
                        break;
                    }
                }
                if (!isHookableSurface) // IS NOT A HOOKABLE SURFACE BUT IT'S SOMETHING (wall)
                {
                    SetHookPoint(hit.point + playerVariableContainer.CameraController.playerCamera.transform.forward);
                    SetHookThrown();
                    lastHookShot = HookShotType.hitOnSurface;
                }

            }
            else // IF DOESN'T COLLISION WITH ANYTHING
            {
                Vector3 position = playerVariableContainer.CameraController.playerCamera.transform.position + playerVariableContainer.CameraController.playerCamera.transform.forward * maxDistanceHookThrow;
                SetHookPoint(position);
                SetHookThrown();
                lastHookShot = HookShotType.noHit;
            }

        }
    }
    
    void SetHookPoint(RaycastHit hit) // Sets the hook point to where it has attached, from an IHookTrigger, searching for the nearest IHookPoint
    {
        IHookTrigger hookTrigger = hit.collider.gameObject.GetComponent<IHookTrigger>();

        if (hookTrigger != null)
        {
            Transform point = hookTrigger.GetNearestPoint(hit.point); //Gets nearest point
            if (point == null) Debug.LogWarning("IHookTrigger no tiene points o sus points no tienen el componente IHookPoint");
            hookshotTransformPoint.position = point.position; //Sets the position
            hookshotTransformPoint.SetParent(point); //Then sets its parent, so, if it is a dynamic object, it will update the global point position
            currentHookPoint = point.GetComponent<IHookPoint>(); //Updates the current hook point 
        }
        else
        {
            Debug.LogWarning("El HookableSurface al que te has enganchado no tiene el componente IHookTrigger en sus triggers");
        }
    }
    
    void SetHookPoint(Vector3 position) //Sets the hook point to an imaginary place, because raycast has not collided with an IHookTrigger
    {
        hookshotTransformPoint.SetParent(null); //Need to do so it starts always straight
        hookshotTransformPoint.position = position;
        currentHookPoint = null;
        isUsingHook = true;
        Invoke("ParentBack", Time.deltaTime*2); //In this case, feels better if hook goes and returns straight
    }
    void ParentBack()
    {
        hookshotTransformPoint.SetParent(hookshotTransformParent);
    }
    #endregion

    #region HookThrown Related
    void SetHookThrown() //When Hook shot is set to be thrown
    {
        //Logic
        chain.gameObject.layer = LayerMask.NameToLayer("HookWeapon"); //Sets layer so its not in the weapon camera
        chainObj.layer = LayerMask.NameToLayer("HookWeapon");
        hookshotTransform.gameObject.layer = LayerMask.NameToLayer("HookWeapon");
        playerVariableContainer.MovementController.SetPlayerState(MovementController.MovementState.HookshotThrown); //Changes state as it starts the hook thrown

        //Sound
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.hookShot, transform.position);
        hookLoopSound.start();

        //Particles
        playerVariableContainer.PlayerParticlesSystem.HookShotParticles(); // Sets particles
        playerVariableContainer.PlayerAnimationSystem.HookThrow(); // Sets animation

    }

    public void HookshotThrowUpdate() //Update hook going to the IHookPoinst
    {
        if (!GameController.GetGameController().GetPlayer().CanMove()) CancelHookThrow();

        //Updates size and rotation while throwing hookshot
        HookshotThrowUpdatePosition();

        //Set particles
        playerVariableContainer.PlayerParticlesSystem.StartHookLoopParticles();

        //Starting the movement when attached to a surface
        if (Vector3.Distance(hookshotTransform.position, hookshotTransformPoint.position) < distanceToHookTransformIntoHookPoint)
        {
           if(currentHookPoint) StartHookMovement(); // If player has hooked into a hokable surface
           else { CancelHookThrow(); } // If has hooked into nothing
        }
    }
    
    void HookshotThrowUpdatePosition() //Updates the position of the hook going to the IHookPoint 
    {
        Vector3 dir = (hookshotTransformPoint.position - hookshotTransform.position).normalized;
        hookshotTransform.position += hookshotThrowSpeed * Time.deltaTime * dir;
    }

    public void HookshotThrowInput() //For if cancels the hookshot while yet unhooked
    {
        //Canceling Hookshot
        if (playerVariableContainer.ActionManager.HookKeyStart())
        {
            CancelHookThrow();
            lastHookShot = HookShotType.cancelled;
        }
    }
    #endregion

    #region Hook Movement (Input, Start and Update)
    public void HookshotMovementInput() //For if cancels the hookshot while Hooked (INPUT)
    {
        if (playerVariableContainer.ActionManager.Jump()) //Cancelling with jump
        {  
            BackToNormalState();
            ApplyMomentum(hookShotDir, hookShotMoveSpeedByDistance);
            ApplyJumpMomentum(speedOnJump);
        }
        if (playerVariableContainer.ActionManager.HookKeyStart()) //Cancelling input
        {
            BackToNormalState();
            ApplyMomentum(hookShotDir, hookShotMoveSpeedByDistance);
        }
    }

    public void HookStoppedByShooting() //For if cancels the hookshot by shooting (INPUT (in WeaponController))
    {
        BackToNormalState();
        ApplyMomentum(hookShotDir, hookShotMoveSpeedByDistance);
    }

    void StartHookMovement() //Start method for the hookshot flying movement state
    {
        playerVariableContainer.MovementController.SetPlayerState(MovementController.MovementState.HookshotFlying);

        reachedHookShotPositionDistance = ((enemyMask.value & (1 << lastHookedObjectMask)) != 0) ? distanceToUnhookEnemy : distanceToUnhookNormal;
        playerVariableContainer.CameraController.HookFOVEnabled = true;

        //If attaches to an enemy, stun him
        if(((enemyMask.value & (1 << lastHookedObjectMask)) != 0))
        {
            if(lastHookedGameObject != null) lastHookedGameObject.GetComponentInParent<IEnemyAI>().SetStunState();
        }

        //Reset jump count
        playerVariableContainer.MovementController.FPSMoveController.SetJumpCount(0);
        //Unset particles
        playerVariableContainer.PlayerParticlesSystem.StartMovementParticles();
    }

    public void HookshotMovementUpdate() //Updates the movement while being hooked
    {
        if (!GameController.GetGameController().GetPlayer().CanMove()) BackToNormalState();
        hookShotDir = (hookshotTransformPoint.position - transform.position).normalized;

        hookShotMoveSpeedByDistance = Mathf.Clamp(Vector3.Distance(transform.position, hookshotTransformPoint.position), minHookMoveSpeed, maxHookMoveSpeed);

        playerVariableContainer.CharacterController.Move(hookShotDir * hookShotMoveSpeedByDistance * hookMoveSpeedMultiplier * Time.deltaTime);

        if (Vector3.Distance(transform.position, hookshotTransformPoint.position) < reachedHookShotPositionDistance) //If it has arruved to the hookpoint
        {
            //Reached hookshot Position
            BackToNormalState();
            //Doesnt apply physical momentum as it should be all-time controlled to go upon a surface
            if (currentHookPoint.ApplyJumpForce()) ApplyJumpMomentum(speedOnJumpAtTouchingSurface);//Going a bit up when about to touch the surface
            if(!(((enemyMask.value & (1 << lastHookedObjectMask)) != 0))) ApplyForwardMomentum(speedOnForwardAtTouchingSurface); //pushing player a bit forward if it's not a enemy the last hooked go
        }
        if ((playerVariableContainer.CharacterController.collisionFlags & CollisionFlags.Sides) != 0 || (playerVariableContainer.CharacterController.collisionFlags & CollisionFlags.Above) != 0) //If Collides
        {
            BackToNormalState();
        }
    }
    #endregion

    #region Momentum and Physics related
    void ApplyMomentum(Vector3 hookShotDir, float hookShotMoveSpeedByDistance) //When we want physics do their job
    {
        if (((enemyMask.value & (1 << lastHookedObjectMask)) != 0)) hookShotMoveSpeedByDistance /= momentumSpeedDividerForEnemyHooks; //If the last hooked gameobject is an enemy, way less momentum
        
        playerVariableContainer.MovementController.CharacterVelocityMomentum = hookShotDir * hookShotMoveSpeedByDistance * momentumSpeedMultiplier;
    }
    
    void ApplyJumpMomentum(float upForce) //When we want to force player go up
    {
            playerVariableContainer.MovementController.CharacterVelocityMomentum.y = 0;
            playerVariableContainer.MovementController.FPSMoveController.SetCharacterVelocityY(upForce);
    }
   
    void ApplyForwardMomentum(float forwardForce) //When we want to force player go forward
    {
        playerVariableContainer.MovementController.CharacterVelocityMomentum += (playerVariableContainer.transform.forward * forwardForce);
    }
    #endregion

    #region Going Back to Normal State and Resetting Hook
    void CancelHookThrow() //If stops through input
    {
        playerVariableContainer.MovementController.SetPlayerState(MovementController.MovementState.Normal);
        lastHookedGameObject = null;
        StartCoroutine(HookGoBack());
    }
    
    void BackToNormalState() //When wants to go back to normal state
    {
        //Can Hook Kill points style meter
        EnableHookKill();

        //Goes to normal state
        playerVariableContainer.MovementController.SetPlayerState(MovementController.MovementState.Normal);

        //Resets velocity and resets hook values
        ResetVerticalVelocity();
        playerVariableContainer.CameraController.HookFOVEnabled = false;
        //Unset particles
        playerVariableContainer.PlayerParticlesSystem.StopMovementParticles();
        //Set jump count
        playerVariableContainer.MovementController.FPSMoveController.SetJumpCount(1);
        //make hook rego to player
        StartCoroutine(HookGoBack());
    }

    void EnableHookKill() //For the style meter
    {
        if ((enemyMask.value & (1 << lastHookedObjectMask)) != 0) playerVariableContainer.StyleSystem.EnableHookKills();
        else
        {
            playerVariableContainer.StyleSystem.EnableAirHookKills();
        }
    }
    
    IEnumerator HookGoBack() //Updates the movement so the hook goes back to his normal place
    {
        isUsingHook = false; //It is not using the hook anymore
        isHookReturning = true; //But yes it is returning to it's place

        //Resets info on the point
        hookshotTransformPoint.SetParent(hookShotTransformPointParent); 
        hookshotTransformPoint.gameObject.SetActive(true);
        hookshotTransformPoint.localPosition = Vector3.zero;

        //while hook is far, go back to it's place
        while (Vector3.Distance(hookshotTransformPoint.position, hookshotTransform.position) > distanceToAttachHookAgain) {
            hookshotTransform.position = Vector3.Lerp(hookshotTransform.position, hookshotTransformPoint.position, smoothnessReturnHook * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        ResetHook(); //Final Reset
    }

    void ResetHook() //Final Reset of the hook
    {
        isUsingHook = false; //It is not using hook anymore
        isHookReturning = false; //It is not returning anymore, so it can start hook again

        //Resets the transform of the hook and hookpoint
        hookshotTransform.SetParent(hookshotTransformParent);
        hookshotTransform.localPosition = Vector3.zero;
        hookshotTransform.localEulerAngles = Vector3.zero;
        hookshotTransformPoint.SetParent(hookShotTransformPointParent);

        //Rests the Layers so it's again in the weapon camera
        chain.gameObject.layer = LayerMask.NameToLayer("Weapon");
        hookshotTransform.gameObject.layer = LayerMask.NameToLayer("Weapon"); 
        chainObj.layer = LayerMask.NameToLayer("Weapon");

        //Camera Shake
        if (lastHookShot == HookShotType.hitOnSurface || lastHookShot == HookShotType.noHit) { //If has not hooked into a hookable surface
            GameController.GetGameController().GetPlayer().CameraController.GetCameraShaker().Shake(camShakeOnHookFailReturn);
        }
        if (lastHookShot == HookShotType.cancelled) { //If hookshot has been cancelled
            GameController.GetGameController().GetPlayer().CameraController.GetCameraShaker().Shake(camShakeOnHookCancellingInput);
        }


        //Particle
        playerVariableContainer.PlayerParticlesSystem.StopHookLoopParticles();

        //HOOK RETURN SOUND
        AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.hookReturned, transform.position);
        hookLoopSound.stop(STOP_MODE.ALLOWFADEOUT);

        //Sets animation
        playerVariableContainer.PlayerAnimationSystem.HookEnd();


        //If attached to an enemy, unstun him
        if (((enemyMask.value & (1 << lastHookedObjectMask)) != 0))
        {
            if(lastHookedGameObject != null) lastHookedGameObject.GetComponentInParent<IEnemyAI>().UnsetStunState();
        }
    }
    #endregion
}

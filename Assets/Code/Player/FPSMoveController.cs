using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using FMODUnity;

[RequireComponent(typeof(CharacterController))]
public class FPSMoveController : MonoBehaviour
{
    PlayerVariableContainer playerVariableContainer;

    #region First Person Related Movement
    [Header("First Person Related Movement")]
    //Speeds, walk and run
    [SerializeField] float walkingSpeed = 7.5f;
    [SerializeField] float runningSpeed = 11.5f;
    [SerializeField] bool WantsAccelerationDeceleration;
    [SerializeField] float LerpSpeedAccelerationDeceleration;
    bool isRunning;
    //Input
    [SerializeField] Vector2 movementDirectionInput;
    public Vector3 MovementDirectionInput() { return movementDirectionInput; }

    float curSpeedX;
    float curSpeedY;
    public float CurrentSpeedXAxis() { return curSpeedX; }
    public float CurrentSpeedYAxis() { return curSpeedY; }
    float lastCurSpeedX;  //Stores last speed of Axis
    float lastCurSpeedY;  //Stores last speed of Axis
    public bool IsRunning() { return isRunning; }
    #endregion

    #region Physics Related
    [Header("Physics Related")]
    [Tooltip("It increments gravity when speed is lower than this number")]
    [SerializeField] float gravityNecesaryToAcceleration = -2f;

    [Tooltip("The acceleration added to the gravity")]
    [SerializeField] float gravityAcceleration = 1.01f;

    [Tooltip("Multiplies the component of gravity in certain situations")]
    [SerializeField] float gravitysMultiplier = 2f;

    [Tooltip("Clamps to the maximum fall speed")]
    [SerializeField] float maxDownSpeed = -80f;

    [Tooltip("Divides the speed created on momentum activities such as hook inertia or shotgunned down inertia, On Higher, less inertia")]
    [SerializeField] float momentumDividorSpeed = 3f;
    
    float characterVelocityY;//This is a variable that counts as the Y axis. Involved in gravity and forces like jump.
    public void SetCharacterVelocityY(float l_VelocityY) { characterVelocityY = l_VelocityY; }
    public void ResetCharacterVelocityY(){ characterVelocityY = 0; }

    //On Air Speed reducing to control air movement variables
    [Header("On Air Speed physics")]
    [Tooltip("The minimum speed momentum can reduce by multiplying (On Air speed reducer)")]
    [SerializeField][Range(0.2f, 0.8f)] float onAirMomentumMinSpeed;
    [Tooltip("The lerp smoothness value between change of air momentum from on ground to on air")]
    [SerializeField][Range(0.01f, 0.2f)] float lerpSpeedBetweenOnAirMomentum;
    [Tooltip("The lerp smoothness value between change of directions on air")]
    [SerializeField][Range(1f, 10f)] float lerpSpeedChangeDirectionBetweenOnAirMomentum;
    float onAirMomentumSpeedReducer; //The updated variable of the Momentum Speed Reducer of being on air
    const float onAirMomentumMaxSpeed = 1f; //Max speed should be 1 so it represents no resistance on ground
    #endregion

    #region Game Features Movement Related (Dash, Stomp, ShotgunDown, Sniper...)
    
    [Header("Stomp")]
    [Tooltip("The Speed of going down while stomping")]
    [SerializeField] float stompSpeed;

    [Tooltip("The Distance enemies should be to get stomped")]
    [SerializeField] float StompDistance;

    bool isStomping; //Updates if player is stomping

    [Header("Dash")]
    [Tooltip("The speed of the dash")]
    [SerializeField] float dashSpeed;

    float dashStartTime;
    [Tooltip("The time a dash lasts")]
    [SerializeField] float dashTime;

    [Tooltip("The time for fully charging a new dash")] 
    [SerializeField] float dashCooldownTimer = 1.0f;

    float currentDashCooldownTimer; //Updates timer, sets to 1.0 when using dash and decreases, when 0, new dash use is charged
    bool isDashing; //Updates if player is dashing
    float currentDashCount; // The current dashes player has available to use
    Vector3 currentDashDirection; //Updates when starts a dash with the direction player is inputting

    public static Action OnDashed;
    //Public references
    public bool IsDashInCooldown() { return currentDashCooldownTimer > 0; }
    public float GetDashCoolDown() { return dashCooldownTimer; }
    public float GetDashCurrentCoolDown() { return currentDashCooldownTimer;}
    public int MaxDashCount() { return GameController.GetGameController().GetUpgradeManagerData().HasDoubleDashAbility ? 2:1; } //The maximum dashes player can have charged
    public float GetCurrentDashCount() { return currentDashCount; }
    public void SetCurrentDashCount(float value) { currentDashCount = value; }
    public bool IsDashing() { return isDashing; }
    

    [Header("ShotgunnedDownMovement")]
    [Tooltip("The velocity when shotgunnes down (0 = resetting Y speed)")]
    [SerializeField] float ShotgunMomentumSpeedMultiplier; 

    Vector3 shotgunnedDownForce; //The inverse vector of the forward of the camera when shooting down (so player goes "back")

    [Header("Rifle Related Movement")]
    [Tooltip("The multiplier of the speed when player is aiming with the sniper")]
    [SerializeField][Range(0,1)] float speedMultiplierOnRifle = 0.6f;
    #endregion

    #region Jump and Landing Related
    bool onGround = true; //Variable on ground setted through the Character Controller
    bool wasCoyoteGrounded; //Variable for checking if it was grounded last frame
    float timeOnAir = 0f; //Variable time on air to calculate a if it's on ground (for coyote jumps)
    public bool IsCoyoteGrounded() { return timeOnAir < timeCoyote; } // Variable for checking if it detects is grounded (with coyote time)
    const float timeCoyote = 0.2f; //Constant that sets what is the coyoteTime
    public static Action OnPlayerLanded; //Action for knowing when player lands

    [Header("Jump")]
    [Tooltip("Speed on jump")]
    [SerializeField] float jumpSpeed = 8.0f;
    [Tooltip("Speed on double jump")]
    [SerializeField] float doublejumpSpeed = 8.0f;
    float jumpCount = 0f; //The current jumps player has
    public void SetJumpCount(int jumps) { jumpCount = jumps; } //Sets how many jumps player has
    float maxJumpCount => CanDoubleJump() ? 2 : 1; // Sets the maximum jump count
    //Buffering jump
    [Tooltip("The time the action jump can be triggered after the input was pressed. (Presses on air, how much time until it lands to jump automatically)")]
    [SerializeField] float jumpBufferTime;
    float jumpBufferCounter; //Updated timer
    bool canDetectJump = true; // Variable to know if game can detect jumps inputs
    #endregion

    void Start() //Gets information
    {
        playerVariableContainer = GameController.GetGameController().GetPlayer();
        currentDashCount = MaxDashCount();  
    }
    
    public void MovementInput() //Gets all Input
    {
        //Do not detect input if player can not move
        if (!playerVariableContainer.CanMove()) return;

        //Dash input start
        if(CanDash())
        {
          if (playerVariableContainer.ActionManager.Dash()) StartDash();
        }
        //Jump
        if (playerVariableContainer.ActionManager.Jump() && canDetectJump)
        {
            canDetectJump = false;
            Invoke("CanDetectJumpAgain", timeCoyote); //Uses coyote time then cannot detect jump while coyote jumping
            jumpBufferCounter = jumpBufferTime; 
        }
        if(jumpBufferCounter > 0.0f) 
        {
            if (IsCoyoteGrounded() || jumpCount < maxJumpCount)
            {
                Jump();
            }
            jumpBufferCounter -= Time.deltaTime;
        }
        //Stomp
        if (CanStomp())
        {
            if (playerVariableContainer.ActionManager.Stomp()) StartStomp();
        }
    }

    public void MovementUpdate() //Updates all movement
    {
        //Here we save the input of Movement on the variable movementDirectionInput
        movementDirectionInput = playerVariableContainer.ActionManager.Movement();

        // Move direction based on axis
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        isRunning = playerVariableContainer.ActionManager.Run();

        float newCurSpeedX = playerVariableContainer.CanMove() ? (isRunning ? runningSpeed : walkingSpeed) * movementDirectionInput.y : 0;
        float newCurSpeedY = playerVariableContainer.CanMove() ? (isRunning ? runningSpeed : walkingSpeed) * movementDirectionInput.x : 0;
        //Speed of player
        if (WantsAccelerationDeceleration)
        {
            curSpeedX = Mathf.Lerp(curSpeedX, newCurSpeedX, LerpSpeedAccelerationDeceleration * Time.deltaTime);
            curSpeedY = Mathf.Lerp(curSpeedY, newCurSpeedY, LerpSpeedAccelerationDeceleration * Time.deltaTime);
        }
        else { 
            curSpeedX = newCurSpeedX;
            curSpeedY = newCurSpeedY;
        }

        //Multiply depending if it's in rifle mode
        if (playerVariableContainer.WeaponController.IsInSniperMode())
        {
            curSpeedX *= speedMultiplierOnRifle;
            curSpeedY *= speedMultiplierOnRifle;
        }

        //OnAir Physics Speed
        if (!IsCoyoteGrounded())
        {
            curSpeedX = Mathf.Lerp(lastCurSpeedX, curSpeedX, lerpSpeedChangeDirectionBetweenOnAirMomentum * Time.deltaTime);
            curSpeedY = Mathf.Lerp(lastCurSpeedY, curSpeedY, lerpSpeedChangeDirectionBetweenOnAirMomentum * Time.deltaTime);

            //If starts input on air, no lerp, go straight at min speed
            if (lastCurSpeedX == 0 && curSpeedX != 0) curSpeedX = onAirMomentumMinSpeed;
            if (lastCurSpeedY == 0 && curSpeedY != 0) curSpeedY = onAirMomentumMinSpeed;
        }

        onAirMomentumSpeedReducer = IsCoyoteGrounded() ? //If player is grounded
            onAirMomentumSpeedReducer = onAirMomentumMaxSpeed : // Go at max Speed
            Mathf.Lerp(onAirMomentumSpeedReducer, onAirMomentumMinSpeed, lerpSpeedBetweenOnAirMomentum * Time.deltaTime); // Go less speed every time is on air

        curSpeedX *= onAirMomentumSpeedReducer;
        curSpeedY *= onAirMomentumSpeedReducer;

        //Velocity of player
        playerVariableContainer.MovementController.CharacterVelocity = ((forward * curSpeedX) + (right * curSpeedY));
        lastCurSpeedX = curSpeedX;
        lastCurSpeedY = curSpeedY;

        //Sets Player Vertical Speed (gravity)
        characterVelocityY += Physics.gravity.y * gravitysMultiplier * Time.deltaTime;
        playerVariableContainer.MovementController.CharacterVelocity.y = characterVelocityY;

        //Apply Momentum
        playerVariableContainer.MovementController.CharacterVelocity += playerVariableContainer.MovementController.CharacterVelocityMomentum; //Its time.deltatime in the reducer of itself

        //Apply Dash if necessary
        if (isDashing) Dash();

        //Apply Stomp if necessary
        if (isStomping) Stomp();

        //Max Down speed
        characterVelocityY = Mathf.Clamp(characterVelocityY, maxDownSpeed, Mathf.Infinity);

        // Move the controller
        CollisionFlags l_CollisionFlags = playerVariableContainer.CharacterController.Move(playerVariableContainer.MovementController.CharacterVelocity * Time.deltaTime);//Collision Flag is just for checking if player is colliding with something behind, if he is, VerticalSpeed should be 0 as he is not falling.
        if ((l_CollisionFlags & CollisionFlags.Above) != 0 && characterVelocityY > 0.0f) ResetCharacterVelocityY(); //If collides with any surface above

        if ((l_CollisionFlags & CollisionFlags.CollidedBelow) != 0) //If collides with any surface below
        {
            ResetCharacterVelocityY();
            onGround = true;
        }
        else
        {
            onGround = false;
        }

        //Landing Information
        if (timeOnAir > timeCoyote && onGround)
        {
            playerVariableContainer.MovementController.CharacterVelocityMomentum = Vector3.zero; //On land, reset momentum
            jumpCount = 0; //Resets jumps
            OnPlayerLanded?.Invoke(); //Calls the landing Action

            StompingLand(); //Ends stomp if it is stomping

            playerVariableContainer.PlayerAnimationSystem.Land(); //Calls the animation
            AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.land, transform.position); //Calls the sound
        }

        //Updates timeOnAir variable for the coyote jump
        if (!onGround) timeOnAir += Time.deltaTime;
        else timeOnAir = 0;

        //Gravity acceleration
        if (characterVelocityY < gravityNecesaryToAcceleration) characterVelocityY *= gravityAcceleration;

        //Dampen Momentum (Hook and Shotgun). Momentum = physics
        if (playerVariableContainer.MovementController.CharacterVelocityMomentum.magnitude >= 0f)
        {
            playerVariableContainer.MovementController.CharacterVelocityMomentum -= playerVariableContainer.MovementController.CharacterVelocityMomentum * momentumDividorSpeed * Time.deltaTime;
            if (playerVariableContainer.MovementController.CharacterVelocityMomentum.magnitude < .01f)
            {
                playerVariableContainer.MovementController.CharacterVelocityMomentum = Vector3.zero;
            }
        }
        
        
    }

    #region Stomp Related
    void StartStomp()
    {
        if (!isStomping )
        {
            isStomping = true;
            playerVariableContainer.PlayerParticlesSystem.StartMovementParticles();
        }
    }
    bool CanStomp()
    {
        return GameController.GetGameController().GetUpgradeManagerData().HasStompAbility && !IsCoyoteGrounded();
    }
    void Stomp()
    {
        playerVariableContainer.MovementController.CharacterVelocity = stompSpeed * -transform.up;
    }
    void StompingLand()
    {
        if (isStomping)
        {
            isStomping = false;
            foreach (IEnemyAI enemy in FindObjectsOfType<IEnemyAI>())
            {
                if (Vector3.Distance(enemy.transform.position, transform.position) < StompDistance) enemy.SetStompState();
            }
            playerVariableContainer.PlayerParticlesSystem.DoStompParticles();
            playerVariableContainer.PlayerParticlesSystem.StopMovementParticles();
            AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.stomp, transform.position);
        }
    }
    #endregion

    #region Dash Related
    void StartDash()
    {
        OnDashed?.Invoke();
       isDashing = true; //Set is dashing
       currentDashCount--; //Subtract a dash from dashes available
       dashStartTime = Time.time; //Set the time dash started
       currentDashCooldownTimer = dashCooldownTimer; //And reset the timer to the Cooldown

       if (playerVariableContainer.CharacterController.velocity.x == 0.0f){// If the player is not moving
           currentDashDirection = transform.forward; //Dashes in the direction the player is facing
       }
       else{ //Gets the direction player is moving so it's the dash direction
           float x = movementDirectionInput.x;
           float z = movementDirectionInput.y;
           currentDashDirection = transform.right * x + transform.forward * z;
       }

       //Audio & Particles
       AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.dash, transform.position);
       playerVariableContainer.PlayerParticlesSystem.StartMovementParticles();

       //Set the Dash FOV
       playerVariableContainer.CameraController.DashFOVEnabled = true;

       //Makes Player Invincible if has the Upgrade
       if (GameController.GetGameController().GetUpgradeManagerData().HasDashInvincibleAbility) playerVariableContainer.HealthSystem.StartInvencibilityTimer(playerVariableContainer.HealthSystem.GetDashTimeInvencible());
    }

    bool CanDash()
    {
        return GameController.GetGameController().GetUpgradeManagerData().HasDashAbility && !isDashing && currentDashCount > 0; //If has any dashes available and is not already dashing
    }

    void Dash()
    {
        float timeSinceDashStart = Time.time - dashStartTime; //Calculates the time in the dash

        if (timeSinceDashStart < dashTime) // If it's in the dashTime
        {
            playerVariableContainer.MovementController.CharacterVelocity = dashSpeed * currentDashDirection.normalized; //Set's the movement
        }
        else //DASH ENDS
        {
           isDashing = false; 

            //Camera FOV
           playerVariableContainer.CameraController.DashFOVEnabled = false;//Unset the Dash FOV

            //Particles
           playerVariableContainer.PlayerParticlesSystem.StopMovementParticles();//Stops movement particles

            //Movement
           ResetCharacterVelocityY(); //Resets gravity to 0

            //Style System
           playerVariableContainer.StyleSystem.EnableDashKill(); // Enables a DashKill         
        }

               
    }
    public void UpdateDashTimers()
    {
        if(currentDashCount < MaxDashCount() && !isDashing) //If can Charge a dash
        {
            currentDashCooldownTimer -= Time.deltaTime; 

            if (currentDashCooldownTimer <= 0) //Dash charged
            {
                currentDashCooldownTimer = 0;
                currentDashCount++;

                if(currentDashCount != MaxDashCount()) //If has another charge of dash available, charge it
                {
                    currentDashCooldownTimer = dashCooldownTimer;
                }

            }
        }
    }
    #endregion

    #region Jump Related
    bool CanDoubleJump()
    {
        return GameController.GetGameController().GetUpgradeManagerData().HasDoubleJumpAbility;
    }
    void CanDetectJumpAgain()
    { //It is invoked after pressing it after coyotetime
        canDetectJump = true;
    }
    void Jump()
    {
        if (IsCoyoteGrounded() || jumpCount < maxJumpCount)//if has touched ground less than 0.1f ago (is on ground, coyoteJump or bugfix for skin width component of the character controller (fixes as due to his width not every frame controller detects collision))
        {
            // Check if the player has double jump ability

            bool isDoubleJump = CanDoubleJump() && jumpCount == 1 ? true : false;
            float jumpSpeedToUse = isDoubleJump ? doublejumpSpeed : jumpSpeed;

            //Sound
            if(isDoubleJump) AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.doubleJump, transform.position);
            else AudioManager.GetAudioManager().PlayOneShot(AudioManager.GetAudioManager().Events.jump, transform.position);

            // Jump
            characterVelocityY = jumpSpeedToUse;
            jumpCount++;

            //Reset buffer counter
            jumpBufferCounter = 0;

            //Set Animation
            playerVariableContainer.PlayerAnimationSystem.Jump();
        }
    }
    #endregion

    public void StartShotgunnedDown(Vector3 cameraForward) //Sets movement to up if shoots looking down 
    {
        ResetCharacterVelocityY();
        shotgunnedDownForce = -cameraForward;
        playerVariableContainer.MovementController.CharacterVelocityMomentum = shotgunnedDownForce * ShotgunMomentumSpeedMultiplier;
    }
}

using UnityEngine;
using FMOD.Studio;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    #region Variables

    [Header("Player Movement classes")]
    [SerializeField] FPSMoveController fpsMoveController;
    public FPSMoveController FPSMoveController { get { return fpsMoveController; } }

    [SerializeField] GrapplingController grapplingController;
    public GrapplingController GrapplingController { get { return grapplingController; } }

    [Header("Movement States Related")]
    [SerializeField] MovementState playerState = MovementState.Normal;
    public enum MovementState { Normal, HookshotFlying, HookshotThrown }
    public MovementState PlayerState { get { return playerState; } }
    public void SetPlayerState(MovementState l_State) { playerState = l_State; }

    [Header("Movement Character Velocity & Momentum")]
    [Tooltip("This will update the inputted character velocity and gravitational")]
    public Vector3 CharacterVelocity;
    [Tooltip("This will update the momentum's character velocity such as the physics on the hook, shotgundown and more")]
    public Vector3 CharacterVelocityMomentum;

    //Player footsteps event instance
    EventInstance playerFootsteps;

    bool canCheckOnAirForArchievement = false;
    float timeOnAir = 0.0f;
    #endregion

    #region Start and OnDisable
    private void Start()
    {
        playerFootsteps = AudioManager.GetAudioManager().CreateEventInstance(AudioManager.GetAudioManager().Events.run);
    }
    private void OnEnable()
    {
        EnemySpawner.OnWaveStarted += CanCheckLevitation;
        EnemySpawner.OnWaveFinished += StopCheckLevitation;
    }
    private void OnDisable()
    {
        playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
        EnemySpawner.OnWaveStarted -= CanCheckLevitation;
        EnemySpawner.OnWaveFinished -= StopCheckLevitation;
    }
    #endregion

    #region Updates on the Input and Movement
    public void InputUpdate() // All input received on the different states
    {
        //No need to detect player movement input if Player can not move
        if (!GameController.GetGameController().GetPlayer().CanMove()) return;

        switch (playerState)
        {
            default:
            case MovementState.Normal: //While moving normally
                grapplingController.HookshotStart(); //Player Can Start HookShot
                fpsMoveController.MovementInput(); //Player can move freely FPS
                break;
            case MovementState.HookshotThrown: // While shooting hook
                fpsMoveController.MovementInput(); //Player Can still move freely FPS
                grapplingController.HookshotThrowInput(); //Player can get input on throwing hook, to cancel it.
                break;
            case MovementState.HookshotFlying: // While flying on the hookshot
                grapplingController.HookshotMovementInput(); //Player can get input while moving on the move hook, to cancel, jump...
                break;
        }
    }
    public void MovementTimersUpdate() //Updates timers
    {
        fpsMoveController.UpdateDashTimers(); //Update dash timer
    }
    public void MovementUpdate() //Updates the movement of the different player states
    {
        //Footsteps from player sound
        if (playerState == MovementState.Normal && fpsMoveController.IsCoyoteGrounded()
            && GameController.GetGameController().GetPlayer().CanMove()
            && (CharacterVelocity.x != 0 || CharacterVelocity.z != 0)
            && GameController.GetGameController().GetPlayer().CharacterController.velocity.magnitude != 0)
        {
            PLAYBACK_STATE playbackState;
            playerFootsteps.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerFootsteps.start();
            }
        }
        else
        {
            playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
        }


        //Update depending on the Movement State
        switch (playerState)
        {
            default:
            case MovementState.Normal:
                fpsMoveController.MovementUpdate(); //Normal FPS Movement Update (WASD, Gravity, Dash, Stomp...)
                break;
            case MovementState.HookshotThrown:
                fpsMoveController.MovementUpdate(); //Normal FPS Movement Update (WASD, Gravity, Dash, Stomp...)
                grapplingController.HookshotThrowUpdate(); //Update of the throw on the hook
                break;
            case MovementState.HookshotFlying:
                grapplingController.HookshotMovementUpdate(); //Update of the movement of the player while using hook
                break;
        }


        if (!ArchievementsNameConstantsData.HaveToCheckOnLevitation()) return;
        if (!canCheckOnAirForArchievement) return;

        //Archievement
        if (!GameController.GetGameController().GetPlayer().CharacterController.isGrounded
            && GameController.GetGameController().GetPlayer().CanMove())
        {
            timeOnAir += Time.deltaTime;
            if (timeOnAir >= 15.0f) ArchievementsNameConstantsData.LevitationChecker(timeOnAir);
        }
        else { timeOnAir = 0.0f; }
    }
    #endregion

    void CanCheckLevitation(int wave)
    {
        canCheckOnAirForArchievement = true;
    }
    void StopCheckLevitation(int wave)
    {
        canCheckOnAirForArchievement = false;
    }
}

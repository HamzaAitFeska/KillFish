using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(PlayerInput))]
public class ActionManager : MonoBehaviour {

    // Enum to determine input's type
    public enum INPUT_SCHEME {
        I_KEYBOARD, I_GAMEPAD
    }

    // Observer to when Change Scene
    public static event Action<INPUT_SCHEME> OnChangeInput;

    // Component New Input System
    [SerializeField]
    private PlayerInput m_Input;
    public PlayerInput GetActionsInput => m_Input;

    // Control of actual's Scheme
    [SerializeField]
    private INPUT_SCHEME m_CurrentScheme;
    public INPUT_SCHEME Scheme() { return m_CurrentScheme; }
    public bool GamePad() { return (Scheme().Equals(INPUT_SCHEME.I_GAMEPAD)); }
    public bool Keyboard() { return (Scheme().Equals(INPUT_SCHEME.I_KEYBOARD)); }

    // Actions
    private Vector2 m_Movement;
    private float m_HorizontalMove;
    private float m_VerticalMove;
    private bool m_MoveForward;
    private bool M_MoveBackward;
    private bool m_Jump;
    private bool m_Run;
    private bool m_Dash;
    private Vector2 m_CameraMovement;
    private bool m_Hook;
    private bool m_LeftClick;
    private bool m_RightClick;
    private bool m_Escape;
    private bool m_Interact;
    private bool m_Fire1;
    private bool m_Fire2;
    private bool m_MovementUpgrade;
    private bool m_HookUpgrade;
    private bool m_DamageUpgrade;
    private bool m_Stomp;
    private bool m_LeftShop;
    private bool m_RightShop;

    // Métodos Check??
    public Vector2 Movement() { return m_Movement; }
    public float VerticalMove() { return m_VerticalMove; }
    public float HorizontalMove() { return m_HorizontalMove; }
    public bool MoveForward() { return m_MoveForward; }
    public bool MoveBackward() { return M_MoveBackward; }
    public bool Jump() { return m_Jump; }
    public bool Run() { return m_Run; }
    public bool Dash() { return m_Dash; }
    public bool HookKeyStart() { return m_Hook; }
    public Vector2 CameraMovement() { return m_CameraMovement; }
    public bool Escape() { return m_Escape; }
    public bool Interact() { return m_Interact;}
    public bool Fire1() { return m_Fire1;}
    public bool Fire2() { return m_Fire2;}

    public bool MovementUpgrade() { return m_MovementUpgrade; }
    public bool HookUpgrade() { return m_HookUpgrade; } 
    public bool DamageUpgrade() { return m_DamageUpgrade; }
    public bool Stomp() { return m_Stomp; }

    public bool LeftShop() { return m_LeftShop; }
    public bool RightShop() { return m_RightShop; }
    // Unity Awake
    void Awake() {
        m_Input = GetComponent<PlayerInput>();
        m_CurrentScheme = INPUT_SCHEME.I_KEYBOARD;
    }

    // Unity Start
    void Start() { }

    // Unity Update
    void Update() { }

    // * ----------------------------------------------------------------------------------------------------------------------------------- *
    // | - Send Mesagges - PlayerInput Component ------------------------------------------------------------------------------------------- |
    // V ----------------------------------------------------------------------------------------------------------------------------------- V
    void OnControlsChanged() {
        if (m_Input.currentControlScheme.Equals("Gamepad")) m_CurrentScheme = INPUT_SCHEME.I_GAMEPAD;
        if (m_Input.currentControlScheme.Equals("Keyboard&Mouse")) m_CurrentScheme = INPUT_SCHEME.I_KEYBOARD;
        OnChangeInput?.Invoke(Scheme());
    }

    void OnHorizontalMove(InputValue value)
    {
        m_HorizontalMove = value.Get<float>();
        
        
    }

    void OnVerticalMove(InputValue value)
    {
        m_VerticalMove = value.Get<float>();
    }
    
    void OnMoveForward(InputValue value) {
        m_MoveForward = value.isPressed;
    }

    void OnMoveBackward(InputValue value) {
        M_MoveBackward = value.isPressed;
    }

    void OnRun(InputValue value) {
        m_Run = value.isPressed;
    }
    IEnumerator OnDash(InputValue value)
    {
        m_Dash = value.isPressed;
        yield return new WaitForEndOfFrame();
        m_Dash = false;
        
    }
    IEnumerator OnHook(InputValue value)
    {
        m_Hook = value.isPressed;
        yield return new WaitForEndOfFrame();
        m_Hook = false;
    }

    IEnumerator OnJump(InputValue value)
    {
        m_Jump = value.isPressed;
        yield return new WaitForEndOfFrame();
        m_Jump = false;
    }

    void OnMovement(InputValue value)
    {
        m_Movement = value.Get<Vector2>();
        m_Movement.Normalize();
    }

    void OnCameraMovement(InputValue value) {
        m_CameraMovement = value.Get<Vector2>();
    }


    IEnumerator OnLeftClick(InputValue value) {
        m_LeftClick = value.isPressed;
        yield return new WaitForEndOfFrame();
        m_LeftClick = false;
    }
    void OnRightClick(InputValue value)
    {
        m_RightClick = value.isPressed;
    }

    IEnumerator OnEscape(InputValue value) {
        m_Escape = value.isPressed;
        yield return new WaitForEndOfFrame();
        m_Escape = false;
    }

    IEnumerator OnInteract(InputValue value)
    {
        m_Interact = value.isPressed;
        yield return new WaitForEndOfFrame();
        m_Interact = false;
       
    }

    IEnumerator OnFire1(InputValue value)
    {
        m_Fire1 = value.isPressed;
        yield return new WaitForEndOfFrame();
        m_Fire1 = false;
    }

    void OnFire2(InputValue value)
    {
        m_Fire2 = value.isPressed;
    }

    void OnMovementUpgrade(InputValue value)
    {
        m_MovementUpgrade = value.isPressed;
    }

    void OnHookUpgrade(InputValue value)
    {
        m_HookUpgrade = value.isPressed;
    }

    void OnDamageUpgrade(InputValue value)
    {
        m_DamageUpgrade = value.isPressed;
    }

    IEnumerator OnStomp(InputValue value)
    {
        m_Stomp = value.isPressed;
        yield return new WaitForEndOfFrame();
        m_Stomp = false;

    }
    IEnumerator OnShopLeft(InputValue value)
    {
        m_LeftShop = value.isPressed;
        yield return new WaitForEndOfFrame();
        m_LeftShop = false;
    }
    IEnumerator OnShopRight(InputValue value)
    {
        m_RightShop = value.isPressed;
        yield return new WaitForEndOfFrame();
        m_RightShop = false;
    }

    // A ----------------------------------------------------------------------------------------------------------------------------------- A
    public string GetInputKeyStringForUI(InputActionReference actionref)
    {
        actionref.action.bindings[0].ToString();
        return actionref.action.bindings[0].ToDisplayString();

    }
}

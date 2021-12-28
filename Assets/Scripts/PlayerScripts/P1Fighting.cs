using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ComboState { 
    NONE,
    PUNCH_1,
    PUNCH_2,
    PUNCH_3,
    KICK_1,
    KICK_2
}

public class P1Fighting : MonoBehaviour
{
    // Player Variables
    private PlayerInput playerInput;
    private Animator animator;

    // Combo Variables
    private bool activateTimerToReset;

    private float default_Combo_Timer = 0.4f;
    private float current_Combo_Timer;

    private ComboState current_Combo_State;

    // Animation Hashes
    public const string punch1Hash = "Punch1";
    public const string punch2Hash = "Punch2";
    public const string punch3Hash = "Punch3";

    public const string kick1Hash = "Kick1";
    public const string kick2Hash = "Kick2";

    private bool isPunchPressed;
    private bool isKickPressed;

    void Awake()
    {
        playerInput = new PlayerInput();
        animator = GetComponent<Animator>();

        playerInput.CharacterControls.Punch.started += onPunch;
        playerInput.CharacterControls.Punch.canceled += onPunch;

        playerInput.CharacterControls.Kick.started += onKick;
        playerInput.CharacterControls.Kick.canceled += onKick;
    }

    void onPunch(InputAction.CallbackContext context)
    {
        isPunchPressed = context.ReadValueAsButton();
    }

    void onKick(InputAction.CallbackContext context)
    {
        isKickPressed = context.ReadValueAsButton();
    }

    void Start()
    {
        current_Combo_Timer = default_Combo_Timer;
        current_Combo_State = ComboState.NONE;
    }

    void HandleFighting()
    {
        if(isPunchPressed)
        {
            isPunchPressed = false;
            if (current_Combo_State == ComboState.PUNCH_3 ||
                current_Combo_State == ComboState.KICK_1 ||
                current_Combo_State == ComboState.KICK_2)
                return;

            current_Combo_State++;
            activateTimerToReset = true;
            current_Combo_Timer = default_Combo_Timer;

            if(current_Combo_State == ComboState.PUNCH_1) {
                animator.SetTrigger(punch1Hash);
            }

            if (current_Combo_State == ComboState.PUNCH_2) {
                animator.SetTrigger(punch2Hash);
            }

            if (current_Combo_State == ComboState.PUNCH_3) {
                animator.SetTrigger(punch3Hash);
            }
        }

        if(isKickPressed)
        {
            isKickPressed = false;
            // if the current combo is punch 3 or kick 2
            // return meaning exit because we have no combos to perform
            if (current_Combo_State == ComboState.KICK_2 ||
                current_Combo_State == ComboState.PUNCH_3)
                return;

            // if the current combo state is NONE, or punch1 or punch2
            // then we can set current combo state to kick 1 to chain the combo
            if(current_Combo_State == ComboState.NONE ||
                current_Combo_State == ComboState.PUNCH_1 ||
                current_Combo_State == ComboState.PUNCH_2) {

                current_Combo_State = ComboState.KICK_1;

            } else if(current_Combo_State == ComboState.KICK_1) {
                // move to kick2
                current_Combo_State++;
            }

            activateTimerToReset = true;
            current_Combo_Timer = default_Combo_Timer;

            if(current_Combo_State == ComboState.KICK_1) {
                animator.SetTrigger(kick1Hash);
            }

            if (current_Combo_State == ComboState.KICK_2) {
                animator.SetTrigger(kick2Hash);
            }
        }     
    }

    // Update is called once per frame
    void Update()
    {
        HandleFighting();
        ResetComboState();
    }

    void ResetComboState()
    { 
        if(activateTimerToReset)
        {
            current_Combo_Timer -= Time.deltaTime;

            if(current_Combo_Timer <= 0f) {

                current_Combo_State = ComboState.NONE;

                activateTimerToReset = false;
                current_Combo_Timer = default_Combo_Timer;
            }
        }
    }

    // called when the object becomes enabled and active.
    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    // called when the object becomes disabled and inactive.
    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}


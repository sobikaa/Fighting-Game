using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// controls all player movement
public class AnimationAndMovementControl : MonoBehaviour
{
    // Player variables
    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator animator;
    
    // variables for hashed animation bool conditions 
    private int isWalkingHash;
    private int isRunningHash;
    private int isJumpingHash;

    // variables to store player input
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;
    private Vector3 appliedMovement;
    private bool isMovementPressed;
    private bool isRunPressed;

    // constants
    [SerializeField] private float rotationFactorPerFrame = 15.0f;
    [SerializeField] private float runMultiplier = 8.0f;
    [SerializeField] private float walkMultiplier = 2.5f;
    [SerializeField] private float gravity = -9.8f;
    private float groundedGravity = -9.8f;

    // jumping variables
    private bool isJumpPressed = false;
    private float initialJumpVelocity;
    [SerializeField] private float maxJumpHeight = 2.0f;
    [SerializeField] private float maxJumpTime = 0.5f;
    private bool isJumping = false;
    private bool isJumpAnimating = false;

    //audio source
    [SerializeField] private AudioSource runSound;
    [SerializeField] private AudioSource jumpSound;


    // called when the script instance is being loaded.
    void Awake()
    {
        // initially set reference variables
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // set the parameter hash references
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");

        // set the player input callbacks
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;

        setupJumpVariables();
    }

    // defines jump height, duration and gravity
    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    // determines if input callbacks are triggered
    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();

    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
        runSound.Play();
    }

    // handler functions for Jumping, Rotating, Animating and Gravity
    void handleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            currentMovement.y = initialJumpVelocity;
            appliedMovement.y = initialJumpVelocity;
        }
        else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;
        // the change in position the character should point to
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        // the current rotation of our character
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed){
            // creates a new rotation based on where the player is currently pressing
            Quaternion targetRotaion = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotaion, rotationFactorPerFrame * Time.deltaTime);

        }
    }

    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if (!isMovementPressed && isWalking) 
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }

    void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f;
        float fallMultiplier = 2.0f ;

        // apply gravity depending on if the character is grounded or not
        if (characterController.isGrounded)
        {
            if (isJumpAnimating){
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
                jumpSound.Play();

            }
            currentMovement.y = groundedGravity;
            appliedMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            appliedMovement.y = Mathf.Max((previousYVelocity + currentMovement.y) * .5f, -15.0f);
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (gravity * Time.deltaTime);
            appliedMovement.y = (previousYVelocity + currentMovement.y) * .5f;
        }
    }

    // handler function to set player input values
    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * walkMultiplier;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0;

    }

    // Update is called once per frame
    void Update()
    {
        handleRotation();
        handleAnimation();

        if (isRunPressed)
        {
            appliedMovement.x = currentRunMovement.x;
        }
        else
        {
            appliedMovement.x = currentMovement.x;
        }

        characterController.Move(appliedMovement * Time.deltaTime);

        handleGravity();
        handleJump();
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

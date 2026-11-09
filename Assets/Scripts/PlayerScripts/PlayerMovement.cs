using UnityEngine;
using UnityEngine.InputSystem;
// Basic player movement
// Melissa coded

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f; 
    public float rotationSpeed = 10f;
    public float jumpPower = 2f;
    public float gravity = -20f;
    public float sprintSpeed = 4f;
    public float sprintTiming = .3f;
    
    public float verticalVelocity; 

    public CharacterController controller;
    public Transform cameraTransform;
    //private Animator animator;
    private Vector2 moveInput;
    [SerializeField] private PlayerStamina stamina;

    //to have the sprint activate when w or arrow is pressed twice quickly
    private bool isSprinting;
    private bool wasPressed;
    private float lastForwardPressTime = -10f;
    
    void Awake() {
        controller = GetComponent<CharacterController>();
        //animator = GetComponentInChildren<Animator>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update(){
        //future pause controller here 
        // if (isPaused || isHiding) { Vector3 moveDirection = 0; return;}

        //camera slows down without normalizing these values:
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x);

        //turn player to face the direction of movement
        if(moveDirection.sqrMagnitude > 0.01f){
            Quaternion roto = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, roto, rotationSpeed * Time.deltaTime);
        }

        //handle gravity
        if(controller.isGrounded && verticalVelocity < 0f){
            verticalVelocity = -2f;
        }
        verticalVelocity += gravity * Time.deltaTime;

        //calculate correct velocity depending on where player is sprinting or not
        Vector3 velocity = moveDirection * speed;
        if(isSprinting && stamina.getStamina()){
            velocity = moveDirection * sprintSpeed;
        } 

        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        //future animator here
    }

    public void Move(InputAction.CallbackContext context){
        Vector2 input = context.ReadValue<Vector2>();
        bool justPressed = input.sqrMagnitude > 0.25f;

        //double press move to activate sprint
        if(justPressed && !wasPressed && stamina.getStamina()){
            if(Time.time - lastForwardPressTime <= sprintTiming){
                //print("sprinting");
                isSprinting = true;
            }
            lastForwardPressTime = Time.time;
        }

        //once player lets go of w or arrow, stops sprinting
        if(!justPressed || !stamina.getStamina()){
            //print("no longer sprinting");
            isSprinting = false;
        }

        wasPressed = justPressed;
        moveInput = input;
    }

    //player jump
    public void Jump(InputAction.CallbackContext context){
        if(context.performed && controller.isGrounded && stamina.currentStamina >= stamina.jump){
            verticalVelocity = Mathf.Sqrt(jumpPower * -2f * gravity);
            stamina.Jump();
       }
    }


    public bool getSprinting(){
        return isSprinting;
    }
}



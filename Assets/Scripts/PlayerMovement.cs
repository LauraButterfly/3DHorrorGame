using UnityEngine;
using UnityEngine.InputSystem;
// Basic player movement
// Melissa coded

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f; //character speed
    public float rotationSpeed = 10f;
    public float jumpPower = 2f;
    public float gravity = -20f;
    
    public float verticalVelocity; 

    public CharacterController controller;
    public Transform cameraTransform;
    //private Animator animator;
    private Vector2 moveInput;
    
    void Awake() {
        controller = GetComponent<CharacterController>();
        //animator = GetComponentInChildren<Animator>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update(){
        //future pause controller here

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

        Vector3 velocity = moveDirection * speed;
        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        //future animator here
    }

    public void Move(InputAction.CallbackContext context){
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context){
        if(context.performed && controller.isGrounded){
            verticalVelocity = Mathf.Sqrt(jumpPower * -2f * gravity);
        }
    }
}



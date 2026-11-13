
using UnityEngine.InputSystem;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // Movement Variables
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    // Rotational Variables
    public float rotationSpeed = 720f; // Controls how fast player rotates in degrees per second. Higher = faster
    private CharacterController controller;
    private Transform cameraTransform;
    private Vector3 velocity;
    private Animator animator;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        // Get the main camera's transform
        // transform is the position, rotation, and scale info
        // Camera.main looks for a camera with tthe MainCamera tag.
        cameraTransform = Camera.main.transform;
        animator = GetComponentInChildren<Animator>();
     }

    void Update()
    {
        // Get the camera directions
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        // removes the vertical movement so player doesnt walk down/up
        cameraForward.y = 0f; // direction in front of the camera
        cameraRight.y = 0f; // Direction to the right of the camera
        // Normalize directions - adusts vector magnitude to 1 if moving diagonally
        cameraForward.Normalize();
        cameraRight.Normalize();
        //Get WASD input
        Vector3 move = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            move += cameraForward;
        if (Keyboard.current.sKey.isPressed)
            move -= cameraForward;
        if (Keyboard.current.dKey.isPressed)
            move += cameraRight;
        if (Keyboard.current.aKey.isPressed)
            move -= cameraRight;

        // Prevent diagonal movement from being faster
        move = move.normalized; // Takes the movement vector's exact direction while changing its magnitude to 1
        //Update walking animation
        bool IsWalking = (move != Vector3.zero); // is walking if move is not a zero vector
        animator.SetBool("IsWalking", IsWalking);
        // Rotate player towards movement direction
        if (move!= Vector3.zero){ // < -- Checks if moevment vector is different than (0,0,0) aka if player is pressing any keys
            // Quaternion = Unity data type to represent rotations
            // Quaternity.LookRotation() calculates the rotation needed to face a particular direction
            Quaternion targetRotation = Quaternion.LookRotation(move); // Determine which direction to face
// Smooths rotation
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, // PC current rotation
                targetRotation, // Direction PC should face
                rotationSpeed * Time.deltaTime // How quickly to rotate; deltaTime keeps rotation consistent across frames
            );
        }

        // The actual move action - Move the character horizontally
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Apply gravity so player doesn't float away
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        // Move vertically
        controller.Move(velocity * Time.deltaTime);
    }
}

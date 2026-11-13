
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
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Get WASD input
        Vector3 move = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            move.z += 1f;

        if (Keyboard.current.sKey.isPressed)
            move.z -= 1f;

        if (Keyboard.current.aKey.isPressed)
            move.x -= 1f;

        if (Keyboard.current.dKey.isPressed)
            move.x += 1f;

        // Prevent diagonal movement from being faster
        move = move.normalized; // Takes the movement vector's exact direction while changing its magnitude to 1
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

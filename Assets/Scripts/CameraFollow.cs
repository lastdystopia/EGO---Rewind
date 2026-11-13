using UnityEngine;
using UnityEngine.InputSystem;
public class CameraFollow: MonoBehaviour
{
    // Player object for camera to follow
    public Transform target;

    //Camera positioning
    public float distance = 6f;
    public float lookHeight = 1.5f;

    // Camera Movement Speeds
    public float rotationSpeed = 100f;
    public float tiltSpeed = 60f;
    public float positionSmoothness = 5f;
    //Camera rotation limits
    public float minTilt = -20f;
    public float maxTilt = 70f;
    // Current camera angles
    private float yaw; // yaw is the rotation or turning of an object's nose side to side. Side camera movement
    private float pitch;// Pitch is the rotation of an object's nose on front end up/down. Up/down tilt movement

    void Start(){
        if (target ==null){
            return;
        }
        // Determine the starting camera angle from its current position
        // Vector3 pivot = target.position +Vector3.up*lookHeight;
        // Vector3 offset = target.position - pivot;

        // if (offset.sqrMagnitude>.001f){
        //     distance = offset.magnitude;

        //     Vector3 direction = offset.normalized;

        //     yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //     pitch = Mathf.Asin(direction.y)* Mathf.Rad2Deg;
        // }
        // Set the camera's starting rotation
        transform.rotation = Quaternion.Euler(18.45f, 6.2f, 0f);    

        // Initialize the camera angles
        pitch = 18.45f;
        yaw = 6.2f;
    }
    // Late update because we want the camera to follow the player after their action has updated
    void LateUpdate(){
        // if no player, do nothing
        if(target == null){
            return;
        }
        if (Keyboard.current ==null)
            {return;}

        //J and L rotate the camera left
        if(Keyboard.current.jKey.isPressed)
            yaw -= rotationSpeed * Time.deltaTime;// rotate the yaw left
        if (Keyboard.current.lKey.isPressed)
            yaw += rotationSpeed * Time.deltaTime;//rotate the yaw right
        
        //I and K tilt camera up down
        if (Keyboard.current.iKey.isPressed)
            pitch += tiltSpeed * Time.deltaTime; //rotate camera above player facing them
        if (Keyboard.current.kKey.isPressed)
            pitch -= tiltSpeed * Time.deltaTime;// rotate the camera down to the player's feet
        
        //Prevent camera from rotation too far vertically
        pitch = Mathf.Clamp(pitch, minTilt, maxTilt);
        // Calculate point camera should orbit around
        Vector3 pivot = target.position +Vector3.up * lookHeight;

        // convert camera angles into a direction
        Quaternion cameraRotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 cameraDirection = cameraRotation * Vector3.forward;
        // Calculate desired camera position
        Vector3 desiredPosition = 
           pivot - cameraDirection * distance;
        // Follow the player
        transform.position = Vector3.Lerp(
            transform.position, // The current position
            desiredPosition, // The position to get to
            positionSmoothness* Time.deltaTime); // The smoothness of position transition * framerate

        // Calculate the position the camera should look at = always look at the player
        transform.rotation = Quaternion.LookRotation(pivot-transform.position);
    }
}

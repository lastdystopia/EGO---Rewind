using UnityEngine;
// Camera modes
public enum CameraMode
{
    FULL, LIMITED
}
public class CameraFollow: MonoBehaviour
{
    // Player object for camera to follow
    public Transform target;
    public CameraMode cameraMode = CameraMode.FULL;
    //Camera positioning
    public float distance = 6f;
    public float lookHeight = 1.5f;

    // Camera Movement Speeds
    public float rotationSpeed = 100f;
    public float tiltSpeed = 60f;
    public float positionSmoothness = 5f;
    // Pan variables so camera auto-pans
    public float panAmount = 8f;
    public float panSpeed = 15f;
    private float currentPan = 0f;

    //Camera rotation limits (up/down)
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
       
        //Read Camera movement input from game input manager
        Vector2 CameraInput = GameInputManager.Instance.CameraInput.ReadValue<Vector2>();

        // Detect A/D movement for auto pan
        float panInput = GameInputManager.Instance.MoveInput.ReadValue<Vector2>().x;
        // Accumulate camera pan
        currentPan += panInput * panSpeed * Time.deltaTime;
        //limit camera pan
        currentPan = Mathf.Clamp(currentPan, -panAmount, panAmount);
        
        yaw += CameraInput.x * rotationSpeed * Time.deltaTime;
        pitch += CameraInput.y * tiltSpeed * Time.deltaTime;
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
        // Calculate horizontal direction of camera 
        Vector3 cameraRight = cameraRotation * Vector3.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();
        // Follow the player
        transform.position = Vector3.Lerp(
            transform.position, // The current position
            desiredPosition, // The position to get to
            positionSmoothness* Time.deltaTime); // The smoothness of position transition * framerate

        // Calculate the position the camera should look at = always look at the player
        transform.rotation = Quaternion.LookRotation(pivot-transform.position);
    }
}

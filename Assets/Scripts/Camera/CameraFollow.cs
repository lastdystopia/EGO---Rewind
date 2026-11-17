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
    // Player visibility
    [Header("Player Visibility")]
    public float invisibilityDistance = 1.0f;

    private Renderer[] playerRenderers;
    private bool playerIsInvisible = false;
    //Camera positioning
    public float distance = 6f;
    public float lookHeight = 1.5f;
    [Header("Camera Collision")]
    public float collisionRadius = .25f; // size of invisible sphere used to detect obstacles
    public float collisionPadding = .1f; // how far camera stays from surface it hits
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
        // Get all renderers belonging to player and children
        playerRenderers = target.GetComponentsInChildren<Renderer>(true);
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

        if (cameraMode == CameraMode.FULL)
        {
            float allowedDistance = GetCameraCollisionDistance(pivot, desiredPosition);
            desiredPosition = pivot - cameraDirection * allowedDistance;
        }
        // Hide player if camera gets too close
        float actualCameraDistance = Vector3.Distance(pivot, desiredPosition);
        if (cameraMode == CameraMode.FULL)
            SetPlayerVisibility(actualCameraDistance>invisibilityDistance);
        else SetPlayerVisibility(true);
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
    private void SetPlayerVisibility(bool visible)
    {
        // // avoid repeatedly enabling or disabling renders
         if (playerIsInvisible == !visible)
             return;
        foreach(Renderer renderer in playerRenderers)
        {
            renderer.enabled = visible;
        }

        playerIsInvisible = !visible;
    }
    // Restore visibility if camera is disabled
    private void OnDisable()
    {
        if (playerRenderers ==null)
            return;
        foreach (Renderer renderer in playerRenderers)
        {
            if (renderer != null)
                renderer.enabled = true;
        }
        playerIsInvisible = false;
    }
    // Collision Detection Method
    private float GetCameraCollisionDistance(Vector3 pivot, Vector3 desiredPosition)
    {
        // Direction from player toward intended camera position
        Vector3 direction = (desiredPosition - pivot).normalized;
        // Check all solid objects between player and camera
        RaycastHit[] hits = Physics.SphereCastAll(
            pivot,
            collisionRadius,
            direction, 
            distance,
            Physics.AllLayers,
            QueryTriggerInteraction.Ignore
        );
        // start with full camera distance
        float closestDistance = distance;

        foreach (RaycastHit hit in hits)
        {
            // ignore player & its children
            if (hit.transform == target || hit.transform.IsChildOf(target))
                continue;
            // Find closest obstacle
            if (hit.distance < closestDistance)
            {
                closestDistance = hit.distance;
            }

        }
        // keep camera slightly away from obstacle
        return Mathf.Max(0f, closestDistance - collisionPadding);
    }
}

using UnityEngine;

public class CameraFollow: MonoBehaviour
{
    // Player object for camera to follow
    public Transform target;

    //Camera positioning
    public float distance = 6f;
    public float height = 3f;
    public float lookHeight = 1.5f;

    //Camera smoothness
    public float positionSmoothness = 5f;
    public float rotationSmoothness = 5f;
    // Late update because we want the camera to follow the player after their action has updated
    void LateUpdate(){
        // if no player, do nothing
        if(target == null){
            return;
        }
        // Calculate where the camera should be
        // Always behind the player when set, and above
        Vector3 desiredPosition = 
            target.position - (target.forward * distance) + Vector3.up * height;
        // Move the camera into position
        transform.position = Vector3.Lerp(
            transform.position, // The current position
            desiredPosition, // The position to get to
            positionSmoothness* Time.deltaTime); // The smoothness of position transition * framerate

        // Calculate the position the camera should look at
        Vector3 lookPosition = target.position  + Vector3.up*lookHeight;
        //Calculate the desired camera rotation
        Quaternion desiredRotation = Quaternion.LookRotation(lookPosition-transform.position);
        //Smooth rotate the camera
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            rotationSmoothness * Time.deltaTime
        );
    }
}

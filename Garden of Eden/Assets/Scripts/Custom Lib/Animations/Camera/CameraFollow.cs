using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    #region Variables

    public Transform target = null; // Follow Target
    private Transform thisTransform = null; // Reference to local transform
    public float distanceFromTarget = 10.0f; // Linear distance to maintain from target
    public float camHeight = 1.0f; // Height of camera above target
    public float rotationDamp = 4.0f; // Damping for rotation
    public float posDamp = 4.0f; // Damping for position

    #endregion

    #region Unity Events

    void Awake () {
        // Get transform for camera
        thisTransform = GetComponent<Transform>();
    }

    void LateUpdate () {
        // Get output velocity
        Vector3 velocity = Vector3.zero;
        // Calculate rotation interpolate
        thisTransform.rotation = Quaternion.Slerp(thisTransform.rotation , target.rotation , rotationDamp * Time.deltaTime);
        // Get new position
        Vector3 destination = thisTransform.position = Vector3.SmoothDamp(thisTransform.position, target.position, ref velocity, posDamp * Time.deltaTime);
        // Move away from target
        thisTransform.position = destination - (thisTransform.forward * distanceFromTarget);
        // Set height
        thisTransform.position = new Vector3(thisTransform.position.x , camHeight, thisTransform.position.y);
        // Look at destination
        thisTransform.LookAt(destination);
    }

    #endregion

}

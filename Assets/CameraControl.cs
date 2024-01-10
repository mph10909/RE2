using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float rotationSpeed = 2.0f; // Rotation speed of the camera
    public float minDistance = 2.0f; // Minimum distance from the player
    public float maxDistance = 10.0f; // Maximum distance from the player
    public float minYAngle = -30.0f; // Minimum vertical angle
    public float maxYAngle = 30.0f; // Maximum vertical angle

    private bool isRotating = false;
    private float currentRotationX;
    private float currentDistance;

    private void Start()
    {
        currentRotationX = transform.rotation.eulerAngles.x;
        currentDistance = maxDistance; // Initialize the current distance to the maximum distance
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2)) // Middle mouse button is pressed
        {
            isRotating = true;
        }

        if (Input.GetMouseButtonUp(2)) // Middle mouse button is released
        {
            isRotating = false;
        }

        if (isRotating)
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.RotateAround(player.position, Vector3.up, mouseX * rotationSpeed);

            float mouseY = Input.GetAxis("Mouse Y");
            currentRotationX -= mouseY * rotationSpeed;
            currentRotationX = Mathf.Clamp(currentRotationX, minYAngle, maxYAngle);

            transform.rotation = Quaternion.Euler(currentRotationX, transform.rotation.eulerAngles.y, 0);
        }

        // Scroll wheel input to adjust the distance
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        currentDistance = Mathf.Clamp(currentDistance - scrollWheel, minDistance, maxDistance);

        // Adjust the camera's position relative to the player
        Vector3 cameraPosition = player.position - transform.forward * currentDistance;
        transform.position = cameraPosition;
    }
}

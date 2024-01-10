using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 10.0f;
    public float rotationThreshold = 90.0f;
    public Vector3 rotationAxis = Vector3.forward;

    private float currentRotation = 0.0f;
    [SerializeField] bool rotateClockwise = true;

    void Update()
    {
        if (rotateClockwise)
        {
            currentRotation += rotationSpeed * Time.unscaledDeltaTime;
        }
        else
        {
            currentRotation -= rotationSpeed * Time.unscaledDeltaTime;
        }

        if (currentRotation >= rotationThreshold && rotateClockwise)
        {
            rotateClockwise = false;
            currentRotation = 2.0f * rotationThreshold - currentRotation;
        }
        else if (currentRotation <= -rotationThreshold && !rotateClockwise)
        {
            rotateClockwise = true;
            currentRotation = -2.0f * rotationThreshold - currentRotation;
        }

        Quaternion originalRotation = Quaternion.Euler(-90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        Quaternion newRotation = Quaternion.AngleAxis(currentRotation, rotationAxis);
        transform.rotation = originalRotation * newRotation;
    }
}

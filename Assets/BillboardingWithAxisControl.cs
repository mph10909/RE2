using UnityEngine;

public class BillboardingWithAxisControl : MonoBehaviour
{
    public Transform target; // The target to look at
    public Vector3 lockedAxis = Vector3.right; // The axis you want to lock

    // Axis rotation control variables
    public bool rotateX = true;
    public bool rotateY = true;
    public bool rotateZ = true;

    protected virtual void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the direction from the object to the target
            Vector3 toTarget = target.position - transform.position;

            // Calculate the rotation with locked axis
            Quaternion rotation = Quaternion.LookRotation(toTarget, lockedAxis);

            // Apply the rotation to the object
            transform.rotation = rotation;

            // Apply axis rotation control
            Vector3 rotationEuler = transform.eulerAngles;

            if (!rotateX) rotationEuler.x = 0;
            if (!rotateY) rotationEuler.y = 0;
            if (!rotateZ) rotationEuler.z = 0;

            transform.eulerAngles = rotationEuler;
        }
    }
}

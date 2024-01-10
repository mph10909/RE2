using UnityEngine;

public class MultiObjectBillboarding : BillboardingWithAxisControl
{
    public Transform[] objectsToBillboard; // Array of objects to billboard

    protected override void LateUpdate()
    {
        //base.LateUpdate();

        if (target != null)
        {
            foreach (Transform objTransform in objectsToBillboard)
            {
                if (objTransform != null)
                {
                    // Calculate the direction from the object to the target
                    Vector3 toTarget = target.position - objTransform.position;

                    // Calculate the rotation with locked axis
                    Quaternion rotation = Quaternion.LookRotation(toTarget, lockedAxis);

                    // Apply the rotation to the object
                    objTransform.rotation = rotation;

                    // Apply axis rotation control
                    Vector3 rotationEuler = objTransform.eulerAngles;

                    if (!rotateX) rotationEuler.x = 0;
                    if (!rotateY) rotationEuler.y = 0;
                    if (!rotateZ) rotationEuler.z = 0;

                    objTransform.eulerAngles = rotationEuler;
                }
            }
        }
    }
}

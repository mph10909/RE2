using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class AlignBoard : MonoBehaviour
    {
        public Transform skater;
        public float groundCheckDistance;
        public Rigidbody rb;
        public float boardGroundedSpeed;

        void Start()
        {
            // If the skater and rigid body are not assigned in the Inspector, try to find them automatically.
            if (!rb) rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (IsGrounded())
            {
                //AlignWithGround();
                AlignWithGround(skater);
                AlignWithForwardDirection();
            }
        }

        public bool IsGrounded()
        {
            bool grounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance);
            Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, grounded ? Color.green : Color.red);
            return grounded;
        }

        private void AlignWithGround(Transform objectToAlign)
        {

            // Cast a ray downward to get the ground normal
            RaycastHit hit;
            Vector3 raycastOrigin = objectToAlign.position;
            Vector3 raycastDirection = -objectToAlign.up;
            Debug.DrawRay(raycastOrigin, raycastDirection, Color.blue);
            if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, groundCheckDistance))
            {
                // Calculate the rotation to align the object with the ground normal
                Quaternion targetRotation = Quaternion.FromToRotation(objectToAlign.up, hit.normal) * objectToAlign.rotation;

                // Apply the rotation using Slerp for smooth alignment
                float alignSpeed = boardGroundedSpeed; // You can adjust the alignment speed to your preference
                objectToAlign.rotation = Quaternion.Slerp(objectToAlign.rotation, targetRotation, Time.deltaTime * alignSpeed);
            }
            //else
            //{
            //    // Rotate back to horizontal when not grounded
            //    Quaternion targetRotation = Quaternion.Euler(0f, objectToAlign.rotation.eulerAngles.y, 0f);
            //    float alignSpeed = 0; // Adjust the rotation speed when not grounded
            //    objectToAlign.rotation = Quaternion.Slerp(objectToAlign.rotation, targetRotation, Time.deltaTime * alignSpeed);
            //}
        }



        private void AlignWithGround()
        {
            // Cast a ray downward to get the ground normal
            RaycastHit hit;
            Vector3 raycastOrigin = transform.position;
            Vector3 raycastDirection = -transform.up;

            if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, groundCheckDistance))
            {
                // Calculate the rotation to align the skateboard with the ground normal
                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * rb.rotation;

                // Apply the rotation using Slerp for smooth alignment
                float alignSpeed = boardGroundedSpeed; // You can adjust the alignment speed to your preference
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * alignSpeed));
            }
        }


        private void AlignWithForwardDirection()
        {
            // Get the rigid body's forward direction and use it to update the skater's rotation.
            Vector3 forwardDirection = rb.transform.forward;
            if (forwardDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(forwardDirection, Vector3.up);
                skater.rotation = Quaternion.Slerp(skater.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

    }
}

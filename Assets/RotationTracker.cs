using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class RotationTracker : MonoBehaviour
    {
        [SerializeField] HBController boardController;

        public float torqueAmount = 10f;
        [SerializeField] private int spinCount = 0; // Counter for spin count
        [SerializeField] private bool isRotating = false;
        [SerializeField] private Quaternion previousRotation;

        [SerializeField]private float accumulatedAngle = 0f;
        [SerializeField] bool canDisplaySpins;

        void Start()
        {
            previousRotation = transform.rotation;
        }

        private void Update()
        {
            if (canDisplaySpins && boardController.IsGrounded) { print(spinCount * 180); canDisplaySpins = false; }
            if (boardController.IsGrounded) { spinCount = 0; accumulatedAngle = 0; }
            float horizontalInput = Input.GetAxis("Horizontal");
            // Check if the horizontal axis is not equal to 0
            if (Mathf.Abs(horizontalInput) > 0.1f && !boardController.IsGrounded)
            {
                isRotating = true;
                canDisplaySpins = true;
                float angle = torqueAmount * Mathf.Rad2Deg * Time.deltaTime;

                // Accumulate the rotation angle
                accumulatedAngle += angle;

                // Check if the accumulated angle exceeds 180 degrees
                if (accumulatedAngle >= 180f)
                {
                    int completedSpins = Mathf.FloorToInt(accumulatedAngle / 180f);
                    spinCount += completedSpins;
                    accumulatedAngle -= completedSpins * 180f;
                }
                else if (accumulatedAngle <= -180f)
                {
                    int completedSpins = Mathf.FloorToInt(accumulatedAngle / -180f);
                    spinCount -= completedSpins;
                    accumulatedAngle += completedSpins * 180f;
                }
            }
            else { isRotating = false;  }
        }
    }
}



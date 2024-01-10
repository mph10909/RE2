using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class CarController : MonoBehaviour
    {
        const string HORIZONTAL = "Horizontal";
        const string VERTICLE = "Vertical";
        private float horizontalInput;
        private float verticalInput;
        private float currentbreakForce;
        private float currentSteerAngle;
        private float currentMotorForce;
        private bool isBreaking;
        private bool isBoosting;


        [SerializeField] float motorForce;
        [SerializeField] float boostForce;
        [SerializeField] float breakForce;
        [SerializeField] float maxSteerAngle;
        [SerializeField] float maxTiltAngle = 30f;

        [SerializeField] WheelCollider frontLeftWheel;
        [SerializeField] WheelCollider frontRightWheel;
        [SerializeField] WheelCollider rearLeftWheel;
        [SerializeField] WheelCollider rearRightWheel;

        [SerializeField] Transform frontLeftWheelTransform;
        [SerializeField] Transform frontRightWheelTransform;
        [SerializeField] Transform rearLeftWheelTransform;
        [SerializeField] Transform rearRightWheelTransform;

        [SerializeField] Transform cOG;

        [SerializeField] bool isRearWheel;
        [SerializeField] bool isFrontWheel;
        [SerializeField] bool isAllwheel;

        public Vector3 centerOfMass;
        Rigidbody rb;
        public float AntiRoll = 5000.0f;


        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = centerOfMass;
        }

        void FixedUpdate()
        {
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
            AdjustCenterOfMass();
            //Stabalizer();
        }

        private void HandleTipping()
        {
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.z = Mathf.Clamp(currentRotation.z, -maxTiltAngle, maxTiltAngle);
            transform.eulerAngles = currentRotation;
        }

        private void AdjustCenterOfMass()
        {
            rb.centerOfMass = centerOfMass;
            cOG.position = rb.centerOfMass;
        }

        void HandleMotor()
        {
            if (isAllwheel) { isFrontWheel = true;  isRearWheel = true; }

            currentMotorForce = isBoosting ? motorForce * boostForce : motorForce;

            if (isFrontWheel)
            {
            frontLeftWheel.motorTorque = verticalInput * 600 * currentMotorForce * Time.deltaTime;
            frontRightWheel.motorTorque = verticalInput * 600 * currentMotorForce * Time.deltaTime;
            }

            if (isRearWheel)
            {
                rearLeftWheel.motorTorque = verticalInput * 600 * currentMotorForce * Time.deltaTime;
                rearRightWheel.motorTorque = verticalInput * 600 * currentMotorForce * Time.deltaTime;
            }

            currentbreakForce = isBreaking ? breakForce : 0;
            ApplyBreaking();
        }

        void Stabalizer()
        {
            {
                WheelHit hit;
                float travelL = 1.0f;
                float travelR = 1.0f;

                bool groundedL = frontLeftWheel.GetGroundHit(out hit);
                if (groundedL)
                    travelL = (-frontLeftWheel.transform.InverseTransformPoint(hit.point).y - frontLeftWheel.radius) / frontLeftWheel.suspensionDistance;

                bool groundedR = frontRightWheel.GetGroundHit(out hit);
                if (groundedR)
                    travelR = (-frontRightWheel.transform.InverseTransformPoint(hit.point).y - frontRightWheel.radius) / frontRightWheel.suspensionDistance;

                float antiRollForce = (travelL - travelR) * AntiRoll;

                if (groundedL)
                    rb.AddForceAtPosition(frontLeftWheel.transform.up * -antiRollForce, frontLeftWheel.transform.position);
                if (groundedR)
                    rb.AddForceAtPosition(frontRightWheel.transform.up * antiRollForce, frontRightWheel.transform.position);
            }
        }

        private void ApplyBreaking()
        {
            frontRightWheel.brakeTorque = currentbreakForce;
            frontLeftWheel.brakeTorque = currentbreakForce;
            rearRightWheel.brakeTorque = currentbreakForce;
            rearLeftWheel.brakeTorque = currentbreakForce;
        }

        void HandleSteering()
        {
            currentSteerAngle = maxSteerAngle * horizontalInput;
            frontLeftWheel.steerAngle = currentSteerAngle;
            frontRightWheel.steerAngle = currentSteerAngle;

        }

        void GetInput()
        {
            horizontalInput = Input.GetAxis(HORIZONTAL);
            verticalInput = Input.GetAxis(VERTICLE);
            isBreaking = Input.GetKey(KeyCode.Space); // Use GetKey instead of GetKeyDown
            isBoosting = Input.GetKey(KeyCode.LeftShift);
        }


        void UpdateWheels()
        {
            UpdateSingleWheel(frontLeftWheel, frontLeftWheelTransform);
            UpdateSingleWheel(frontRightWheel, frontRightWheelTransform);
            UpdateSingleWheel(rearLeftWheel, rearLeftWheelTransform);
            UpdateSingleWheel(rearRightWheel, rearRightWheelTransform);
        }

        void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
        {
            Vector3 pos;
            Quaternion rot;
            wheelCollider.GetWorldPose(out pos, out rot);
            wheelTransform.rotation = rot;
            wheelTransform.position = pos;
        }
    }
}

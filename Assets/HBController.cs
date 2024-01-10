using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace ResidentEvilClone
{
    public class HBController : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera virtualCamera1;
        [SerializeField] CinemachineVirtualCamera virtualCamera2;

        Rigidbody rb;
        [SerializeField] bool jumpPressed;
        [SerializeField] bool isGrounded;
        [SerializeField] float multiplier;
        [SerializeField] float moveForce, turnTorque;
        [SerializeField] float jumpForce;

        [SerializeField] Transform[] anchors = new Transform[4];
        [SerializeField] Transform[] _leftAnchors = new Transform[2];
        [SerializeField] Transform[] _rightAnchors = new Transform[2];

        [SerializeField] float[] yHeights = new float[4];
        [SerializeField] float leanHeight;
        [SerializeField] float groundDistance;

        RaycastHit[] hits = new RaycastHit[4];

        [SerializeField] float moveForceIncreaseRate = 1f;
        [SerializeField] float moveForceDecreaseRate = 2f; // Adjust the rate of decrease
        [SerializeField] float moveForceMax = 4000f;
        [SerializeField] float moveForceMin = 500f; // Adjust the minimum value for moveForce

        bool isVerticalInputHeld = false;

        [SerializeField] float applyForceDuration = 2f; // Adjust the duration of applying force after input release
        [SerializeField] float applyForceMultiplier = 1f; // Adjust the force multiplier for applying force after input release
    
        [SerializeField]float applyForceTimer = 0f;

        public bool IsGrounded { get { return isGrounded; } }
        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < 4; i++)
            {
                yHeights[i] = anchors[i].localPosition.y;
            }
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            // Check for jump input
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                jumpPressed = true;
            }

            print(IsFacingUpward());
        }

        void FixedUpdate()
        {
            isGrounded = CheckGrounded();

            float turnDirection = Input.GetAxis("Horizontal");
            for (int i = 0; i < 4; i++)
            {
                ApplyForce(anchors[i], hits[i]);
            }

            if (Input.GetAxis("Vertical") > 0)
            {
                isVerticalInputHeld = true;
                applyForceTimer = applyForceDuration;
            }
            else
            {
                isVerticalInputHeld = false;
            }

            if (isGrounded)
            {
                virtualCamera1.Priority = 1;
                virtualCamera2.Priority = 0;

                // Increase moveForce gradually while vertical input is held
                if (isVerticalInputHeld)
                {
                    moveForce += moveForceIncreaseRate * Time.deltaTime; // Increase moveForce over time

                    // Clamp moveForce to a maximum value
                    moveForce = Mathf.Clamp(moveForce, 0f, moveForceMax);
                }
                else
                {
                    // Decrease moveForce gradually when the vertical input is released
                    moveForce -= moveForceDecreaseRate * Time.deltaTime; // Decrease moveForce over time

                    // Clamp moveForce to a minimum value
                    moveForce = Mathf.Clamp(moveForce, moveForceMin, moveForceMax);

                }

                rb.AddForce(Input.GetAxis("Vertical") * moveForce * transform.forward);
                rb.AddTorque(Input.GetAxis("Horizontal") * turnTorque * transform.up);
                Lean(turnDirection);
            }
            else
            {
                virtualCamera1.Priority = 0;
                virtualCamera2.Priority = 1;
                rb.AddTorque(Input.GetAxis("Horizontal") * turnTorque * 10 * transform.up);
            }

            // Apply jump force if jump button is pressed and the hoverboard is grounded
            if (jumpPressed && isGrounded)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                jumpPressed = false;
            }

            if (applyForceTimer > 0.1f && !isVerticalInputHeld)
            {
                rb.AddForce(1 * moveForce * transform.forward);
                applyForceTimer -= Time.deltaTime;
            }

            if (!isGrounded && isVerticalInputHeld)
            {
                // Add force in the direction the camera is facing
                Vector3 cameraForward = virtualCamera2.transform.forward;
                cameraForward.y = 0f; // Remove any vertical component
                cameraForward.Normalize(); // Normalize the vector to ensure consistent force magnitude

                rb.AddForce(cameraForward * moveForce);
            }


        }

        private bool IsFacingUpward()
        {
            float rotationAngle = Vector3.Angle(transform.up, Vector3.up);
            return rotationAngle >= 90f;
        }

        private bool CheckGrounded()
        {

            // Cast a ray downwards from each anchor position
            for (int i = 0; i < 4; i++)
            {
                if (Physics.Raycast(anchors[i].position, -anchors[i].up, groundDistance))
                {
                    return true; // Return true if any of the rays hit the ground
                }
            }

            return false; // Return false if none of the rays hit the ground
        }

        private void Lean(float turnDirection)
        {
            if (turnDirection < 0.75f && turnDirection > -0.75f)
            {
                for (int i = 0; i < 4; i++)
                {
                    anchors[i].localPosition = new Vector3(anchors[i].localPosition.x, yHeights[i], anchors[i].localPosition.z);
                }
            }
            if (turnDirection > .75)
            {
                for (int i = 0; i < _rightAnchors.Length; i++)
                {
                    _leftAnchors[i].localPosition = new Vector3(_leftAnchors[i].localPosition.x, yHeights[i], _leftAnchors[i].localPosition.z);
                    _rightAnchors[i].localPosition = new Vector3(_rightAnchors[i].localPosition.x, leanHeight, _rightAnchors[i].localPosition.z);
                }
            }
            if (turnDirection < -.75)
            {
                for (int i = 0; i < _leftAnchors.Length; i++)
                {
                    _rightAnchors[i].localPosition = new Vector3(_rightAnchors[i].localPosition.x, yHeights[i], _rightAnchors[i].localPosition.z);
                    _leftAnchors[i].localPosition = new Vector3(_leftAnchors[i].localPosition.x, leanHeight, _leftAnchors[i].localPosition.z);
                }
            }

        }

        void ApplyForce(Transform anchor, RaycastHit hit)
        {
            if(Physics.Raycast(anchor.position, -anchor.up, out hit))
            {
                float force = 0;
                force = Mathf.Abs(1 / (hit.point.y - anchor.position.y));
                rb.AddForceAtPosition(transform.up * force * multiplier, anchor.position, ForceMode.Acceleration);
            }
        }
    }
} 
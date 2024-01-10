using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ResidentEvilClone
{
    public class MoveBoard : MonoBehaviour
    {
        [SerializeField]
        GameObject SwitchUI;
        Rigidbody rb;
        public OllieTest ollie;
        public float quarterPipeAngle = 60f;
        public float turnForce;
        public float speed = 5;
        float h, v;
        public float groundDistance = 0.5f;
        public float decelerationRate;
        public bool switchStance;

        public bool isRotating = false;
        public bool calculate;
        public Quaternion previousRotation;
        public float totalRotation = 0f;
        public int rotationCounter = 0;

        public bool initialSwitchStance;
        private Vector3 savedForwardVelocity;
        public bool wasGrounded;
        public bool justLanded;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                switchStance = !switchStance;
            }
            CheckInput();
        }

        void FixedUpdate()
        {
            MoveForward();
            RotateBoard();
            RotationCounter();
        }

        private void RotationCounter()
        {
            Rigidbody rb = ollie.GetComponent<Rigidbody>();
            if (!isRotating && !ollie.IsGrounded())
            {
                calculate = true;
                isRotating = true;
                previousRotation = rb.rotation;
                totalRotation = 0f;
            }

            if (calculate && ollie.IsGrounded())
            {
                isRotating = false;
                calculate = false;
                if (!switchStance && rotationCounter % 2 != 0)
                {
                    switchStance = true;
                    SwitchUI.SetActive(true);
                }
                else if (switchStance && rotationCounter % 2 != 0)
                {
                    switchStance = false;
                    SwitchUI.SetActive(false);
                }
                rotationCounter = 0;
            }

            if (isRotating)
            {
                Quaternion currentRotation = rb.rotation;
                Quaternion rotationDelta = currentRotation * Quaternion.Inverse(previousRotation);
                float angleDelta = rotationDelta.eulerAngles.y;

                if (angleDelta > 180f)
                {
                    angleDelta -= 360f;
                }
                else if (angleDelta < -180f)
                {
                    angleDelta += 360f;
                }

                totalRotation += angleDelta;

                if (Mathf.Abs(totalRotation) >= 180)
                {
                    rotationCounter++;
                    totalRotation -= Mathf.Sign(totalRotation) * 180f;
                }

                previousRotation = currentRotation;
            }
        }

        private void MoveForward()
        {
            Vector3 forwardVelocity;
            if (!switchStance)
            {
                forwardVelocity = transform.forward * speed;
            }
            else
            {
                forwardVelocity = -transform.forward * speed;
            }

            bool isGrounded = Grounded;

            if (v > 0.1f)
            {
                if (isGrounded && !ollie.PerformingOllie)
                {
                    if (!wasGrounded)
                    {
                        savedForwardVelocity = forwardVelocity;
                    }
                    rb.velocity = new Vector3(forwardVelocity.x, rb.velocity.y, forwardVelocity.z);
                }
                else if (ollie.PerformingOllie)
                {
                    Vector3 rotationTorque = Vector3.up * h * turnForce;
                    rb.AddTorque(rotationTorque, ForceMode.Acceleration);
                    rb.velocity = new Vector3(savedForwardVelocity.x, rb.velocity.y, savedForwardVelocity.z);
                }
            }
            else
            {
                rb.velocity -= rb.velocity * decelerationRate * Time.deltaTime;
                if (rb.velocity.magnitude < 0.01f)
                {
                    rb.velocity = Vector3.zero;
                }
            }
        }

        private void RotateBoard()
        {
            float rotateAmount;
            if (XAngle() > quarterPipeAngle)
            {
                rotateAmount = h * (turnForce * 2);
            }
            else
            {
                rotateAmount = h * turnForce;
            }

            Vector3 rotationTorque = Vector3.up * rotateAmount;
            rb.AddTorque(rotationTorque, ForceMode.Acceleration);
        }

        private void CheckInput()
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }

        public float XAngle()
        {
            float normalizedRotationX = rb.rotation.eulerAngles.x % 360f;
            if (normalizedRotationX > 180f)
                normalizedRotationX -= 360f;

            float xAngle = Mathf.Abs(normalizedRotationX);
            return xAngle;
        }

        public bool Grounded
        {
            get { return IsGrounded(); }
        }

        private bool IsGrounded()
        {
            bool grounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundDistance);
            Debug.DrawRay(transform.position, Vector3.down * 0.1f, grounded ? Color.green : Color.red);
            return grounded;
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 130, 200, 30), "Switch: " + switchStance.ToString());
        }
    }
}

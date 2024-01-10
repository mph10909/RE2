using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class OllieTest : MonoBehaviour
    {
        public Transform skater;
        public float ollieVelocity = 5f;
        public Vector3 velocity;
        public float maxJumpTime = 2.0f;
        public float minJumpVelocity = 5f;
        public float maxJumpVelocity = 15f;
        public float groundCheckDistance = 0.1f;
        private float jumpTime;
        public float pullDownVelocity = -5f;
        public float quarterPipeAngle = 60f;
        float h, v;

        [SerializeField]private bool isPerformingOllie = false;

        public bool PerformingOllie { get { return isPerformingOllie; } }
        public bool justLanded;

        private void Update()
        {
            velocity = GetComponent<Rigidbody>().velocity;
            CheckInput();
            OllieControls();
            DebugGizmos();
        }

        private void DebugGizmos()
        {
            Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, IsGrounded() ? Color.green : Color.red);
        }

        private void OllieControls()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpTime = 0f;
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                if (jumpTime < maxJumpTime)
                    jumpTime += Time.deltaTime;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                PerformOllie();
            }
        }

        private void PerformOllie()
        {
            if (!IsGrounded() || isPerformingOllie) return;

            isPerformingOllie = true;

            float startingYVelocity = jumpTime * (maxJumpVelocity - minJumpVelocity) / maxJumpTime + minJumpVelocity;
            startingYVelocity = Mathf.Clamp(startingYVelocity, minJumpVelocity, maxJumpVelocity);

            StartCoroutine(DecreaseYVelocity(startingYVelocity));
        }

        private IEnumerator DecreaseYVelocity(float startingYVelocity)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = new Vector3(rb.velocity.x, startingYVelocity, rb.velocity.z);

            while (rb.velocity.y > 0.1f)
            {
                rb.velocity -= Vector3.up * Time.deltaTime * ollieVelocity;
                yield return null;
            }

            rb.velocity = new Vector3(rb.velocity.x, pullDownVelocity, rb.velocity.z);

            isPerformingOllie = false;
            StopAllCoroutines();
        }

        private void CheckInput()
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }

        public bool IsGrounded()
        {
            bool grounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance);
            Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, grounded ? Color.green : Color.red);
            return grounded;
        }

        private bool CanLaunch()
        {
            float normalizedRotationX = skater.rotation.eulerAngles.x % 360f;
            if (normalizedRotationX > 180f)
                normalizedRotationX -= 360f;

            float xAngle = Mathf.Abs(normalizedRotationX);

            return xAngle > quarterPipeAngle;
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 20), "Velocity X: " + velocity.x.ToString() + " Velocity Y: " + velocity.y.ToString());

            GUI.Label(new Rect(10, 30, 200, 30), "Horizontal: " + h.ToString() + " Vertical: " + v.ToString());

            GUI.Label(new Rect(10, 50, 200, 30), "Rotation X: " + XAngle().ToString("F2"));

            GUI.Label(new Rect(10, 70, 200, 30), "Is Grounded: " + IsGrounded().ToString());

            GUI.Label(new Rect(10, 90, 200, 30), "Quarter Pipe Launch: " + CanLaunch().ToString());

            GUI.Label(new Rect(10, 110, 200, 30), "Ollie: " + isPerformingOllie.ToString());
        }

        public float XAngle()
        {
            float normalizedRotationX = skater.rotation.eulerAngles.x % 360f;
            if (normalizedRotationX > 180f)
                normalizedRotationX -= 360f;

            float xAngle = Mathf.Abs(normalizedRotationX);
            return xAngle;
        }
    }
}

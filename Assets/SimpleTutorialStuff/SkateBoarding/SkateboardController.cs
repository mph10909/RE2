using UnityEngine;
using Cinemachine;
using System.Collections;
using System;

public class SkateboardController : MonoBehaviour
{
    public CinemachineVirtualCamera groundCam;
    public CinemachineVirtualCamera airCam;
    public float forwardSpeed = 10f;
    public float backwardSpeed = 5f;
    public float acceleration = 5f;
    public float deceleration = 10f;
    public float turnForce = 10f;
    public float ollieForce = 10f;
    public float grindForce = 5f;
    public float grindTiltThreshold = 30f;
    public float forwardRayHeight = 0.5f;
    public float frontRayLength = 2.0f;
    public float middleRayLength = 1.5f;
    public float rearRayLength = 2.0f;
    public float forwardRayLength = 2.0f;
    public LayerMask grindableSurfaceMask;

    private Rigidbody rb;
    public bool isAccelerating = false;
    private bool isJumping = false;
    private bool isGrinding = false;
    public bool isGrounded = true;
    public float groundCheckDistance;
    public float downForce;
    public float quarterPipeAngle = 70;
    public bool perfomedLaunch;
    float rotateAmount;
    float moveVertical;
    public float fallSpeed = 0.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (NotLaunched()) perfomedLaunch = false; 
        isGrounded = IsGrounded();

        if (IsGrounded())
        {
            moveVertical = Input.GetAxis("Vertical");
            if (moveVertical > 0f)
            {
                isAccelerating = true;
            }
            else if (moveVertical <= 0f)
            {
                isAccelerating = false;
            }
        }


        if (Input.GetKeyUp(KeyCode.Space) && IsGrounded())
        {
            if(IsGrounded() && !CanLaunch())
            {
                PerformOllie();
            }

            if(IsGrounded() && CanLaunch())
            {
                perfomedLaunch = true;
                PerformLaunch();
            }
        }

        if (moveVertical == 0f)
        {

            rb.velocity -= rb.velocity * deceleration * Time.deltaTime;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 rotationAxis = Vector3.up;
        
        if (IsGrounded())
        {
            groundCam.Priority = 1;
            airCam.Priority = 0;
            rotateAmount = moveHorizontal * turnForce;
        }
        else
        {
            groundCam.Priority = 0;
            airCam.Priority = 1;
            groundCam.transform.position = airCam.transform.position;
            rotateAmount = moveHorizontal * turnForce * 10;
        }

        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotationAxis * rotateAmount));
    }

    private void FixedUpdate()
    {
        WeighDown();
        // Calculate the speed based on the acceleration value
        float speed = isAccelerating && IsGrounded() ? forwardSpeed : backwardSpeed; // Apply acceleration only if grounded

        // Apply acceleration if the forward key is held down
        if (isAccelerating && IsGrounded())
        {
            speed += acceleration;
        }

        if (isGrounded)
        {
            Vector3 movement = transform.forward * speed;
            rb.velocity = movement;
            RollBackOver();
        }

        AlignWithGround();
        RotateOnObstacleHit();
    }

    private void WeighDown()
    {
        if (!perfomedLaunch)
        {
            var gravityVector = new Vector3(0, downForce * 100f, 0);
            rb.AddRelativeForce(gravityVector * Time.deltaTime, ForceMode.Acceleration);
        }
        if (perfomedLaunch)
        {
            
            //var gravityVector = new Vector3(downForce * 10f, 0, 0);
            //rb.AddRelativeForce(gravityVector * Time.deltaTime, ForceMode.Acceleration);
        }
    }

    private void OnGUI()
    {
        // Create a debug menu to display the rigidbody's velocity on screen
        GUI.Label(new Rect(10, 10, 200, 30), "Velocity X: " + rb.velocity.x.ToString("F2") + " Velocity Y: " + rb.velocity.y.ToString("F2"));
        // Calculate the normalized rotation around the X-axis
        float normalizedRotationX = rb.rotation.eulerAngles.x % 360f;
        if (normalizedRotationX > 180f)
            normalizedRotationX -= 360f;

        float xAngle = Mathf.Abs(normalizedRotationX);

        // Create a label to display the normalized rotation around the X-axis
        GUI.Label(new Rect(10, 30, 200, 30), "Rotation X: " + xAngle.ToString("F2"));

        // Check if the skateboard can launch
        bool canLaunch = CanLaunch();

        GUI.Label(new Rect(10, 50, 200, 30), "Can Launch Quarter Pipe: " + canLaunch.ToString());

        GUI.Label(new Rect(10, 70, 200, 30), "Grounded: " + IsGrounded().ToString());

        GUI.Label(new Rect(10, 90, 200, 30), "Not Launched: " + NotLaunched().ToString());

        bool isFlippedOver = IsFlippedOver();

        if (isFlippedOver)
        {
            GUI.Label(new Rect(10, 110, 200, 30), "Skateboard is Flipped Over");
        }
        else
        {
            GUI.Label(new Rect(10, 110, 200, 30), "Skateboard is Upright");
        }
    }

    private bool IsFlippedOver()
    {
        // Calculate the normalized rotation around the X-axis
        float normalizedRotationX = rb.rotation.eulerAngles.x % 360f;
        if (normalizedRotationX > 180f)
            normalizedRotationX -= 360f;

        float normalizedRotationY = rb.rotation.eulerAngles.y % 360f;
        if (normalizedRotationY > 180f)
            normalizedRotationY -= 360f;

        // Check if the skateboard is flipped over by looking at its X-axis rotation
        // You can adjust the threshold angle (e.g., 90 degrees) based on your specific case
        return Mathf.Abs(normalizedRotationX) > 90f || Mathf.Abs(normalizedRotationY) > 90f;
    }

    private void PerformOllie()
    {
        rb.AddForce(Vector3.up * ollieForce, ForceMode.Impulse);
    }

    //void PerformLaunch()
    //{
    //    // Calculate the desired vertical rotation around the X axis based on the ramp's orientation
    //    float rampAngle = transform.eulerAngles.x;
    //    float verticalRotationX = rampAngle >= 0f ? 90f : -90f;

    //    // Get the original Z-axis rotation
    //    float originalZRotation = transform.eulerAngles.z;

    //    // Set the rigidbody's rotation to the target vertical rotation while keeping the original Z rotation
    //    rb.MoveRotation(Quaternion.Euler(-verticalRotationX, transform.eulerAngles.y, originalZRotation));

    //    rb.AddForce(Vector3.up * ollieForce, ForceMode.Acceleration);
    //    // Apply an upward force to make the skateboard go up
    //    rb.AddForce(transform.forward * 100, ForceMode.VelocityChange);

    //    rb.velocity = new Vector3(0, fallSpeed, 0);
    //}

    void PerformLaunch()
    {
        // Calculate the desired vertical rotation around the X axis based on the ramp's orientation
        float rampAngle = transform.eulerAngles.x;
        float verticalRotationX = rampAngle >= 0f ? 90f : -90f;

        // Get the original Z-axis rotation
        float originalZRotation = transform.eulerAngles.z;

        // Set the rigidbody's rotation to the target vertical rotation while keeping the original Z rotation
        rb.MoveRotation(Quaternion.Euler(-verticalRotationX, transform.eulerAngles.y, originalZRotation));

        rb.AddForce(Vector3.up * ollieForce, ForceMode.Acceleration);

        // Gradually increase the y-velocity until it reaches a certain amount
        float targetYVelocity = 10f; // Adjust this value as needed
        StartCoroutine(IncreaseYVelocity(targetYVelocity));
    }

    private IEnumerator IncreaseYVelocity(float targetYVelocity)
    {
        while (rb.velocity.y < targetYVelocity)
        {
            rb.velocity += Vector3.up * Time.deltaTime * 5f; // Increase y-velocity gradually
            yield return null;
        }

        // Negate the y-velocity and make it go back to zero
        while (rb.velocity.y > 0f)
        {
            rb.velocity -= Vector3.up * Time.deltaTime * 5f; // Decrease y-velocity gradually
            yield return null;
        }

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Set y-velocity to zero
    }

    private void AlignWithGround()
    {
        if (perfomedLaunch) return;
        // Cast a ray downward to get the ground normal
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        Vector3 raycastDirection = -transform.up;

        if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, groundCheckDistance))
        {
            // Calculate the rotation to align the skateboard with the ground normal
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * rb.rotation;

            // Apply the rotation using Slerp for smooth alignment
            float alignSpeed = 5f; // You can adjust the alignment speed to your preference
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * alignSpeed));
        }
        else
        {
            // Rotate back to horizontal when not grounded
            Quaternion targetRotation = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 0f);
            float alignSpeed = 5f; // Adjust the rotation speed when not grounded
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * alignSpeed));
        }
    }


    private void RotateOnObstacleHit()
    {
        float forwardRayLength = 1.0f; // Adjust the ray length as needed
        Vector3 raycastOrigin = transform.position + transform.up * forwardRayHeight;
        Vector3 raycastDirection = transform.forward;

        // Visualize the forward-facing ray
        Debug.DrawRay(raycastOrigin, raycastDirection * forwardRayLength, Color.yellow);

        if (Physics.Raycast(raycastOrigin, raycastDirection, out RaycastHit hit, forwardRayLength))
        {
            // Rotate left or right by 45 degrees when an obstacle is hit
            Vector3 rotationAxis = Vector3.up;
            float rotateAmount = 45f;

            if (UnityEngine.Random.Range(0, 2) == 0) // Randomly choose left or right rotation
            {
                rotationAxis *= -1f;
            }

            // Apply the rotation directly to the rigidbody
            rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles + rotationAxis * rotateAmount));
        }
    }

    private bool IsGrounded()
    {

        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        Vector3 raycastDirection = -transform.up;

        if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, groundCheckDistance))
        {
            Debug.DrawRay(raycastOrigin, raycastDirection * groundCheckDistance, Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(raycastOrigin, raycastDirection * groundCheckDistance, Color.red);
            return false;
        }
    }

    private bool CanLaunch()
    {
        Vector3 raycastOriginFront = transform.position + transform.up * forwardRayHeight + transform.forward * 0.5f;
        Vector3 raycastOriginMiddle = transform.position + transform.up * forwardRayHeight;
        Vector3 raycastOriginRear = transform.position + transform.up * forwardRayHeight - transform.forward * 0.5f;
        Vector3 raycastDirection = -transform.up; // Cast rays downward
        Vector3 forwardRaycastDirection = transform.forward; // Cast forward ray

        RaycastHit hitFront, hitMiddle, hitRear, hitForward;

        bool hitMiddleGround = Physics.Raycast(raycastOriginMiddle, raycastDirection, out hitMiddle, middleRayLength);
        bool hitRearGround = Physics.Raycast(raycastOriginRear, raycastDirection, out hitRear, rearRayLength);

        Debug.DrawRay(raycastOriginMiddle, raycastDirection * middleRayLength, Color.blue);
        Debug.DrawRay(raycastOriginRear, raycastDirection * rearRayLength, Color.blue);

        // Set the adjustable forward ray distance
         // You can change this distance to your preference
        bool hitForwardObstacle = Physics.Raycast(raycastOriginFront, forwardRaycastDirection, out hitForward, forwardRayLength);

        Debug.DrawRay(raycastOriginFront, forwardRaycastDirection * forwardRayLength, Color.cyan);

        // Calculate the normalized rotation around the X-axis
        float normalizedRotationX = rb.rotation.eulerAngles.x % 360f;
        if (normalizedRotationX > 180f)
            normalizedRotationX -= 360f;

        // Calculate the absolute value of the normalized rotation around the X-axis (xAngle)
        float xAngle = Mathf.Abs(normalizedRotationX);

        // Check if the middle and rear raycasts hit the ground, the forward raycast doesn't hit an obstacle,
        // and the xAngle is greater than 70
        return hitMiddleGround && hitRearGround && !hitForwardObstacle && xAngle > quarterPipeAngle;
    }

    private bool NotLaunched()
    {
        Vector3 raycastOriginFront = transform.position + transform.up * forwardRayHeight + transform.forward * 0.5f;
        Vector3 raycastOriginMiddle = transform.position + transform.up * forwardRayHeight;
        Vector3 raycastOriginRear = transform.position + transform.up * forwardRayHeight - transform.forward * 0.5f;
        Vector3 raycastDirection = -transform.up;

        RaycastHit hitMiddle, hitRear, hitFront;

        bool hitFrontGround = Physics.Raycast(raycastOriginFront, raycastDirection, out hitFront, frontRayLength);
        bool hitMiddleGround = Physics.Raycast(raycastOriginMiddle, raycastDirection, out hitMiddle, middleRayLength);
        bool hitRearGround = Physics.Raycast(raycastOriginRear, raycastDirection, out hitRear, rearRayLength);

        Debug.DrawRay(raycastOriginFront, raycastDirection * frontRayLength, Color.blue);

        return hitFrontGround && hitMiddleGround && hitRearGround;
    }

    private void RollBackOver()
    {
        // Check if the skateboard is flipped over (upside down)
        bool isFlippedOver = false;

        // Calculate the normalized rotation around the X-axis
        float normalizedRotationX = rb.rotation.eulerAngles.x % 360f;
        if (normalizedRotationX > 180f)
            normalizedRotationX -= 360f;

        // Check if the skateboard is flipped over by looking at its X-axis rotation
        // You can adjust the threshold angle (e.g., 90 degrees) based on your specific case
        if (Mathf.Abs(normalizedRotationX) > 90f)
        {
            isFlippedOver = true;
        }

        if (isFlippedOver)
        {
            // Calculate the corrective rotation to bring the skateboard back upright
            Quaternion targetRotation = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 0f);

            // Calculate the torque force needed to apply the corrective rotation
            Vector3 torque = (targetRotation * Vector3.up - transform.up).normalized * turnForce * 10f;

            // Apply the corrective torque force to make the skateboard roll back over
            rb.AddTorque(torque, ForceMode.Force);
        }
    }

}

using UnityEngine;using System.Collections.Generic;

public class MovingObject : MonoBehaviour
{
    public SplineSystem splineSystem;
    public float speed = 2.0f;
    public float lerpSpeed = 2.0f;

    public int currentSegmentIndex = 0;
    private float t = 0.0f;
    public bool movingForward = true;
    int segmentCount;

    public bool isRotating;
    
    void Awake()
    {
        this.transform.position = splineSystem.waypoints[0].position;
        this.transform.rotation = Quaternion.EulerRotation(splineSystem.waypoints[0].eulerAngles);
    }

    private void Update()
    {
            MoveAlongSpline();
    }

    private void MoveAlongSpline()
    {
        if (splineSystem == null || splineSystem.waypoints.Length == 0)
            return;


        segmentCount = splineSystem.waypoints.Length;

        Vector3 p0 = splineSystem.waypoints[currentSegmentIndex].position;
        Vector3 p1 = p0 + splineSystem.waypoints[currentSegmentIndex].forward * 1.0f;
        Vector3 p2 = splineSystem.waypoints[(currentSegmentIndex + 1) % segmentCount].position - splineSystem.waypoints[(currentSegmentIndex + 1) % segmentCount].forward * 1.0f;
        Vector3 p3 = splineSystem.waypoints[(currentSegmentIndex + 1) % segmentCount].position;

        if (!isRotating)
        {
        float distance = Vector3.Distance(p0, p3);
        float direction = movingForward ? 1.0f : -1.0f;

        t += Time.deltaTime * speed * direction / distance;
        if (t > 1.0f || t < 0.0f)
        {
            t = movingForward ? 0.0f : 1.0f;

            if (splineSystem.loopCurve)
            {
                if (movingForward)
                {
                    currentSegmentIndex = (currentSegmentIndex + 1) % segmentCount;
                    Debug.Log("Hit waypoint: " + currentSegmentIndex); // Debug message for hitting waypoint
                }
                else
                {
                    currentSegmentIndex = (currentSegmentIndex - 1 + segmentCount) % segmentCount;
                    Debug.Log("Hit waypoint: " + currentSegmentIndex); // Debug message for hitting waypoint
                }

            }
            else
            {
                if (movingForward)
                {
                    currentSegmentIndex = (currentSegmentIndex + 1) % segmentCount;
                    Debug.Log("Hit waypoint: " + currentSegmentIndex); // Debug message for hitting waypoint

                    if (currentSegmentIndex == splineSystem.waypoints.Length - 1)
                    {
                        Debug.Log("Reached the last point!");
                        ToggleDirection(); // Disable the script to stop movement
                        isRotating = true;
                        return;
                    }
                }
                else
                {
                    currentSegmentIndex = (currentSegmentIndex - 1 + segmentCount) % segmentCount;
                    Debug.Log("Hit waypoint: " + currentSegmentIndex); // Debug message for hitting waypoint
                    if (currentSegmentIndex == splineSystem.waypoints.Length - 1)
                    {
                        Debug.Log("Reached the first point!");
                        ToggleDirection(); // Disable the script to stop movement
                        isRotating = true;
                        return;
                    }
                }
            }
        }


        Vector3 lerpPosition = splineSystem.CalculateCubicBezierPoint(p0, p1, p2, p3, t);

        transform.position = Vector3.Lerp(transform.position, lerpPosition, Time.deltaTime * lerpSpeed);

        }


        // Calculate the direction the object should face
        Vector3 splineDirection = splineSystem.CalculateCubicBezierPoint(p0, p1, p2, p3, t + 0.01f) - transform.position;
        if (splineDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(splineDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * lerpSpeed);
        }
    }

    public void ToggleDirection()
    {
        movingForward = !movingForward;
    }
}

using UnityEngine;

public class BezierFollower : MonoBehaviour
{
    public SplineSystem splineSystem; // Reference to the spline system script
    public float speed = 2.0f; // Movement speed of the follower

    private int currentWaypointIndex = 0;
    private float t = 0.0f;
    private bool movingForward = true;

    private void Start()
    {
        if (splineSystem.waypoints.Length > 0)
            transform.position = splineSystem.waypoints[0].position;
    }

    private void Update()
    {
        if (splineSystem.waypoints.Length == 0)
            return;

        Vector3 currentWaypoint = splineSystem.waypoints[currentWaypointIndex].position;

        // Calculate the next position using the cubic bezier curve function
        Vector3 nextPosition = splineSystem.CalculateCubicBezierPoint(
            currentWaypoint,
            currentWaypoint + splineSystem.waypoints[currentWaypointIndex].forward * 1.0f,
            splineSystem.waypoints[(currentWaypointIndex + 1) % splineSystem.waypoints.Length].position - splineSystem.waypoints[(currentWaypointIndex + 1) % splineSystem.waypoints.Length].forward * 1.0f,
            splineSystem.waypoints[(currentWaypointIndex + 1) % splineSystem.waypoints.Length].position,
            t
        );

        // Move the follower towards the next position
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);

        // Rotate the follower to face the direction of the next waypoint
        Vector3 lookDirection = nextPosition - transform.position;
        if (lookDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        // Update the parameter t based on the movement speed
        t += speed / Vector3.Distance(transform.position, nextPosition) * Time.deltaTime;

        // Check if the follower reached the next waypoint
        if (t >= 1.0f)
        {
            t = 0.0f; // Reset t

            // Move to the next waypoint or turn around if at the last waypoint
            if (currentWaypointIndex < splineSystem.waypoints.Length - 1 && movingForward)
            {
                currentWaypointIndex++;
            }
            else if (currentWaypointIndex > 0 && !movingForward)
            {
                currentWaypointIndex--;
            }
            else
            {
                movingForward = !movingForward; // Change direction
            }
        }
    }
}

using UnityEngine;
using Cinemachine;
using System.Collections;

public class MoveDollyBody : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineSmoothPath dollyPath;
    public float speed = 1.0f;

    private int currentWaypointIndex = 0;

    private void Start()
    {
        // Set the virtual camera's initial position to the first waypoint.
        if (dollyPath.m_Waypoints.Length > 0)
        {
            virtualCamera.transform.position = dollyPath.m_Waypoints[0].position;
        }
    }

    private void Update()
    {
        // Check for user input to move the virtual camera.
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToPreviousWaypoint();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveToNextWaypoint();
        }
    }

    private void MoveToNextWaypoint()
    {
        if (dollyPath.m_Waypoints.Length == 0)
            return;

        currentWaypointIndex++;
        if (currentWaypointIndex >= dollyPath.m_Waypoints.Length)
        {
            currentWaypointIndex = 0; // Loop back to the first waypoint.
        }

        Vector3 targetPosition = dollyPath.m_Waypoints[currentWaypointIndex].position;
        StartCoroutine(MoveCameraToPosition(targetPosition));
    }

    private void MoveToPreviousWaypoint()
    {
        if (dollyPath.m_Waypoints.Length == 0)
            return;

        currentWaypointIndex--;
        if (currentWaypointIndex < 0)
        {
            currentWaypointIndex = dollyPath.m_Waypoints.Length - 1; // Loop to the last waypoint.
        }

        Vector3 targetPosition = dollyPath.m_Waypoints[currentWaypointIndex].position;
        StartCoroutine(MoveCameraToPosition(targetPosition));
    }

    private IEnumerator MoveCameraToPosition(Vector3 targetPosition)
    {
        float journeyLength = Vector3.Distance(virtualCamera.transform.position, targetPosition);
        float startTime = Time.time;
        float distanceCovered = 0.0f;

        while (distanceCovered < journeyLength)
        {
            float fractionOfJourney = distanceCovered / journeyLength;
            virtualCamera.transform.position = Vector3.Lerp(virtualCamera.transform.position, targetPosition, fractionOfJourney * speed);
            distanceCovered = (Time.time - startTime) * speed;
            yield return null;
        }

        virtualCamera.transform.position = targetPosition;
    }
}

//public class MoveDollyBody : MonoBehaviour
//{
//    public CinemachineVirtualCamera virtualCamera;
//    public CinemachineSmoothPath dollyPath;
//    public float increaseSpeed = 1.0f;

//    private CinemachineTrackedDolly dolly;

//    private void Start()
//    {
//        // Get the Cinemachine Tracked Dolly component attached to the Cinemachine Virtual Camera.
//        dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();

//        // Start increasing the position.
//        //StartCoroutine(IncreaseDollyPosition());

//        print(dollyPath.m_Waypoints.Length);

//    }

//    private IEnumerator IncreaseDollyPosition()
//    {
//        while (true)
//        {
//            // Increase the position by the specified speed.
//            dolly.m_PathPosition += increaseSpeed * Time.deltaTime;

//            // Wait for the next frame.
//            yield return null;
//        }
//    }
//}

using UnityEngine;

public class SplineSystem : MonoBehaviour
{
    public bool loopCurve = false;
    public int curveResolution = 20;
    public float sphereSize = .5F;
    public Transform[] waypoints;

    public Color waypointColor = Color.blue;
    public Color segmentColor = Color.red;

    private void OnDrawGizmos()
    {
        waypoints = GetChildWaypoints();

        Gizmos.color = waypointColor; // Use specified waypoint color
        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.DrawWireSphere(waypoints[i].position, sphereSize);
        }

        int segmentCount = loopCurve ? waypoints.Length : waypoints.Length - 1;

        for (int i = 0; i < segmentCount; i++)
        {
            int nextIndex = (i + 1) % waypoints.Length; // Account for looping
            Vector3 p0 = waypoints[i].position;
            Vector3 p1 = waypoints[i].position + waypoints[i].forward * 1.0f; // Use waypoint's forward direction
            Vector3 p2 = waypoints[nextIndex].position - waypoints[nextIndex].forward * 1.0f; // Use waypoint's forward direction
            Vector3 p3 = waypoints[nextIndex].position;

            Vector3 lastPoint = p0;
            for (int t = 1; t <= curveResolution; t++)
            {
                Gizmos.color = waypointColor;
                float normalizedT = t / (float)curveResolution;
                Vector3 curvePoint = CalculateCubicBezierPoint(p0, p1, p2, p3, normalizedT);
                Gizmos.DrawSphere(curvePoint, 0.05f);
                Gizmos.color = segmentColor;
                Gizmos.DrawLine(lastPoint, curvePoint);
                lastPoint = curvePoint;
            }
        }

        //Quadratic Bezier Curve//
        //for (int i = 0; i < segmentCount; i++)
        //{
        //    int nextIndex = (i + 1) % waypoints.Length; // Account for looping
        //    Vector3 p0 = waypoints[i].position;
        //    Vector3 p1 = waypoints[i].position + waypoints[i].forward * 1.0f; // Use waypoint's forward direction
        //    Vector3 p2 = waypoints[nextIndex].position;

        //    Vector3 lastPoint = p0;
        //    for (int t = 1; t <= curveResolution; t++)
        //    {
        //        Gizmos.color = waypointColor;
        //        float normalizedT = t / (float)curveResolution;
        //        Vector3 curvePoint = CalculateQuadraticBezierPoint(p0, p1, p2, normalizedT);
        //        Gizmos.DrawSphere(curvePoint, 0.05f);
        //        Gizmos.color = segmentColor;
        //        Gizmos.DrawLine(lastPoint, curvePoint);
        //        lastPoint = curvePoint;
        //    }
        //}

    }

    public Vector3 CalculateCubicBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

    private Vector3 CalculateQuadraticBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }


    public Transform[] GetChildWaypoints()
    {
        Transform[] childTransforms = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childTransforms[i] = transform.GetChild(i);
        }
        return childTransforms;
    }
}

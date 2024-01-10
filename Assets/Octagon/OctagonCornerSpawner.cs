using UnityEngine;

public class OctagonCornerSpawner : MonoBehaviour
{
    // Inspector Variables
    public GameObject cornerPrefab;
    public int numberOfCorners = 8;
    public float radius = 5f;
    public LineRenderer lineRendererPrefab;
    public float maxAdjustDistance = 10f;
    public float spawnDistance = 1f;
    public bool castUpward = false;
    public bool updateMode;
    public float heightToCollider = 0;
    public Color newColor;
    public float gapDistance = .5f;
    public float halfWidth = 0.5f;

    // Private Variables
    private GameObject cornersParent;


    // Unity Callbacks
    private void Update()
    {
        if (updateMode)
        {
            ResetAndSpawnCorners();
        }
    }

    private void OnEnable()
    {
        ResetAndSpawnCorners();
    }

    private void OnDisable()
    {
        DestroyCorners();
    }

    // Helper Methods
    private void ResetAndSpawnCorners()
    {
        DestroyCorners();
        AdjustToNearestCollider(heightToCollider);
        SpawnCorners();
    }

    private void DestroyCorners()
    {
        if (cornersParent != null)
        {
            Destroy(cornersParent);
        }
    }

    private void AdjustToNearestCollider(float desiredDistance)
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        float maxDistance = 200f;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Vector3 hitPoint = hit.point;
            Vector3 newPosition = hitPoint + Vector3.up * 5;
            transform.position = newPosition;
        }
        else
        {
            transform.position = transform.position - Vector3.up * (transform.position.y - 5);
        }
    }

    private void SpawnCorners()
    {
        cornersParent = new GameObject("CornersParent");
        cornersParent.transform.SetParent(transform);

        float angleIncrement = 360f / numberOfCorners;
        Vector3[] cornerPositions = new Vector3[numberOfCorners];

        for (int i = 0; i < numberOfCorners; i++)
        {
            float angle = i * angleIncrement;
            Vector3 spawnPosition = transform.position + Quaternion.Euler(0, angle, 0) * Vector3.right * radius;
            Vector3 raycastDirection = castUpward ? Vector3.up : Vector3.down;
            RaycastHit hit;

            if (Physics.Raycast(spawnPosition, raycastDirection, out hit, maxAdjustDistance) && hit.distance <= maxAdjustDistance)
            {
                spawnPosition = hit.point - raycastDirection * spawnDistance;
            }

            GameObject corner = Instantiate(cornerPrefab, spawnPosition, Quaternion.identity, cornersParent.transform);
            cornerPositions[i] = spawnPosition;

            Renderer cornerRenderer = corner.GetComponentInChildren<Renderer>();
            Material cornerMaterial = cornerRenderer.material;
            cornerMaterial.SetColor("_EmissionColor", newColor);

            if (i > 0)
            {
                ConnectCorners(cornerPositions[i - 1], cornerPositions[i]);
            }
        }

        ConnectCorners(cornerPositions[numberOfCorners - 1], cornerPositions[0]);
    }

    private void ConnectCorners(Vector3 fromPosition, Vector3 toPosition)
    {
        LineRenderer lineRenderer = Instantiate(lineRendererPrefab, cornersParent.transform);
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, fromPosition);
        lineRenderer.SetPosition(1, toPosition);
        lineRenderer.material.SetColor("_EmissionColor", newColor);

        MeshCollider meshCollider = lineRenderer.gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = CreateLineMesh(fromPosition, toPosition, gapDistance);
    }

    private Mesh CreateLineMesh(Vector3 fromPosition, Vector3 toPosition, float gapDistance)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[4];
        int[] triangles = new int[] { 0, 1, 2, 2, 1, 3 }; // Two triangles to form a quad

        // Define vertices to create a rectangular shape
        Vector3 lineDirection = (toPosition - fromPosition).normalized;
        Vector3 perpendicularDirection = new Vector3(-lineDirection.y, lineDirection.x, 0f).normalized; // Perpendicular to the line


        // Calculate the positions of the vertices
        vertices[0] = fromPosition - perpendicularDirection * halfWidth;
        vertices[1] = fromPosition + perpendicularDirection * halfWidth;
        vertices[2] = toPosition - perpendicularDirection * halfWidth;
        vertices[3] = toPosition + perpendicularDirection * halfWidth;

        // Apply a gap by adjusting the vertices' positions
        Vector3 gapOffset = lineDirection * gapDistance;
        vertices[0] += gapOffset;
        vertices[1] += gapOffset;
        vertices[2] -= gapOffset;
        vertices[3] -= gapOffset;

        // Set mesh properties
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }




}


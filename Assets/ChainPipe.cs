using UnityEngine;

public class ChainPipe : MonoBehaviour
{
    public GameObject ballPrefab; // Assign your ball prefab here
    public GameObject chainHead; // Assign the special head GameObject here
    public Transform handleTransform; // Assign the transform of the handle
    public int maxBalls = 10;
    public float extendSpeed = 1f; // Time in seconds to fully extend
    public float ballSpacing = 0.5f;
    public float handleMoveSpeed = 2f; // Speed at which handle moves towards the head
    public KeyCode extendKey = KeyCode.Space; // Button to press for extending

    private GameObject[] balls;
    public bool isExtending = false;
    public bool isRetracting = false;
    public bool handleMoving = false;
    private float currentExtendTime = 0f; // Current time spent on extending
    private Vector3 headStartPosition; // Starting position of the chain head

    void Start()
    {
        balls = new GameObject[maxBalls];
        for (int i = 0; i < maxBalls; i++)
        {
            balls[i] = Instantiate(ballPrefab, chainHead.transform.position, Quaternion.identity);
            balls[i].SetActive(false);
        }

        headStartPosition = chainHead.transform.position; // Save the starting position of the head
    }

    void Update()
    {
        if (Input.GetKeyDown(extendKey) && !isExtending && !isRetracting && !handleMoving)
        {
            print("extend");
            isExtending = true;
            currentExtendTime = 0f;
        }

        if (isExtending)
        {
            ExtendBalls();
        }
        else if (isRetracting)
        {
            RetractBalls();
        }
        else if (handleMoving)
        {
            MoveHandle();
        }
    }

    void ExtendBalls()
    {
        currentExtendTime += Time.deltaTime;
        int targetBallCount = Mathf.Min(maxBalls, (int)(currentExtendTime / extendSpeed * maxBalls));

        for (int i = 0; i < targetBallCount; i++)
        {
            if (!balls[i].activeSelf)
            {
                balls[i].SetActive(true);
                balls[i].transform.position = headStartPosition + new Vector3(ballSpacing * i, 0, 0);
            }
        }

        if (targetBallCount < maxBalls)
        {
            chainHead.transform.position = headStartPosition + new Vector3(ballSpacing * targetBallCount, 0, 0);
        }

        if (currentExtendTime >= extendSpeed)
        {
            isExtending = false;
            isRetracting = true;
        }
    }

    void RetractBalls()
    {
        if (currentExtendTime > 0)
        {
            currentExtendTime -= Time.deltaTime;
            int targetBallCount = (int)(currentExtendTime / extendSpeed * maxBalls);

            for (int i = targetBallCount; i < maxBalls; i++)
            {
                balls[i].SetActive(false);
            }

            chainHead.transform.position = headStartPosition + new Vector3(ballSpacing * targetBallCount, 0, 0);
        }
        else
        {
            isRetracting = false;
            chainHead.transform.position = headStartPosition; // Return the head to its starting position
        }
    }
    public void StopExtendingAndStartRetracting()
    {
        isExtending = false;
        isRetracting = false;
        handleMoving = true;
    }

    public void MoveHandle()
    {
        // Move the handle towards the head
        Vector3 direction = (chainHead.transform.position - handleTransform.position).normalized;
        handleTransform.position += direction * handleMoveSpeed * Time.deltaTime;

        // Determine how many balls should be active based on the handle's position
        float distanceToHead = Vector3.Distance(handleTransform.position, chainHead.transform.position);
        int activeBallsCount = Mathf.Clamp((int)(distanceToHead / ballSpacing), 0, maxBalls);

        for (int i = activeBallsCount; i < maxBalls; i++)
        {
            balls[i].SetActive(false);
        }

        // Check if the handle has reached the head
        if (Vector3.Distance(handleTransform.position, chainHead.transform.position) < ballSpacing)
        {
            handleMoving = false;
            chainHead.transform.position = headStartPosition; // Optionally reset the head position
        }
    }

}



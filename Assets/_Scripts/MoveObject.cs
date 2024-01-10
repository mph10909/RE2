using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float moveThreshold = 1.0f;
    public Vector3 moveAxis = Vector3.up;

    private float currentHeight = 0.0f;
    private bool moveUp = true;

    void Update()
    {
        if (moveUp)
        {
            currentHeight += moveSpeed * Time.unscaledDeltaTime;
        }
        else
        {
            currentHeight -= moveSpeed * Time.unscaledDeltaTime;
        }

        if (currentHeight >= moveThreshold && moveUp)
        {
            moveUp = false;
            currentHeight = 2.0f * moveThreshold - currentHeight;
        }
        else if (currentHeight <= -moveThreshold && !moveUp)
        {
            moveUp = true;
            currentHeight = -2.0f * moveThreshold - currentHeight;
        }

        transform.position += currentHeight * moveAxis;
    }
}

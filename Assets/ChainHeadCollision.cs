using UnityEngine;

public class ChainHeadCollision : MonoBehaviour
{
    public ChainPipe chainPipeScript; // Assign the ChainPipe script in the Inspector

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Hitable>() != null)
        {
            Debug.Log("Chain head collided with a Hitable object: " + collision.gameObject.name);

            // Notify the ChainPipe script to stop extending and start moving the handle
            if (chainPipeScript != null)
            {
                chainPipeScript.StopExtendingAndStartRetracting();
            }
        }
    }
}

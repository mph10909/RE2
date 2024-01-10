using UnityEngine;

namespace UICollecter
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 5.0f; // Speed of the player movement

        void Update()
        {
            // Get input from the horizontal and vertical axes (arrow keys or WASD)
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Calculate movement direction
            Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
            movementDirection.Normalize(); // Normalize to ensure consistent movement speed in all directions

            // Move the player
            transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
        }
    }
}

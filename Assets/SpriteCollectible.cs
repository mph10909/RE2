using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace UICollecter
{
    public class SpriteCollectible : MonoBehaviour
    {
        public CollectibleItem collectibleItem; // Assign this via the inspector
        public float duration = 1.0f; // Duration for the item to move to the slot
        private bool isMoving = false;

        public float floatDuration = 1f; // Duration of one cycle (up and down) for floating
        public float floatHeight = 0.5f; // How high it moves for floating

        void Start()
        {
            StartFloating(); // Start the floating animation
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check if the collider is the player
            if (other.CompareTag("Player") && !isMoving)
            {
                // Debug log to confirm the player has entered
                Debug.Log("Player has entered the trigger zone of " + gameObject.name);

                StartMoveToSlot();
            }
        }

        void StartFloating()
        {
            transform.DOMoveY(0 + floatHeight, floatDuration)
                .SetEase(Ease.InOutSine) // Smooth movement
                .SetLoops(-1, LoopType.Yoyo) // Loop indefinitely, alternating between moving up and down
                .SetRelative(true); // Move relative to the current position
        }

        private void StartMoveToSlot()
        {
            DOTween.Kill(transform);
            // Find an available slot for this item
            Slot availableSlot = InventoryManager.Instance.FindAvailableSlot(collectibleItem);
            if (availableSlot != null)
            {
                // Start the coroutine to move the item to the slot
                StartCoroutine(MoveToSlot(availableSlot));
            }
        }

        IEnumerator MoveToSlot(Slot slot)
        {
            isMoving = true;
            float time = 0f;
            Vector3 startPosition = transform.position;
            float updateInterval = 0.1f; // Update endPosition every 0.1 seconds
            float updateTimer = 0f;

            // Initial calculation of the end position
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(slot.uiImage.transform.position);
            Vector3 endPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, Camera.main.WorldToScreenPoint(startPosition).z));

            while (time < duration)
            {
                time += Time.deltaTime;
                updateTimer += Time.deltaTime;

                // Only update the endPosition at set intervals
                if (updateTimer >= updateInterval)
                {
                    updateTimer = 0f;
                    screenPoint = Camera.main.WorldToScreenPoint(slot.uiImage.transform.position);
                    endPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, Camera.main.WorldToScreenPoint(startPosition).z));

                    transform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
                    yield return null;
                }
            }

            // Snap to the final position in case of any discrepancies
            transform.position = endPosition;

            // Once the item has arrived at the slot, update the slot's state
            InventoryManager.Instance.UpdateSlot(slot, collectibleItem);

            isMoving = false;
            gameObject.SetActive(false); // Deactivate the item gameObject after it has reached the slot
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace ResidentEvilClone
{
    public class LookAtRig : MonoBehaviour
    {
        public RE playerActions;

        [SerializeField] LayerMask targetLayer;
        [SerializeField] Transform lookAtItem;
        [SerializeField] Transform lookAtReturn;

        [SerializeField] float detectionRange = 40f;
        [SerializeField] float threshold = 20f;
        [SerializeField] float maxAngle = 70f;
        [SerializeField] float lerpSpeed = 5f;

        private Transform closestTarget;
        private bool hasDividedMaxAngle = false;

        public float MaxAngle {
            
            get { return maxAngle; }
            set { maxAngle = value; } }

        void Start()
        {
            playerActions = new RE();
            playerActions.Player.Enable();
        }

        private void Update()
        {
            
            float aim = playerActions.Player.Aim.ReadValue<float>();

            if (aim > 0 && !hasDividedMaxAngle)
            {
                MaxAngle /= 2f;
                hasDividedMaxAngle = true;
            }
            else if (aim <= 0 && hasDividedMaxAngle)
            {
                MaxAngle *= 2f;
                hasDividedMaxAngle = false;
            }
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, targetLayer);

            float closestDistance = Mathf.Infinity;

            foreach (Collider collider in colliders)
            {
                LookAt lookAtComponent = collider.GetComponent<LookAt>();
                if (lookAtComponent == null)
                {
                    continue;
                }

                float distance = Vector3.Distance(transform.position, collider.transform.position);

                if (distance < closestDistance)
                {
                    Vector3 targetPosition = collider.transform.position;
                    Vector3 targetDirection = (targetPosition - transform.position).normalized;
                    float angle = Vector3.Angle(transform.forward, targetDirection);

                    if (angle < MaxAngle)
                    {
                        closestTarget = collider.transform;
                        closestDistance = distance;
                    }
                }
            }

            if (closestTarget != null && closestDistance < threshold)
            {
                Vector3 targetPosition = closestTarget.position;
                lookAtItem.position = Vector3.Lerp(lookAtItem.position, targetPosition, Time.deltaTime * lerpSpeed);
            }
            else
            {
                lookAtItem.position = Vector3.Lerp(lookAtItem.position, lookAtReturn.position, Time.deltaTime * lerpSpeed);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingFXAnim : MonoBehaviour
{
    [SerializeField] Transform groundCheck;
    [SerializeField] AudioSource audioSource;
    [SerializeField] FootStepData footStepManager;

    void PlayFootStep()
    {
        RaycastHit hit;

        if (Physics.Raycast(groundCheck.transform.position, Vector3.down, out hit, 3))
        {
            GroundMaterial groundMaterial = hit.collider.GetComponent<GroundMaterial>();

            if (groundMaterial != null)
            {
                foreach (FootStepInfo footStepInfo in footStepManager.footStepData)
                {
                    if (footStepInfo.footStepSurface == groundMaterial.SurfaceType)
                    {
                        if (footStepInfo.footSteps != null && audioSource != null)
                        {
                            audioSource.PlayOneShot(footStepInfo.footSteps.footstep[Random.Range(0, footStepInfo.footSteps.footstep.Length - 1)]);
                        }

                        break;
                    }
                }
            }
        }
    }

}


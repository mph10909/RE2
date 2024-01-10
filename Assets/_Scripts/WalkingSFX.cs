using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingSFX : MonoBehaviour
{
    [SerializeField] HealthManager health;
    [SerializeField] NavMeshAgent _player;
    [SerializeField] Transform groundCheck;
    [SerializeField] AudioSource footstepAudioSource;
    [SerializeField] float baseStepSpeed = 0.5f;
    [SerializeField] float footstepTimer = 0;
    float footstepOffset => baseStepSpeed;
    bool isWalking;
    bool isRunning;

    void Start()
    {
        health = GetComponent<HealthManager>();
    }

    void Update()
    {
        IsRunning();
        IsWalking();
        FootSteps();
        if (isRunning) RunningFXSpeed();
        if (!isRunning) WalkingFXSpeed();
    }

    private void IsWalking()
    {
        if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0 || Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (Mathf.Abs(_player.velocity.x) > 0 || Mathf.Abs(_player.velocity.y) > 0 || Mathf.Abs(_player.velocity.z) > 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    void FootSteps()
    {
        if (!isWalking) return;
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0)
        {
            if (Physics.Raycast(groundCheck.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                switch (hit.collider.tag)
                {
                    default:
                    case "Footsteps/Wood":   footstepAudioSource.PlayOneShot(WalkingFXAssets.Instance.footstepsWood
                                             [Random.Range(0, WalkingFXAssets.Instance.footstepsWood.Length - 1)]); 
                        break;
                    case "Footsteps/Carpet": footstepAudioSource.PlayOneShot(WalkingFXAssets.Instance.footstepsCarpet
                                             [Random.Range(0, WalkingFXAssets.Instance.footstepsCarpet.Length - 1)]);
                        break;
                    case "Footsteps/Stone":  footstepAudioSource.PlayOneShot(WalkingFXAssets.Instance.footstepsStone
                                             [Random.Range(0, WalkingFXAssets.Instance.footstepsStone.Length - 1)]);
                        break;
                    case "Footsteps/Gravel": footstepAudioSource.PlayOneShot(WalkingFXAssets.Instance.footstepsGravel
                                             [Random.Range(0, WalkingFXAssets.Instance.footstepsGravel.Length - 1)]);                     
                        break;
                    case "Footsteps/Grass":  footstepAudioSource.PlayOneShot(WalkingFXAssets.Instance.footstepsGrass
                                             [Random.Range(0, WalkingFXAssets.Instance.footstepsGrass.Length - 1)]);                        
                        break;
                    case "Footsteps/Metal":  footstepAudioSource.PlayOneShot(WalkingFXAssets.Instance.footstepsMetal
                                             [Random.Range(0, WalkingFXAssets.Instance.footstepsMetal.Length - 1)]);                        
                        break;
                   case "Footsteps/Glass":  footstepAudioSource.PlayOneShot(WalkingFXAssets.Instance.footstepsGlass
                                             [Random.Range(0, WalkingFXAssets.Instance.footstepsGlass.Length - 1)]);                        
                        break;
                }
            }
            footstepTimer = baseStepSpeed;
        }
    }

    void WalkingFXSpeed()
    {
        if (health != null)
        {
            switch (health.Health)
            {
                case 0:
                    baseStepSpeed = 0.0f;
                    break;
                case int i when i > 0 && i <= 35:
                    baseStepSpeed = 1.15f;
                    break;
                case int i when i > 35 && i <= 65:
                    baseStepSpeed = 0.95f;
                    break;
                default:
                    baseStepSpeed = 0.65f;
                    break;
            }

        }
    }

    void RunningFXSpeed()
    {
        if (health != null)
        {
            switch (health.Health)
            {
                case 0:
                    baseStepSpeed = 0.0f;
                    break;
                case int i when i > 0 && i <= 35:
                    baseStepSpeed = 1.05f;
                    break;
                case int i when i > 35 && i <= 65:
                    baseStepSpeed = 0.85f;
                    break;
                default:
                    baseStepSpeed = 0.65f;
                    break;
            }

        }
    }

    void IsRunning()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }
}

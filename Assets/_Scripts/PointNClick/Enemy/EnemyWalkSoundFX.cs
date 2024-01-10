using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWalkSoundFX : MonoBehaviour
{
    NavMeshAgent _Character;
    AudioSource _audioSource;
    EnemyNavigation _enemyNavigation;
    bool isWalking;

    [SerializeField] Transform groundCheck;
    [SerializeField] AudioClip enemyfootStep;
    [SerializeField] float baseStepSpeed = 0.5f;
    [SerializeField] float footstepTimer;
    [SerializeField] float _distanceAway;


    void Start()
    {
        _enemyNavigation = GetComponent<EnemyNavigation>();
        _Character = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        IsWalking();
        FootSteps();

        AdjustVolume();

    }

    private void AdjustVolume()
    {
        if (_enemyNavigation.Eating && _enemyNavigation.PlayerDistance > _distanceAway * 1.1f)
        {
            _audioSource.volume = 0.0f;
        }

        float targetVolume = 0;
        if (!_enemyNavigation.Eating)
        {
            targetVolume = 1;
        }
        else if (_enemyNavigation.Eating && _enemyNavigation.PlayerDistance < _distanceAway)
        {
            targetVolume = 1;
        }
        else if(_enemyNavigation.Eating && _enemyNavigation.PlayerDistance > _distanceAway)
        {
            targetVolume = 0.001f;
        }
        _audioSource.volume = Mathf.Clamp(Mathf.Lerp(_audioSource.volume, targetVolume, Time.deltaTime), 0.001f, 1);
    }



    private void IsWalking()
    {
        
        if (Mathf.Abs(_Character.velocity.x) > 0 || Mathf.Abs(_Character.velocity.y) > 0 || Mathf.Abs(_Character.velocity.z) > 0)
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
        RaycastHit hit;
        if (footstepTimer <= 0)
        {
            if (Physics.Raycast(groundCheck.transform.position, Vector3.down, out hit, 3))
            {
                switch (hit.collider.tag)
                {
                    default:
                    case "Footsteps/Stone":
                        _audioSource.PlayOneShot(enemyfootStep);
                        break;
                    case "Footsteps/Grass":
                        _audioSource.PlayOneShot(enemyfootStep);
                        break;
                }
            }
            footstepTimer = baseStepSpeed;

        }
    }
        
    }

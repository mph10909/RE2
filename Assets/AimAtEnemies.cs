using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class AimAtEnemies : MonoBehaviour
{
    #region Constants
    private const string AIMER = "Aimer";
    private const string AIMING = "Aiming";
    private const string RELOAD = "Reload";
    private const string FIRE = "Fire";
    #endregion

    #region Public Variables
    public Transform player;
    public Transform WeaponSlot;
    public float rotationSpeed = 2.0f;
    public float fireInterval = 20.0f;
    #endregion

    #region Private Variables
    private NavMeshAgent navMeshAgent;
    private PointAndClick characterController;
    private int weapon;
    private bool isOutOfAmmo = false;
    private bool isFiring = false;
    private bool isTargetingEnemy = false;
    [SerializeField]
    private SphereCollider detectionZone;

    #endregion

    #region Properties
    public bool isEnemyDetected { get; private set; }
    public bool Firing { get { return characterController.PlayerAnimator.GetBool("Firing"); } }
    public bool SetFire
    {
        get { return isFiring; }
        set
        {
            if (value && !isFiring)
            {
                characterController.PlayerAnimator.SetTrigger("Fire");
                isFiring = true;
            }
            else if (!value && isFiring)
            {
                characterController.PlayerAnimator.ResetTrigger("Fire");
                isFiring = false;
            }
        }
    }
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        Initialize();
    }

    private void OnDestroy()
    {
        Actions.AmmoEmpty -= HandleAmmoEmpty;
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleEnemyDetection(other);
    }

    private void OnTriggerStay(Collider other)
    {
        HandleEnemyDetection(other);
    }

    private void OnTriggerExit(Collider other)
    {
        HandleEnemyDetectionExit(other);
    }
    #endregion

    #region Main Methods
    private void Initialize()
    {
        weapon = Animator.StringToHash("Weapon");
        detectionZone = GetComponent<SphereCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        characterController = GetComponent<PointAndClick>();
        Actions.AmmoEmpty += HandleAmmoEmpty;
    }

    public void Aim()
    {
        if (weapon == 0) return;
        characterController.PlayerAnimator.SetBool(AIMER, true);
    }

    public void StopAim()
    {
        characterController.PlayerAnimator.SetBool(AIMING, false);
        characterController.PlayerAnimator.SetBool(AIMER, false);
    }

    public void Fire()
    {
        // Check conditions before firing
        if (characterController.PlayerAnimator.GetBool(RELOAD)) return;
        if (!characterController.PlayerAnimator.GetBool(AIMING)) return;

        // Loop through the children of WeaponSlot and attempt to fire
        foreach (Transform child in WeaponSlot)
        {
            var fireableWeapon = child.GetComponent<IFireable>();
            if (fireableWeapon != null)
            {
                // Only fire (trigger animation) if AIFire returns true
                if (fireableWeapon.AIFire())
                {
                    characterController.PlayerAnimator.SetTrigger(FIRE);
                    break;
                }
            }
        }
    }


    private void StopFiring()
    {
        StopCoroutine(FireWeaponRoutine());
    }
    #endregion

    #region Helper Methods
    private void HandleAmmoEmpty()
    {
        isOutOfAmmo = true;
        characterController.IsAiming = false;
        StopFiring();
    }

    private void HandleEnemyDetection(Collider other)
    {
        if (isOutOfAmmo) return;

        if (characterController.PlayerNavMesh.destination == player.position) return;

        if (other.TryGetComponent<Enemy>(out Enemy enemy) && !isTargetingEnemy)
        {
            isEnemyDetected = true;
            navMeshAgent.SetDestination(transform.position);
            StopAllCoroutines();
            StartCoroutine(RotateTowardsTarget(other.transform));
            characterController.IsAiming = true;
            Aim();
            StartCoroutine(FireWeaponRoutine());
            isTargetingEnemy = true;
        }
    }

    private void HandleEnemyDetectionExit(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            isEnemyDetected = false;
            isTargetingEnemy = false; // Reset the targeting flag
            StopAllCoroutines();

            if (navMeshAgent.destination == player.position)
            {
                characterController.IsAiming = false;
                
                StopAim();
            }
            else
            {
                StartCoroutine(RotateBackToOriginalTarget(player));
            }

            Debug.Log("Enemy left detection zone. Resuming other behaviors.");
        }
    }

    #endregion

    #region Coroutines
    IEnumerator FireWeaponRoutine()
    {
        while (characterController.IsAiming)
        {
            yield return new WaitForSeconds(1);
            Fire();
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator RotateTowardsTarget(Transform target)
    {
        Quaternion targetRotation;
        Vector3 directionToEnemy;
        do
        {
            if (navMeshAgent.destination == player.position)
            {
                StopAim();
                yield break;
            }

            directionToEnemy = (target.position - transform.position).normalized;
            directionToEnemy.y = 0;
            targetRotation = Quaternion.LookRotation(directionToEnemy);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        } while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f);
    }


    IEnumerator RotateBackToOriginalTarget(Transform target)
    {
        Quaternion originalRotation;
        Vector3 directionToTarget;
        do
        {
            directionToTarget = (target.position - transform.position).normalized;
            directionToTarget.y = 0;
            originalRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        } while (Quaternion.Angle(transform.rotation, originalRotation) > 0.01f);
    }
    #endregion
}

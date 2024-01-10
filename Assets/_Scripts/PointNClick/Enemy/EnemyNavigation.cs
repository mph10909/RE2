using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ResidentEvilClone;

public class EnemyNavigation : MonoBehaviour
{

    #region Variables
    [SerializeField] bool displayCasts;

    [Header("Layers To Ignore")][Space(2)]
    [SerializeField] LayerMask layerMask;
   
    [Header("RayCast Info")][Space(2)]
    [SerializeField] float _rayDistance = 12f;
    [SerializeField] int _numRays = 5;
    [SerializeField] float _fanAngle  = 60f;
    [SerializeField] float _rayRange = 20f;
    [SerializeField] float rayHeight = 7;

    [Header("SphereCast Info")][Space(2)]
    [SerializeField] private float sphereCastRadius = 5f;
    [SerializeField] private float _sphereCastInterval = 1f;
    [SerializeField] float _spherCastHeight = 5f;
    [SerializeField]float _distanceToAlertOthers = 2f;

    [Header("Notify Enemy Time")][Space(2)]
    [Tooltip("How often to signal other enemies")]
    [SerializeField] private float _notifyEnemyInterval = 3f;

    [Header("Follow Player Time")][Space(2)]
    [SerializeField] float followTime = 5f;
    [SerializeField] float _enemyAvoidanceDistance;
    
    [SerializeField] GameObject character;
    CharacterManager characterSwap;
    NavMeshAgent enemyNav;
    Animator anim;
    Vector3 startLocation, startRotation;

    bool hasBeenTold;
    bool hasBeenSpotted;
    bool hasGottenToClose;
    bool isFollowing = false;
    float followTimer = 0f;
    float spottedDistance;
    private float lastSphereCastTime;
    private float lastEnemyCheckTime;

    [SerializeField] SphereCollider _sphereCollider;
    #endregion

    void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        followTime = UnityEngine.Random.Range(followTime - 2, followTime + 3);
        _rayDistance = UnityEngine.Random.Range(10, 15);
        ColliderRadius = UnityEngine.Random.Range(1.5f, 2.5f);
        characterSwap = FindObjectOfType<CharacterManager>();
        ActionSet();
        startLocation = this.transform.position;
        startRotation = this.transform.localEulerAngles;
        if(character == null)character = characterSwap.Character;
        anim = GetComponentInChildren<Animator>();
        enemyNav = GetComponent<NavMeshAgent>();
    }

    #region Enable/Disable
    void OnEnable()
    {
        Actions.CharacterSwap += SetCharacter;
        Actions.EnemyForget += ResetEnemyPos;
    }

    void OnDisable()
    {
        ActionSet();
    }
    #endregion

    #region Mutators
    private void ActionSet()
    {
        Actions.CharacterSwap -= SetCharacter;
        Actions.EnemyForget -= ResetEnemyPos;
    }
    
    public bool BeenTold
    {
        get { return hasBeenTold; }
        set { hasBeenTold = value; }
    }

    public NavMeshAgent EnemyNavMesh
    {
        get {return enemyNav; }
        set { enemyNav = value; }
    }

    public bool BeenShot
    {
        get { return anim.GetBool("BeenShot"); }
        set { anim.SetBool("BeenShot", value); }
    }

    public bool Eating
    {
        get { return anim.GetBool("Eating"); }
        set { anim.SetBool("Eating", value); }
    }

    public bool Standing
    {
        get { return anim.GetBool("Standing"); }
        set { anim.SetBool("Standing", value); }
    }

    public bool LimbMissing
    {
        get { return anim.GetBool("LimbMissing"); }
        set { anim.SetBool("LimbMissing", value); }
    }

    private float ColliderRadius { get { return _sphereCollider.radius; } set { if (_sphereCollider != null) _sphereCollider.radius = value; } }

    public float PlayerDistance { get { return Vector3.Distance(transform.position, character.transform.position); } }

    public float Health
    {
        get { return anim.GetFloat("Health"); }
    }

    bool BeenSpotted => anim.GetFloat("MovementSpeed") > 0;
    #endregion

    void Update()
    {
        if (GameStateManager.Instance.CurrentGameState == GameState.Paused) return;
        if (Health <= 0)
        {
            if (EnemyNavMesh != null && EnemyNavMesh.isActiveAndEnabled && EnemyNavMesh.isOnNavMesh)
            {
                EnemyNavMesh.isStopped = true; // Or any other desired value
            }
            return;
        }

        LookForPlayer();
        //NavRadiusSetter();

    }

    //private void NavRadiusSetter()
    //{
    //    float distanceToPlayer = Vector3.Distance(transform.position, character.transform.position);
    //    if (distanceToPlayer < _enemyAvoidanceDistance)
    //    {
    //        // Player is too close, set the radius to 0.25f
    //        EnemyNavMesh.radius = 0.25f;
    //        transform.LookAt(character.transform);
    //    }
    //    else
    //    {
    //        // Player is far enough away, set the radius to 0.5f
    //        EnemyNavMesh.radius = 0.5f;
    //    }
    //}

    //private void OnTriggerEnter(Collider collider)
    //{
    //    if(collider.gameObject == character)
    //    {
    //        print("Character Enter");
    //        _sphereCollider.radius = _sphereCollider.radius * 1.5f ;
    //    }
    //}

    //private void OnTriggerStay(Collider collider)
    //{
    //    if (collider.gameObject == character)
    //    {
    //        Debug.DrawLine(this.transform.position, character.transform.position, Color.red);
    //        print(PlayerDistance);
    //        //LookForPlayer();
    //    }
    //}

    //private void OnTriggerExit(Collider collider)
    //{
    //    if (collider.gameObject == character)
    //    {
    //        print("Character Exit");
    //        _sphereCollider.radius = _sphereCollider.radius / 1.5f;
    //    }
    //}

    private void LookForPlayer()
    {
        //print(CheckForPlayer());
        bool foundPlayer = CheckForPlayer() || BeenShot || BeenTold || hasGottenToClose;

        if (foundPlayer)
        {
            // Start following the player
            if (EnemyNavMesh.enabled)
            {
                if (!Standing && !LimbMissing) return;
                EnemyNavMesh.SetDestination(character.transform.position);
                if (EnemyNavMesh != null && EnemyNavMesh.isActiveAndEnabled && EnemyNavMesh.isOnNavMesh)
                {
                    EnemyNavMesh.isStopped = false; // Or any other desired value
                }
                isFollowing = true;
                CheckForEnemies();
            }
        }

        if (BeenSpotted || hasBeenTold || hasGottenToClose)
        {
            return;
            // Move towards last seen position of the player
            if (!Standing && !LimbMissing) return;
            EnemyNavMesh.SetDestination(character.transform.position);
            Debug.DrawLine(this.transform.position, character.transform.position, Color.blue);
            spottedDistance = 35f;

            if (!CheckForPlayer())
            {
                // Player is not in sight anymore, wait for a while before giving up
                followTimer += Time.deltaTime;

                if (followTimer >= followTime)
                {
                    EnemyNavMesh.isStopped = true;
                    isFollowing = false;
                    hasBeenTold = false;
                    hasGottenToClose = false;
                    spottedDistance = _rayRange;
                    followTimer = 0f;
                }
            }
            else
            {
                followTimer = 0f;
            }
        }

        else if (!foundPlayer)
        {
            if (EnemyNavMesh == null) return;
            // Reset to default state
            spottedDistance = _rayRange;
            if (EnemyNavMesh != null && EnemyNavMesh.isActiveAndEnabled && EnemyNavMesh.isOnNavMesh)
            {
                EnemyNavMesh.isStopped = true; // Or any other desired value
            }
            followTimer = 0f;
            isFollowing = false;
        }
    }

    public void CheckForEnemies()
    {
        if (Time.time > lastEnemyCheckTime + _sphereCastInterval)
        {
            lastEnemyCheckTime = Time.time;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position + Vector3.up * _spherCastHeight, sphereCastRadius * _distanceToAlertOthers, layerMask);

            foreach (Collider hitCollider in hitColliders)
            {
                EnemyNavigation enemy = hitCollider.GetComponent<EnemyNavigation>();
                if (enemy != null && !enemy.isFollowing)
                {
                    if (enemy.Eating) enemy.Eating = false;
                    enemy.BeenTold = true;
                }
            }
        }
    }

    bool CheckForPlayer()
    {
        #region SphereCastForPlayer
        if (Time.time > lastSphereCastTime + _sphereCastInterval && !hasGottenToClose)
        {
            lastSphereCastTime = Time.time;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position + Vector3.up * _spherCastHeight, sphereCastRadius, layerMask);
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject == character)
                {
                    if (Eating) Eating = false;
                    Debug.Log("Player detected by sphere cast!");
                    hasGottenToClose = true;
                    followTimer = 0f;
                    return true;
                }
            }
        }
        #endregion

        #region RayCastFowardForPlayer
        Vector3 targetDirection = character.transform.position - transform.position;
        float halfAngle = _fanAngle / 2f;
        float angleBetweenRays = _fanAngle / (_numRays - 1);

        // Cast rays within the fan angle
        for (int i = 0; i < _numRays; i++)
        {
            Quaternion rayRotation = Quaternion.AngleAxis(-halfAngle + (i * angleBetweenRays), Vector3.up);
            Vector3 rayDirection = rayRotation * transform.forward;
            Ray ray = new Ray(transform.position + Vector3.up * rayHeight, rayDirection);

            if (Physics.Raycast(ray, out RaycastHit hit, spottedDistance, layerMask))
            {
                if (hit.collider.gameObject == character)
                {
                    if(displayCasts)Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                    followTimer = 0f;
                    if (Eating) Eating = false;
                    //hasGottenToClose = true;
                    return true;
                }
                else
                {
                    if (displayCasts) Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
                }
            }
            else
            {
                if (displayCasts) Debug.DrawRay(ray.origin, ray.direction * spottedDistance, Color.yellow);
            }
        }
        #endregion

        #region RayCastBackForPlayer
        // Cast additional rays with smaller angle and shorter range behind the enemy
        float backAngle = 180f - _fanAngle;
        float backHalfAngle = backAngle / 2f;
        float backAngleBetweenRays = backAngle / (_numRays - 1);

        for (int i = 0; i < _numRays; i++)
        {
            Quaternion rayRotation = Quaternion.AngleAxis(-backHalfAngle + (i * backAngleBetweenRays), Vector3.up);
            Vector3 rayDirection = rayRotation * -transform.forward;
            Ray ray = new Ray(transform.position + Vector3.up * rayHeight, rayDirection);

            if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, layerMask))
            {
                if (hit.collider.gameObject == character)
                {
                    if (Eating) Eating = false;
                    if (displayCasts) Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                    followTimer = 0f;
                    hasGottenToClose = true;
                    return true;
                }
            }
            else
            {
                if (displayCasts) Debug.DrawRay(ray.origin, ray.direction * _rayDistance, Color.cyan);
            }
        }
        #endregion

        return false;
    }

    void ResetEnemyPos()
    {
        BeenShot = false;
        EnemyNavMesh.Warp(startLocation);
        transform.localEulerAngles = startRotation;
        EnemyForget();
    }

    void EnemyForget()
    {
        if (Health <= 0 || !EnemyNavMesh.enabled) return;
        BeenShot = false;
        hasBeenTold = false;
        hasGottenToClose = false;
        EnemyNavMesh.ResetPath();
        if (EnemyNavMesh != null && EnemyNavMesh.isActiveAndEnabled && EnemyNavMesh.isOnNavMesh)
        {
            EnemyNavMesh.isStopped = true; // Or any other desired value
        }
    }

    void SetCharacter(GameObject newCharacter)
    {
        character = newCharacter;
        EnemyForget();
    }

    private void OnDrawGizmos()
    {
        if (displayCasts)
        {
            if (!hasGottenToClose && !hasBeenTold) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * _spherCastHeight, sphereCastRadius);

            Gizmos.color = Color.yellow;
            if (EnemyNavMesh != null && EnemyNavMesh.destination == character.transform.position) return;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * _spherCastHeight, sphereCastRadius * _distanceToAlertOthers);
        }

    }
}

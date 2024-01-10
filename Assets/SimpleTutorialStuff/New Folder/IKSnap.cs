using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKSnap : MonoBehaviour
{

    [SerializeField] bool _useIK;
    [SerializeField] bool _leftHandIK;
    [SerializeField] bool _rightHandIk;
    [SerializeField] bool _leftFootIK;
    [SerializeField] bool _rightFootIK;

    Vector3 _leftHandPos;
    Vector3 _rightHandPos;

    Vector3 _leftFootPos;
    Vector3 _rightFootPos;


    [SerializeField] Vector3 _leftIkRayStart;
    [SerializeField] Vector3 _leftIkRayFinish;
    [SerializeField] Vector3 _rightIkRayStart;
    [SerializeField] Vector3 _rightIkRayFinish;

    [SerializeField] Vector3 _leftFootIkRayStart;
    [SerializeField] Vector3 _leftFootIkRayFinish;
    [SerializeField] Vector3 _rightFootIkRayStart;
    [SerializeField] Vector3 _rightFootIkRayFinish;

    Quaternion _leftHandRot;
    Quaternion _rightHandRot;

    [SerializeField] Vector3 _leftHandOffset;
    [SerializeField] Vector3 _rightHandOffset;

    [SerializeField] float _distanceFromCharacter;
    [SerializeField] float _handCastDistance = 2;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        RaycastHit leftHit;
        RaycastHit rightHit;

        RaycastHit leftFootHit;
        RaycastHit rightFootHit;

        if (Physics.Raycast(transform.position + transform.forward * _distanceFromCharacter + transform.TransformDirection(_leftIkRayStart), -transform.up + transform.TransformDirection(_leftIkRayFinish), out leftHit, _handCastDistance))
        {
            _leftHandIK = true;
            _leftHandPos = leftHit.point - _leftHandOffset;
            _leftHandRot = Quaternion.FromToRotation(transform.TransformDirection(transform.position), leftHit.normal);
        }
        else
        {
            _leftHandIK = false;
        }

        if (Physics.Raycast(transform.position + transform.forward * _distanceFromCharacter + transform.TransformDirection(_rightIkRayStart), -transform.up + transform.TransformDirection(_rightIkRayFinish), out rightHit, _handCastDistance))
        {
            _rightHandIk = true;
            _rightHandPos = rightHit.point - _rightHandOffset;
            _rightHandRot = Quaternion.FromToRotation(Vector3.forward, leftHit.normal);
        }
        else
        {
            _rightHandIk = false;
        }

        if(Physics.Raycast(transform.position + _leftFootIkRayStart, transform.forward, out leftFootHit, _distanceFromCharacter))
        {
            _leftFootIK = true;
            _leftFootPos = leftFootHit.point;
        }
        else
        {
            _leftFootIK = false;
        }

        if (Physics.Raycast(transform.position + _rightFootIkRayStart, transform.forward, out rightFootHit, _distanceFromCharacter))
        {
            _rightFootIK = true;
            _rightFootPos = rightFootHit.point;
        }
        else
        {
            _rightFootIK = false;
        }

        Debug.DrawRay(transform.position + _leftFootIkRayStart, transform.forward * _handCastDistance, Color.green);
        Debug.DrawRay(transform.position + _rightFootIkRayStart, transform.forward * _handCastDistance, Color.green);
        Debug.DrawRay(transform.position + transform.forward * _distanceFromCharacter + transform.TransformDirection(_leftIkRayStart), -transform.up + transform.TransformDirection(_leftIkRayFinish) * _handCastDistance, Color.green);
        Debug.DrawRay(transform.position + transform.forward * _distanceFromCharacter + transform.TransformDirection(_rightIkRayStart), -transform.up + transform.TransformDirection(_rightIkRayFinish) *_handCastDistance, Color.green);
    }

    void OnAnimatorIK()
    {
        if (_useIK)
        {
            if (_leftHandIK)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandPos);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandRot);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            }
            if (_rightHandIk)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                anim.SetIKPosition(AvatarIKGoal.RightHand, _rightHandPos);
                anim.SetIKRotation(AvatarIKGoal.RightHand, _rightHandRot);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            }

            if (_leftFootIK)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
                anim.SetIKPosition(AvatarIKGoal.LeftFoot, _leftFootPos);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            }
            if (_rightFootIK)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
                anim.SetIKPosition(AvatarIKGoal.RightFoot, _rightFootPos);
                anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_leftHandPos, 0.25f);
        Gizmos.DrawWireSphere(_rightHandPos, 0.25f);
        Gizmos.DrawWireSphere(_leftFootPos, 0.25f);
        Gizmos.DrawWireSphere(_rightFootPos, 0.25f);
    }

}

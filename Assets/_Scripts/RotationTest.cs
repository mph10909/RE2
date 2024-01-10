using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class RotationTest : MonoBehaviour
    {

        public IEnumerator Rotate(float lerpDuration)
        {
            Animator anim = GetComponentInChildren<Animator>();
            anim.speed = 0;
            float timeElapsed = 0;
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 180, 0);
            while (timeElapsed < lerpDuration)
            {
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            transform.rotation = targetRotation;
            anim.speed = 1;
        }
    }
}

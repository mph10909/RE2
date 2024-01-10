using UnityEngine;
using System.Collections;

namespace ResidentEvilClone
{
    public class DoorAnimation : MonoBehaviour
    {
        public Animator animator;

        bool isFinished;

        public IEnumerator Play(string direction)
        {
            animator.SetTrigger(direction);
            isFinished = false;
            while (!isFinished)
                yield return null;
        }

        public void AnimationFinished()
        {
            print("animation finished");
            animator.ResetTrigger("Exit");
            animator.ResetTrigger("Enter");
            isFinished = true;

        }
    }
}

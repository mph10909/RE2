using UnityEngine.Events;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;

namespace ResidentEvilClone
{

    public class BaseDoor : MonoBehaviour
    {
        public Transform exitPoint;
        public AudioClip newSceneTrack;

        private DoorLock doorlocked;

        public BaseDoor Exit;

        [Header("Door Animation Prefab")]
        public DoorIdentifier direction;
        public DoorAnimation doorAnimationPrefab;
        private static Dictionary<DoorAnimation, DoorAnimation> m_AnimationInstances = new Dictionary<DoorAnimation, DoorAnimation>();
        private Vector3 m_InstantiationPosition = Vector3.zero;

        public UnityEvent OnLocked;
        public UnityEvent OnOpened;
        private Transform m_Interactor;

        private void Awake()
        {
            doorlocked = GetComponent<DoorLock>();
        }

        public bool IsLocked()
        {
            return (doorlocked && doorlocked.isLocked || Exit && Exit.doorlocked && Exit.doorlocked.isLocked);
        }


        public virtual void OpenDoor(IInteractor interactor)
        {
            if (doorlocked && doorlocked.IsLocked)
            {
                doorlocked.TryToUnlock(out bool open);
                if (!open)
                {
                    if (IsLocked())
                        OnLocked?.Invoke();
                    return;
                }
            }

            if (Exit && Exit.doorlocked && Exit.doorlocked.isLocked)
            {
                doorlocked.TryToUnlock(out bool open);
                if (!open)
                {
                    if (IsLocked())
                        OnLocked?.Invoke();
                    return;
                }
            }

            OnOpened?.Invoke();
            StartCoroutine(TransitionSequence(interactor));
        }

        private IEnumerator TransitionSequence(IInteractor interactor)
        {
            Cursor.visible = false;
            PauseController.Instance.Pause();

            yield return StartCoroutine(Fade.Instance.FadeRoutine(1, 2f));

            if (newSceneTrack != SoundManagement.Instance.CurrentSoundTrack && newSceneTrack != null) SoundManagement.Instance.FadeOut(2.0f);

            Actions.EnemyForget?.Invoke();

            if (doorAnimationPrefab)
            {
                DoorAnimation doorAnimInstance = GetDoorAnimInstance();

                doorAnimInstance.gameObject.SetActive(true);
                

                Coroutine doorAnimRoutine = StartCoroutine(doorAnimInstance.Play(direction.identifier));

                yield return doorAnimRoutine;

                doorAnimInstance.gameObject.SetActive(false);
            }

            MonoBehaviour interactorMB = (MonoBehaviour)interactor;
            m_Interactor = interactorMB.transform;

            var Nav = m_Interactor.gameObject.GetComponent<NavMeshAgent>();

            Nav.Warp(Exit.exitPoint.position);
            PauseController.Instance.Resume();

            if (newSceneTrack != SoundManagement.Instance.CurrentSoundTrack && newSceneTrack != null ) SoundManagement.Instance.FadeIn(2.0f, newSceneTrack);


            Fade.Instance.DoFade(0, 2.0f, () => { Cursor.visible = true; });

            
        }


        private DoorAnimation GetDoorAnimInstance()
        {
            if (!m_AnimationInstances.TryGetValue(doorAnimationPrefab, out DoorAnimation existingAnimInstance))
            {

                GameObject animInstanceGO = Instantiate(doorAnimationPrefab.gameObject, m_InstantiationPosition, Quaternion.identity);
                animInstanceGO.transform.SetParent(transform);
                DoorAnimation newAnimInstance = animInstanceGO.GetComponent<DoorAnimation>();
                m_AnimationInstances.Add(doorAnimationPrefab, newAnimInstance);
                return newAnimInstance;
            }
            return existingAnimInstance;
        }
    }
}

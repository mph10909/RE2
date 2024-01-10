using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace ResidentEvilClone
{
    public class SceneDoor : MonoBehaviour
    {
        [Header("Scene Transition Properties")]
        public SceneReference destinationScene;
        public DoorIdentifier ownIdentifier;
        public DoorIdentifier targetIdentifier;
        public Transform exitPoint;
        private DoorLock islocked;

        [Header("Audio Properties")]
        [SerializeField]
        private AudioClip newSceneTrack;

        [Header("Scene Management")]
        public static List<SceneDoor> AllSceneDoors = new List<SceneDoor>();
        private CharacterManager charManager;

        [Header("Door Animation Prefab")]
        public DoorAnimation doorAnimationPrefab;
        private static Dictionary<DoorAnimation, DoorAnimation> m_AnimationInstances = new Dictionary<DoorAnimation, DoorAnimation>();
        private Vector3 m_InstantiationPosition = Vector3.zero; // This should be set to where you want to instantiate the animations.

        private void Awake()
        {
            islocked = GetComponent<DoorLock>();
        }

        private void OnEnable()
        {
            AllSceneDoors.Add(this);
        }

        private void OnDisable()
        {
            AllSceneDoors.Remove(this);
            m_AnimationInstances.Clear();
        }

        public void OpenDoor()
        {
            if(islocked && islocked.IsLocked)
            {
                islocked.TryToUnlock(out bool open);
                if(!open){
                    return;
                }
            }

            print("Start The Transition");
            StartCoroutine(TransitionSequence());
        }

        private IEnumerator TransitionSequence()
        {
            PauseController.Instance.Pause();
            yield return StartCoroutine(Fade.Instance.FadeRoutine(1, 2f));

            if (newSceneTrack != null)
            {
                SoundManagement.Instance.FadeOut(2.0f);
            }
            // Check if there is a DoorAnimation prefab, then instantiate and play.
            if (doorAnimationPrefab != null)
            {
                print("Get The Door Loaded");
                DoorAnimation doorAnimInstance = GetDoorAnimInstance();
                // Assuming Play is a coroutine within the DoorAnimation script.
                yield return StartCoroutine(doorAnimInstance.Play(null));
            }

            Fade.Instance.DoFade(1, 2.0f, () =>
            {
                StartCoroutine(TransitionToScene(destinationScene.sceneName));
            });// Once the door animation is done, proceed to load the scene.
        }

        private IEnumerator PlayDoorAnimationAndTransition(DoorAnimation doorAnimInstance)
        {
            yield return StartCoroutine(doorAnimInstance.Play(null));

            Fade.Instance.DoFade(1, 2.0f, () =>
            {
                StartCoroutine(TransitionToScene(destinationScene.sceneName));
            });
        }

        IEnumerator TransitionToScene(string sceneName)
        {
            ObjectStateManager.Instance.CaptureStates();
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            SceneManager.sceneLoaded += OnSceneLoaded;

            while (!asyncLoad.isDone)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
                yield return null;
            }
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            print("SceneLoaded");
            SceneManager.sceneLoaded -= OnSceneLoaded;
            ObjectStateManager.Instance.IsSceneTransition = true;

            SceneDoor correspondingDoor = FindCorrespondingDoor(targetIdentifier);
            SetCharacterPosition(correspondingDoor);
            ObjectStateManager.Instance.ApplyStates();

            if (newSceneTrack != null)
            {
                SoundManagement.Instance.FadeIn(2.0f, newSceneTrack);
            }
            PauseController.Instance.Resume();
            Fade.Instance.DoFade(0, 2.0f, CompleteSceneTransition);
        }

        private void SetCharacterPosition(SceneDoor correspondingDoor)
        {
            if (correspondingDoor != null && correspondingDoor.exitPoint != null)
            {
                charManager = FindObjectOfType<CharacterManager>();
                if (charManager?.Character != null)
                {
                    charManager.Character.GetComponent<NavMeshAgent>().Warp(correspondingDoor.exitPoint.position);
                }
            }
            else
            {
                Debug.LogWarning("Missing references in SceneDoor");
            }
        }

        private void CompleteSceneTransition()
        {
            
            ObjectStateManager.Instance.IsSceneTransition = false;

        }

        private SceneDoor FindCorrespondingDoor(DoorIdentifier targetID)
        {
            foreach (SceneDoor door in AllSceneDoors)
            {
                if (door.ownIdentifier == targetID)
                {
                    return door;
                }
            }
            return null;
        }

        private DoorAnimation GetDoorAnimInstance()
        {
            print("Instantiate");
            if (!m_AnimationInstances.TryGetValue(doorAnimationPrefab, out DoorAnimation existingAnimInstance))
            {

                GameObject animInstanceGO = Instantiate(doorAnimationPrefab.gameObject, m_InstantiationPosition, Quaternion.identity);
                animInstanceGO.transform.SetParent(transform); // Set the parent to keep the hierarchy organized
                print("Instanited");
                DoorAnimation newAnimInstance = animInstanceGO.GetComponent<DoorAnimation>();
                m_AnimationInstances.Add(doorAnimationPrefab, newAnimInstance);
                return newAnimInstance;
            }
            // If an instance exists, return it
            print("Exists");
            return existingAnimInstance;
        }
    }
}

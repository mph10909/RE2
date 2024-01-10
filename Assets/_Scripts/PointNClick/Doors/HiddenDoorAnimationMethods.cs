using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class HiddenDoorAnimationMethods : MonoBehaviour, IILoadable
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] GameObject thisObject;
        [SerializeField] bool activated;

        public void Load()
        {
            if (activated) { thisObject.SetActive(false); print("No Trap Door"); }
        }
    
        void Pause()
        {
            Time.timeScale = 0;
            GameStateManager.Instance.SetState(GameState.Paused);
        }

        void StartAudio()
        {
            audioSource.enabled = true;
        }
        void StopAudio()
        {
            audioSource.enabled = false;
        }

        void UnPause()
        {
            activated = true;
            thisObject.SetActive(false);
            Time.timeScale = 1;
            GameStateManager.Instance.SetState(GameState.GamePlay);
        }

    }
}

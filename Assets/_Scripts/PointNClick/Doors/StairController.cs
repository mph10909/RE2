using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class StairController : MonoBehaviour, IOpenable
    {
        [SerializeField] Animator stairAnimations;
        [SerializeField] AnimationClip stairsUp;
        [SerializeField] AnimationClip stairsDown;
        [SerializeField] AudioClip stairStep;


        void PlayFootStep()
        {
            SoundManagement.Instance.PlaySound(stairStep);
        }

        void Pause()
        {
            Cursor.visible = false;
            Time.timeScale = 0;
            GameStateManager.Instance.SetState(GameState.Paused);
        }

        void UpPause()
        {
            Cursor.visible = true;
            Time.timeScale = 1;
            GameStateManager.Instance.SetState(GameState.GamePlay);
            Destroy(this.gameObject);
        }

        public void Open(AudioClip clip)
        {
            stairAnimations.Play(stairsUp.name);
        }

        public void Close(AudioClip clip)
        {
            stairAnimations.Play(stairsDown.name);
        }
    }
}

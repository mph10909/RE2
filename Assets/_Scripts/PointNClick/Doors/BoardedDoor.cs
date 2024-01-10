using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ResidentEvilClone
{
    public class BoardedDoor : LockableDoor, IInteractable
    {

        [Header("FinishDemoStuff")]
        [SerializeField] GameObject doorObstruction;
        [SerializeField] GameObject results;
        [SerializeField] AudioClip finalMusic;
        [SerializeField] CharacterManager charactermanager;
        [SerializeField] float fadeTime;
        [SerializeField] float rangeThreshold = 30f;
        [SerializeField] List<GameObject> charactersNotClose;
        Item doorKey;
        bool doorBlocked = true;

        public override void Interact()
        {
            if (doorBlocked)
            {
                base.Interact();
            }
            else
            {
                if (CheckAllCharactersWithinRange()) { FinishDemo(); }
                else
                {
                    foreach(GameObject character in charactersNotClose)
                    {
                        SoundManagement.Instance.MenuSound(MenuSounds.Decline);
                        TextDisplay("<color=Green>" + character.name.ToString() + "</color>" + " cannot be left behind");
                    }
                }
                
            }

        }

        private void FinishDemo()
        {
            Actions.FadeTrigger += Result;
            Fader.Instance.FadeOut(fadeTime, false);
            
        }

        public override void UnlockDoor(Item key)
        {
            doorKey = key;
            Actions.FadeTrigger += UnblockDoor;
            Fader.Instance.FadeOut(fadeTime, false); 
        }

        public void UnblockDoor()
        {
           
            base.UnlockDoor(doorKey);
            Actions.FadeTrigger -= UnblockDoor;
            doorObstruction.SetActive(false);
            Fader.Instance.FadeIn(fadeTime, true);
            doorBlocked = false;
            
        }

        private void Result()
        {
            Time.timeScale = 0;
            if(finalMusic != null)SoundManagement.Instance.FadeOutIn(fadeTime,finalMusic);
            Actions.FadeTrigger -= Result;
            results.SetActive(true);
            StartCoroutine(CountdownCoroutine(finalMusic.length));
        }

        private IEnumerator CountdownCoroutine(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1;
            SceneManager.LoadScene("Start", LoadSceneMode.Single);
        }

        private bool CheckAllCharactersWithinRange()
        {
            foreach (GameObject character in charactermanager.characters)
            {
                float distance = Vector3.Distance(transform.position, character.transform.position);

                // If any character is outside the range, return false
                if (distance > rangeThreshold)
                {
                    charactersNotClose.Clear();
                    charactersNotClose.Add(character);
                    return false;
                }
                else charactersNotClose.Remove(character);
            }

            // If all characters are within the range, return true
            return true;
        }

    }
}

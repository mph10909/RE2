using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace ResidentEvilClone
{
    [Serializable]
    public enum MenuSounds
    {
        Click,
        Accept,
        Decline,
        Cancel,
    }

    [Serializable]
    public class MenuSoundFX
    {
        public MenuSounds menuSounds;
    }

    [System.Serializable]
    public class MenuSound
    {
        public MenuSounds sound;
        public AudioClip audioClip;
    }

    public class SoundManagement : MonoBehaviour
    {
        public static SoundManagement Instance;
        [SerializeField] AudioSource musicSource, effectSource;
        [SerializeField] MenuSound[] menuSounds;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public AudioClip CurrentSoundTrack { get { return musicSource.clip; } }

        public void MenuSound(MenuSounds sound)
        {
            foreach(MenuSound clip in menuSounds)
            {
                if(clip.sound == sound)
                {
                    effectSource.PlayOneShot(clip.audioClip);
                }
            }
        }

        public void PlaySound(AudioClip clip)
        {
            effectSource.PlayOneShot(clip);
        }

        public void PitchAdjust(float pitch)
        {
            effectSource.pitch = pitch;
        }

        public void PlayMusic(AudioClip clip)
        {
            musicSource.PlayOneShot(clip);
        }

        public void SetTrack(AudioClip newMusic)
        {
            musicSource.clip = newMusic;
        }

        public void SwapTrack(float fadeTime, AudioClip newClip)
        {
            StartCoroutine(FadeOutToIn(fadeTime, newClip));
        }

        public string GetTrack()
        {
            //return musicSource.clip.name;
            return Path.GetFileName(musicSource.clip.name);

        }

        public void FadeOut(float fadeTime)
        {
            StartCoroutine(FadeOutTrack(fadeTime));
        }

        public void FadeIn(float fadeTime, AudioClip newClip)
        {
            StartCoroutine(FadeInTrack(fadeTime, newClip));
        }

        public void FadeOutIn(float fadeTime, AudioClip newClip)
        {
            StartCoroutine(FadeOutToIn(fadeTime, newClip));
        }

        IEnumerator FadeOutTrack(float duration)
        {
            float currentTime = 0;
            float start = musicSource.volume;
            while (currentTime < duration)
            {
                currentTime += Time.unscaledDeltaTime;
                musicSource.volume = Mathf.Lerp(start, 0, currentTime / duration);
                yield return null;
            }
        }

        IEnumerator FadeOutToIn(float duration, AudioClip newClip)
        {
            float currentTime = 0;
            float start = musicSource.volume;
            while (currentTime < duration)
            {
                currentTime += Time.unscaledDeltaTime;
                musicSource.volume = Mathf.Lerp(start, 0, currentTime / duration);
                yield return null;
            }
            StartCoroutine(FadeInTrack(.5f, newClip));
        }

        IEnumerator FadeInTrack(float duration, AudioClip newClip)
        {
            musicSource.clip = newClip;
            musicSource.Play();
            float currentTime = 0;
            float start = musicSource.volume;
            while (currentTime < duration)
            {
                currentTime += Time.unscaledDeltaTime;
                musicSource.volume = Mathf.Lerp(start, 1, currentTime / duration);
                yield return null;
            }
            yield break;
        }


    }
}

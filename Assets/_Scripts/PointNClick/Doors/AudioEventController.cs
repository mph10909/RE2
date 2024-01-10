using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class AudioEventController : MonoBehaviour
    {
        public AudioClip[] audioClips;  // Array of audio clips

        // Method to play audio by index
        public void PlayAudioByIndex(int index)
        {
            if (index >= 0 && index < audioClips.Length)
            {
                SoundManagement.Instance.PlaySound(audioClips[index]);
            }
            else
            {
                Debug.LogWarning("Audio index out of range: " + index);
            }
        }

        // Method to play audio by name
        public void PlayAudioByName(string name)
        {
            foreach (AudioClip clip in audioClips)
            {
                if (clip.name == name)
                {
                    SoundManagement.Instance.PlaySound(clip);
                    return;
                }
            }

            Debug.LogWarning("No audio clip found with name: " + name);
        }
    }
}

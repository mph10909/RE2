using UnityEngine;

namespace ResidentEvilClone
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceHandler : MonoBehaviour, IComponentSavable
    {
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (ResidentEvilClone.PersistAcrossScenes.IsParentDestroyed)
            {
                // If you don't want to continue executing the rest of Awake, return
                return;
            }
        }

        public string GetSavableData()
        {
            // Get the name of the currently playing audio clip.
            // Ensure there's a clip playing, otherwise save an empty string.
            string clipName = audioSource.clip ? audioSource.clip.name : string.Empty;

            // Convert isPlaying state and clip name to a string format "clipName|isPlaying".
            return $"{clipName}|{audioSource.isPlaying}";
        }

        public void SetFromSaveData(string savedData)
        {

            // Parse the saved string "clipName|isPlaying".
            string[] parts = savedData.Split('|');

            if (parts.Length != 2)
            {
                Debug.LogError("Invalid saved audio data.");
                return;
            }

            string clipName = parts[0];
            bool shouldPlay = bool.Parse(parts[1]);

            // Find the audio clip by its name in the resources or any other location you're managing audio clips.
            AudioClip clipToPlay = Resources.Load<AudioClip>($"AudioTracks/{clipName}"); // Adjust this if you store your audio elsewhere.

            if (clipToPlay)
            {
                audioSource.clip = clipToPlay;

                // If the clip was playing when saved, play it again.
                if (shouldPlay)
                {
                    audioSource.Play();
                }
            }
            else
            {
                Debug.LogWarning($"Clip named {clipName} not found.");
            }
        }
    }
}

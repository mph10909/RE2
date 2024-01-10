using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace ResidentEvilClone
{
    public class StartScene : MonoBehaviour
    {
        [SerializeField] AudioClip newGameAudio;
        [SerializeField] float fadeInTime;
        [SerializeField] float fadeInDelay;
        [SerializeField] string loadedAudio; 
        [SerializeField] SaveData data;
        [SerializeField] string startText;
        [SerializeField] string loadedStartText;
        DisplayText displayText;
        [SerializeField] TextData startingText;

        void Awake()
        {
            if (Loader.Loaded) Loaded();
        }

        void Start()
        {    
            //if (Loader.Loaded) { Fader.Instance.FadeInText(fadeInTime, true, loadedStartText, fadeInDelay); }
            //else
            //{
            //    Time.timeScale = 0;
            //    Fader.Instance.FadeInText(fadeInTime, true, startText, fadeInDelay);
            //    Actions.SetText += StartingDialogue;
            //}
            SoundManagement.Instance.FadeIn(fadeInTime, newGameAudio);
            Cursor.lockState = CursorLockMode.Confined;

        }

        void StartingDialogue()
        {
            Actions.SetText -= StartingDialogue;
            displayText.TextDisplay(startingText.body[0]);
        }

        void Loaded()
        {
                
                print("Loaded " + Loader.SaveFile + ".dat");
                data = SaveManager.GetData(Loader.SaveFile);
                LoadSaveAudio();
        }

        void LoadSaveAudio()
        {
            loadedAudio = data.audioName;
            print(Resources.Load<AudioClip>("AudioTracks/" + loadedAudio));
            newGameAudio = Resources.Load<AudioClip>("AudioTracks/" + loadedAudio);
        }

    }
}

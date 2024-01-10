using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Assistant : MonoBehaviour
{
    public Text messageText;
    TextWriter.TextWriterSingle textWriterSingle;
    [SerializeField] AudioSource soundAudioSource;

    void Start()
    {
        //messageText.text = "";
        
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if(textWriterSingle != null && textWriterSingle.IsActive())
            {
                textWriterSingle.WriteAllAndDestroy();
            }
            else
            {
            string[] messageArray = new string[]
            {
                "Hello_Leon",
                "Hello_Claire",
                "We_must_escape"
            };
            string message = messageArray[Random.Range(0, messageArray.Length)];
            StartTalkingSound();
            textWriterSingle = TextWriter.AddWriter_Static(messageText, message, 0.1f, true, true, StopTalkingSound);
            }

        }
    }

    private void StartTalkingSound()
    {
        soundAudioSource.Play();
    }

    private void StopTalkingSound()
    {
        soundAudioSource.Stop();
    }


}

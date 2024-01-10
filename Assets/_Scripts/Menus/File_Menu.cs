using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class File_Menu : MonoBehaviour
{
    [SerializeField] AudioClip PageFlip;
    [SerializeField] AudioSource PagePlayer; 

    void PageAudio()
    {
        PagePlayer.ignoreListenerPause=true;
        PagePlayer.PlayOneShot(PageFlip);
    }


}


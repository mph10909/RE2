using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResidentEvilClone;

public class PageDisable : MonoBehaviour
{
    [SerializeField] AudioClip PageFlip;
    [SerializeField] AudioSource PagePlayer;
    [SerializeField] GameObject PageLeft, PageRight;
    [SerializeField] FileMenuInventory fileDisable;

    void PageAudio()
    {
        PagePlayer.ignoreListenerPause=true;
        PagePlayer.PlayOneShot(PageFlip);
    }
    void PagesFlipped()
    {
        PageRight.SetActive(false);
        PageLeft.SetActive(false);
    }

    void FlipOn()
    {
        fileDisable.flipping = true;
    }

    void FlipOff()
    {
        fileDisable.flipping = false;
    }
}

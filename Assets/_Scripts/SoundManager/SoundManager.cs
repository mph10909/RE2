using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound{
        menuEnter,
        menuExit,
        menuClick,
        menuDelcine,
        grenadeLaunch,
        grenadeExplode,
        safeKnobTurn,
        safeOpen

    }

    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    public static void PlaySound(Sound sound){
        if(oneShotGameObject == null){
        oneShotGameObject = new GameObject("One Shot Sound");
        oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
        }
        AudioListener.pause = false;
        oneShotAudioSource.ignoreListenerPause = true;
        oneShotAudioSource.PlayOneShot(GetAudioClip(sound));

    }

    private static AudioClip GetAudioClip(Sound sound){
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray)
        {
            if(soundAudioClip.sound == sound){
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found");
        return null;
    }
}

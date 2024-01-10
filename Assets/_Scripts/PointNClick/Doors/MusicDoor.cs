using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
public class MusicDoor : BaseDoor

    {
        public void SetNewTrack(AudioClip clip)
        {
            newSceneTrack = clip;
        }

        public void NullTrack()
        {
            newSceneTrack = null;
        }
    }
}

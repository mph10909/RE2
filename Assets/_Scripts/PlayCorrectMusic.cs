using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class PlayCorrectMusic : MonoBehaviour
    {
        Collider roomCollider;
        [SerializeField] AudioClip music;
        

        void OnEnable()
        {
            roomCollider = GetComponent<Collider>();
            Actions.CharacterSwap += PlayMusic;
        }
        void OnDisable()
        {
            Actions.CharacterSwap -= PlayMusic;
        }


        void PlayMusic(GameObject charSet)
        {
            if (RoomNotEmpty(charSet) && !SameTrack(charSet))
            {
                SoundManagement.Instance.FadeOutIn(0.15F, music);
            }

            else return;
        }

        bool SameTrack(GameObject character)
        {
            if (roomCollider.bounds.Contains(character.transform.position) && SoundManagement.Instance.GetTrack() == music.name) return true;
            else return false;
        }

        bool RoomNotEmpty(GameObject character)
        {
            if (roomCollider.bounds.Contains(character.transform.position)) { return true; }
            else return false;
        }

    }
}

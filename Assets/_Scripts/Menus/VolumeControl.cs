using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using ResidentEvilClone;
using UnityEngine.EventSystems;

public class VolumeControl : MonoBehaviour, IEventSystemHandler
{
    [SerializeField] Color color;
    [SerializeField] MenuSounds menuSound;
    public AudioMixer audioMixer;
    public Text bgmVolumeText, sfxVolumeText;
    [SerializeField][Range(0.0001f, 1)] float musicVolume, sfxVolume;
    [SerializeField] float speed, speedIncrease;
    [SerializeField] bool isHeldDown = false;

    public bool IsHeldDown
    {
        set { isHeldDown = value; print("VolumeCancel");}
    }

    public float SoundFxVolume
    {
        get { return sfxVolume; }
        set
        {
            sfxVolumeText.color = color;
            sfxVolume = Mathf.Clamp(value, 0.0001f, 1);
            float logVolume = Mathf.Log10(sfxVolume) * 20;
            audioMixer.SetFloat("SoundFx", logVolume);
            FxLevel();
        }
    }

    public float MusicVolume
    {
        get { return musicVolume; }
        set
        {
            bgmVolumeText.color = color;
            musicVolume = Mathf.Clamp(value, 0.0001f, 1);
            float logVolume = Mathf.Log10(musicVolume) * 20;
            audioMixer.SetFloat("Music", logVolume);
            MusicLevel();
        }
    }

    void OnEnable()
    {
        audioMixer.GetFloat("Music", out float currentMusicVolume);
        musicVolume = Mathf.Pow(10, currentMusicVolume / 20);
        MusicLevel();

        audioMixer.GetFloat("SoundFx", out float currentFXVolume);
        sfxVolume = Mathf.Pow(10, currentFXVolume / 20);
        FxLevel();
    }




    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
           isHeldDown = false;
           StopAllCoroutines();
       }
    }

    private void FxLevel()
    {
        if (sfxVolume > 0.0001f) { sfxVolumeText.text = (Mathf.RoundToInt(sfxVolume * 100)).ToString(); }
        else { sfxVolumeText.color = Color.white; sfxVolumeText.text = "OFF"; }
    }

    private void MusicLevel()
    {
        if (musicVolume > 0.0001f) { bgmVolumeText.text = (Mathf.RoundToInt(musicVolume * 100)).ToString(); }
        else { bgmVolumeText.color = Color.white; bgmVolumeText.text = "OFF"; }
    }

    public void IncreaseMusicVolume()
    {
        StartCoroutine(HoldChangeVolume(0.01f, true, true));
    }

    public void DecreaseMusicVolume()
    {
        StartCoroutine(HoldChangeVolume(0.01f, false, true));
    }

    public void IncreaseSFXVolume()
    {
        StartCoroutine(HoldChangeVolume(0.01f, true, false));
    }

    public void DecreaseSFXVolume()
    {
        StartCoroutine(HoldChangeVolume(0.01f, false, false));
    }

    public void OnPointerDown()
    {
        isHeldDown = true;
    }

    public void OnPointerUp()
    {
        print("Up");
        isHeldDown = false;
        StopAllCoroutines();
    }

    IEnumerator HoldChangeVolume(float changeAmount, bool isIncreasing, bool isMusic)
    {
        float counter = 0;
        
        while (isHeldDown)
        {
            SoundManagement.Instance.MenuSound(menuSound);
            float newChangeAmount = changeAmount + (counter * speedIncrease);
            counter++;
            if (isIncreasing)
            {
                if (isMusic)
                {
                    MusicVolume += newChangeAmount;
                }
                else
                {
                    SoundFxVolume += newChangeAmount;
                }
            }
            else
            {
                if (isMusic)
                {
                    MusicVolume -= newChangeAmount;
                }
                else
                {
                    SoundFxVolume -= newChangeAmount;
                }
            }
            yield return new WaitForSecondsRealtime(speed);
        }
    }
}



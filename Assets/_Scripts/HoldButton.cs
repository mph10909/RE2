using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public VolumeControl volumeControl;

    private bool isHolding;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        StartCoroutine(HoldChangeVolume());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
    }

    IEnumerator HoldChangeVolume()
    {
        while (isHolding)
        {
            if (this.name == "IncreaseMusic")
            {
                volumeControl.MusicVolume += 0.01f;
            }
            else if (this.name == "DecreaseMusic")
            {
                volumeControl.MusicVolume -= 0.01f;
            }
            else if (this.name == "IncreaseSFX")
            {
                volumeControl.SoundFxVolume += 0.01f;
            }
            else if (this.name == "DecreaseSFX")
            {
                volumeControl.SoundFxVolume -= 0.01f;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}

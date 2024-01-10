using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArrowFlashing : MonoBehaviour
{

    [SerializeField] SpriteRenderer ArrowMarker;
    [SerializeField] Color White;
    [SerializeField] Color Grey;
    [Range(2f, 15f)][SerializeField] float FlashingTime = 10f;
    [Range(0.1f, .5f)][SerializeField] float FlashingSpeed = 0.15f;
    [SerializeField] bool FlashingOn; 

    void Start()
    {
        ArrowMarker.color = White;
        FlashingOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(FlashingOn) {ArrowMarker.color = Color.Lerp(ArrowMarker.color, Grey, Time.unscaledDeltaTime * FlashingTime); StartCoroutine(ColorOn());}
        if(!FlashingOn) {ArrowMarker.color = Color.Lerp(ArrowMarker.color, White, Time.unscaledDeltaTime * FlashingTime); StartCoroutine(ColorOff());}
    }
    
    IEnumerator ColorOn()
    {
        yield return new WaitForSecondsRealtime(FlashingSpeed);
        FlashingOn = false;
    }
    IEnumerator ColorOff()
    {
        yield return new WaitForSecondsRealtime(FlashingSpeed);
        FlashingOn = true;
    }
}

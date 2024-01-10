using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightFlicker : MonoBehaviour
{
    [Header("Light")]
    [SerializeField] Light flickeringLight;

    [Header("Timers")]
    [SerializeField] float intensityTime;
    [SerializeField] float sizeTime;

    [Header("Light Intensity")]
    [SerializeField] float minLightIntensity;
    [SerializeField] float maxLightIntensity;

    [Header("Light Range")]
    [SerializeField] float minLightRange;
    [SerializeField] float maxLightRange;

    [Header("Lerp Speed")]
    [SerializeField] float intensityTimer;
    [SerializeField] float sizeTimer;

    IEnumerator intensityLight;
    IEnumerator sizeLight;

    void OnEnable()
    {
        Components();
    }

    void Awake()
    {
        flickeringLight = GetComponent<Light>();
        
    }

    void Components()
    {
        sizeLight = LightSize();
        intensityLight = LightIntensity();
        StartCoroutine(intensityLight);
        StartCoroutine(sizeLight);
    }


    private IEnumerator LightIntensity()
    {
        float newIntensity;
        newIntensity = Random.Range(minLightIntensity, maxLightIntensity);
        float timer = 0;
        while(timer < intensityTime)
        {
            flickeringLight.intensity = Mathf.Lerp(flickeringLight.intensity, newIntensity, intensityTimer);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return StartCoroutine(LightIntensity());
    }

    private IEnumerator LightSize()
    {
        float newSize;
        newSize = Random.Range(minLightRange, maxLightRange);
        float timer = 0;
        while (timer < sizeTime)
        {
            flickeringLight.range = Mathf.Lerp(flickeringLight.range, newSize, sizeTimer);
            timer += Time.deltaTime;
            yield return null;
        }


        yield return StartCoroutine(LightSize());
    }




}

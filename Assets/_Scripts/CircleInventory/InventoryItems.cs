using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class InventoryItems : MonoBehaviour
{

    AudioSource audioSource;

    int   numObjects = 0;
    float rotationAmount;
    [SerializeField]  Transform parentT;
    [SerializeField]  AudioClip movingAudio;
    [HideInInspector] public bool rotating;
    [SerializeField]  float menuRadius;
    [SerializeField]  float lerpDuration;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        numObjects = parentT.childCount;
        rotationAmount = 360 / numObjects;
        Vector3 center = parentT.position;
        int i = 0;
        foreach(Transform item in parentT)
        {
            int a = -360 / parentT.childCount* i;
            Vector3 pos = RandomCircle(center, a, menuRadius);
            item.position = pos;
            item.rotation = Quaternion.identity;
            i++;
        }
        parentT.localPosition = new Vector3(45, 0, 0);
        parentT.localEulerAngles = new Vector3(15, 0, 0);
    }

    Vector3 RandomCircle(Vector3 center, int a, float radius)
    {
        Debug.Log(a);
        float ang = a;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.z = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = center.z;
        return pos;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A) && !rotating)
        {
            StartCoroutine(Rotate());
        }
        if (Input.GetKey(KeyCode.D) && !rotating)
        {
            StartCoroutine(RotateBack());
        }
    }

    IEnumerator Rotate()
    {
        rotating = true;
        audioSource.PlayOneShot(movingAudio);
        float timeElapsed = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, rotationAmount, 0);
        while (timeElapsed < lerpDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
        rotating = false;
    }

    IEnumerator RotateBack()
    {
        rotating = true;
        audioSource.PlayOneShot(movingAudio);
        float timeElapsed = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, rotationAmount *-1, 0);
        while (timeElapsed < lerpDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
        rotating = false;
    }
}

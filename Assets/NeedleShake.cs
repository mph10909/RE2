using System.Collections;
using UnityEngine;

public class NeedleShake : MonoBehaviour
{
    public float shakeAmount = 10f; // The amount to shake the needle
    public float shakeDuration = 0.5f; // The duration of the shake
    public float shakeSpeed = 20f; // The speed of the shake

    private Quaternion startRotation; // The starting rotation of the needle
    private bool isShaking; // Whether the needle is currently shaking

    private void Start()
    {
        startRotation = transform.rotation; // Store the starting rotation of the needle
    }

    private void Update()
    {
        if (isShaking)
        {
            // Shake the needle
            transform.rotation = startRotation * Quaternion.AngleAxis(Mathf.Sin(Time.time * shakeSpeed) * shakeAmount, Vector3.forward);
        }
    }

    public void Shake()
    {
        if (!isShaking)
        {
            // Start shaking the needle
            isShaking = true;
            StartCoroutine(StopShaking());
        }
    }

    private IEnumerator StopShaking()
    {
        // Wait for the duration of the shake
        yield return new WaitForSeconds(shakeDuration);

        // Stop shaking the needle
        isShaking = false;

        // Reset the rotation of the needle
        transform.rotation = startRotation;
    }
}


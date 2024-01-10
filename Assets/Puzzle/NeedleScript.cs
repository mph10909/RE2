using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NeedleScript : MonoBehaviour
{
    [SerializeField] private Image needleImage;
    [SerializeField] private Transform needleTransform;
    [SerializeField] private float needleMoveDuration = 1f;
    [SerializeField] private float shakeMagnitude = 10f;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private SwitchManager switchManager;

    private float currentAngle = 0f;
    public bool isNeedleMoving = false;

    private IEnumerator MoveNeedle(float targetPosition)
    {
        isNeedleMoving = true;

        float initialPosition = needleTransform.eulerAngles.z;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / needleMoveDuration;
            float zRotation = Mathf.Lerp(initialPosition, targetPosition, t);
            needleTransform.eulerAngles = new Vector3(needleTransform.eulerAngles.x, needleTransform.eulerAngles.y, zRotation);
            yield return null;
        }

        float overshootAmount;
        int sign = Math.Sign(targetPosition);
        if(sign == -1)
        {
            overshootAmount = UnityEngine.Random.Range(-5f, -15f);
        }
        else
        {
            overshootAmount = UnityEngine.Random.Range(5f, 15f);
        }
        // The amount to move past the target position
        float overshootDuration = UnityEngine.Random.Range(0.2f, 0.4f); // The duration of the overshoot animation

        // Move past the target position
        float overshootPosition = targetPosition + overshootAmount;
        t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / overshootDuration;
            float zRotation = Mathf.Lerp(targetPosition, overshootPosition, t);
            needleTransform.eulerAngles = new Vector3(needleTransform.eulerAngles.x, needleTransform.eulerAngles.y, zRotation);
            yield return null;
        }

        // Animate back to the target position
        t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime  / overshootDuration;
            float zRotation = Mathf.Lerp(overshootPosition, targetPosition, t);
            needleTransform.eulerAngles = new Vector3(needleTransform.eulerAngles.x, needleTransform.eulerAngles.y, zRotation);
            yield return null;
        }
        isNeedleMoving = false;
        switchManager.CheckSwitchPattern();

    }

    public void UpdateNeedle(bool switchState, int movementAmount)
    {
        int needleMoveAmount = movementAmount;

        if (movementAmount < 0)
        {
            if (switchState)
                SetNeedle(needleMoveAmount);
            else
                SetNeedle(-needleMoveAmount);
        }
        else
        {
            int adjustedAmount = switchState ? movementAmount : -movementAmount;
            SetNeedle(adjustedAmount);
        }
    }


    public void SetNeedle(float amount)
    {
        if (isNeedleMoving)
        {
            return;
        }

        float newZRotation = needleTransform.eulerAngles.z + amount;
        StartCoroutine(MoveNeedle(newZRotation));
    }

}

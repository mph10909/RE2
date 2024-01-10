using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwitchScript : MonoBehaviour
{
    public bool switchState = false;
    public float switchSlideDuration = 0.5f;
    public float switchSlideDistance = 1f;
    public int needleMovement = 10;
    public NeedleScript needleScript;

    private Vector3 switchStartPosition;
    private Vector3 switchEndPosition;

    private Coroutine currentSlideCoroutine = null;

    public bool selected = false;
    public Material selectedMaterial;
    public Material originalMaterial;

    void Start()
    {
        switchStartPosition = transform.localPosition;
        switchEndPosition = switchStartPosition - new Vector3(0, switchSlideDistance, 0);

        if (switchState) transform.localPosition = switchEndPosition;
    }

    private IEnumerator SlideSwitch(bool targetState)
    {
        if (currentSlideCoroutine != null)
        {
            StopCoroutine(currentSlideCoroutine);
        }

        currentSlideCoroutine = StartCoroutine(SlideSwitchCoroutine(targetState));

        yield return currentSlideCoroutine;
    }

    private IEnumerator SlideSwitchCoroutine(bool targetState)
    {
        float t = 0f;
        Vector3 startPosition = targetState ? switchStartPosition : switchEndPosition;
        Vector3 endPosition = targetState ? switchEndPosition : switchStartPosition;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / switchSlideDuration;
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        transform.localPosition = endPosition;
        switchState = targetState;
        needleScript.UpdateNeedle(switchState, needleMovement);
        currentSlideCoroutine = null;
    }

    public void ToggleSwitchState()
    {
        bool targetState = !switchState;
        StartCoroutine(SlideSwitch(targetState));
    }

    public void SetSwitchState(bool state)
    {
        bool targetState = state != switchState;
        StartCoroutine(SlideSwitch(targetState));
    }

    public void SetSelected(bool value)
    {
        selected = value;

        if (selected)
        {
            GetComponent<Renderer>().material = selectedMaterial;
        }
        else
        {
            GetComponent<Renderer>().material = originalMaterial;
        }
    }

    public void PuzzleFinished()
    {
        GetComponent<Renderer>().material = originalMaterial;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaLerp : MonoBehaviour
{
    public Image targetImage;
    public float targetAlpha;
    public float startAlpha;
    public float percent;

    // Update is called once per frame
    void Update()
    {
        var TC = targetImage.color;

        targetImage.color = new Color(TC.r, TC.g, TC.b, Mathf.Lerp(startAlpha, targetAlpha, 2));
    }
}

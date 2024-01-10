using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenFade : TweenData
{
    public Image targetImage;
    public float targetAlpha;
    public float startAlpha;

    public override void Update()
    {
        base.Update();
        
        float percent = elapseDuration / totalDuration;

        var TC = targetImage.color;

        targetImage.color = new Color(TC.r, TC.g, TC.b, Mathf.Lerp(startAlpha, targetAlpha, percent));
    }
}

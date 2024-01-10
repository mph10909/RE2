using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialItemAsset : MonoBehaviour
{
    public static TutorialItemAsset Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Sprite firstAidSprite;
    public Sprite keySprite;
    public Sprite weaponSprite;
}

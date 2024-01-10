using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


[Serializable]
public class TutorialItem
{
    public enum TutItem
    {
        None,
        FirstAid,
        Key,
        Weapon
    }

    public TutItem item;
    public int amount;

    public TutItem GetItem()
    {
        return item;
    }

    public Sprite GetSprite()
    {
        if (TutorialItemAsset.Instance == null) return null;

        switch (item)
        {
            default:
            case TutItem.FirstAid:  return TutorialItemAsset.Instance.firstAidSprite;
            case TutItem.Key:       return TutorialItemAsset.Instance.keySprite;
            case TutItem.Weapon:    return TutorialItemAsset.Instance.weaponSprite;
        }

    }

    public bool IsStackable()
    {
        switch (item)
        {
            default:
            case TutItem.FirstAid:
            case TutItem.Key:
            case TutItem.Weapon:
            return false;
        }
    }
}




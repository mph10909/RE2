using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveKnife : MonoBehaviour
{
    [SerializeField]
    Animator anim;
    Transform playerHolding;

    void Awake()
    {
        foreach (Transform child in this.transform.root)
        {
            if (child.tag == "Player")
            {
                playerHolding = child;
            }
        }
        anim = playerHolding.GetComponentInChildren<Animator>();
    }

    void OnDestroy()
    {
        if (anim.GetBool("Knife"))
        {
            anim.SetBool("Knife", false);
        }
    }
    }

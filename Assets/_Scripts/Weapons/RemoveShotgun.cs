using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveShotgun : MonoBehaviour
{
    [SerializeField]
    Animator anim;
    Transform playerHolding;

    void Awake()
    {
        Transform currentTransform = this.transform;

        while (currentTransform != null)
        {
            if (currentTransform.tag == "Player")
            {
                playerHolding = currentTransform;
                break; // Exit the while loop once the player is found
            }

            currentTransform = currentTransform.parent;
        }

        if (playerHolding != null)
        {
            anim = playerHolding.GetComponentInChildren<Animator>();
        }
        else
        {
            Debug.LogWarning("Player not found in the hierarchy!");
        }

        anim = playerHolding.GetComponentInChildren<Animator>();
    }

    void OnDestroy()
    {
        if (anim.GetBool("Shotgun"))
        {
            anim.SetBool("Shotgun", false);
        }
    }
    }

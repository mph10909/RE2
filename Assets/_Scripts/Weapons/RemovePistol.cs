using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovePistol : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform playerHolding;

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
        //foreach (Transform child in this.transform.root)
        //{
        //    print(child.name);
        //    if (child.tag == "Player")
        //    {

        //        playerHolding = child;

        //    }
        //}
        anim = playerHolding.GetComponentInChildren<Animator>();
    }

    void OnDestroy()
    {
        if (anim.GetBool("Pistol"))
        {
            anim.SetBool("Pistol", false);
        }
    }

    }

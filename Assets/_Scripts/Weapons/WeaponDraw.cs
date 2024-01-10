using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponDraw : MonoBehaviour
{
    [SerializeField] Animator animator;
    float turn;
    
    const string AIMING = "Aiming";



    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Cursor.visible = false;
            animator.SetBool(AIMING, true);
            AimUp();
            AimDown();
            Fire();
            CharacterRotater();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Cursor.visible = true;
            animator.SetBool(AIMING, false);
        }
    }

    void AimUp()
    {
        if(animator.GetBool("AimDown"))return;
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("AimUp", true);
        }
        else
        {
            animator.SetBool("AimUp", false);
        }
    }

    void AimDown()
    {
        if (animator.GetBool("AimUp")) return;
        if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("AimDown", true);
        }
        else
        {
            animator.SetBool("AimDown", false);
        }
    }

    void Fire()
    {
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("Fire", true);
        }
        else
        {
            animator.SetBool("Fire", false);
        }
    }

    void CharacterRotater()
    {
        
        turn = Input.GetAxis("Mouse X");
        transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, turn * -2, 0);

    }


}

    

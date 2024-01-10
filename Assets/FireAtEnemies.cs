using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

    public class FireAtEnemies : MonoBehaviour
    {
        [SerializeField] Animator anim;   
        [SerializeField] Transform WeaponSlot;    
        PointAndClick _pointAndClick;
        int weapon;

        const string FIRE = "Fire";
        const string AIMER = "Aimer", AIMING = "Aiming";
        const string ATTACKED = "Attack";
        const string RELOAD = "Reload";

        public PointAndClick CharacterControl { get { return _pointAndClick; } }

        public bool Firing { get { return anim.GetBool("Firing"); } }
        private bool isFiring = false;

        public bool SetFire { get { return isFiring; }  set { if (value && !isFiring) { anim.SetTrigger("Fire"); isFiring = true; } else if(!value && isFiring) { anim.ResetTrigger("Fire"); isFiring = false; } }  }


        void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            weapon = Animator.StringToHash("Weapon");;
        }

        void OnDisable()
        {
            anim.ResetTrigger("ReloadTrigger");
            SetFire = false;
        }


        public void Fire()
        {
            if (anim.GetBool(RELOAD)) return;
            if (!anim.GetBool(AIMING)) return;
            if (SetFire) return;
            // Loop through the children of WeaponSlot to find the weapon implementing IFireable and fire it.
            foreach (Transform child in WeaponSlot)
            {
                var fireableWeapon = child.GetComponent<IFireable>();
                if (fireableWeapon != null)
                {
                    print(transform.gameObject.name);
                    fireableWeapon.AIFire();
                    break;  // exit loop after firing the first found weapon
                }
            }
        }
    }


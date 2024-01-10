using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class WeaponSet : MonoBehaviour
    {
        
        [SerializeField] Animator animator;
        [SerializeField] int weaponslot;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public int Weapon { set { animator.SetFloat("Weapon", value); } }

        void OnClick_SetWeapon()
        {
            Weapon = weaponslot;
        }
    }
}

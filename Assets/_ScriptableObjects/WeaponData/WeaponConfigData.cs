using UnityEngine;
using UnityEngine.UI;
using ResidentEvilClone;

[CreateAssetMenu(fileName = "New WeaponConfiguration", menuName = "ResidentEvilClone/Weapon Data/Configuration")]
public class WeaponConfigData : ScriptableObject
{
        public WeaponStruct weaponEffects;
        public LayerMask shootMask;

        [Header("Ammo Managment")]
        public AmmoType ammoType;
        public int ammoPerShot = 1;
        public int fullAmmo = 6;

        [Header("Aiming Up")]
        public int upWardDamage = 25;
        public ZombieBody head;
        public bool removeHead;
        public bool kockbackHead;

        [Header("Aiming Forward")]
        public int fowardDamage = 25;
        public ZombieBody torso;
        public bool removeTorso;
        public bool knockbackTorso;

        [Header("Aiming Down")]
        public int  downWardDamage = 25;
        public ZombieBody legs;
        public bool removeLegs;
        public bool knockbackLegs;

        [Header("Weapon Damage / Range")]
        public Vector3 boxSize = new Vector3(3,3,3);
        public float m_MaxDistance = 20;
        public float minForce, maxForce;
}




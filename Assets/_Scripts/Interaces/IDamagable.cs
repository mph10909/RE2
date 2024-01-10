using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZombieBody
{
    None,
    Head,
    Torso,
    Legs
}

public interface IDamagable
{
    void Damage(int DamageAmount, GameObject Particle, ZombieBody BodyPart, bool LimbRemoved, bool Isfacing, bool KnockBack , float Force);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public interface IPlayerInput
    {
        Vector2 GetPrimaryAxis();

        float WeaponDirection();
        bool IsRunHeld();
        bool IsAimingHeld();
        bool IsAttackDown();
        bool IsAttackUp();


        void Flush();
    }
}

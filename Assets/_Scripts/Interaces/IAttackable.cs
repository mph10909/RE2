using UnityEngine;
public enum AttackArea
{
    UpperFront,
    LowerFront,
    UpperBack,
    LowerBack
}


public interface IAttackable
{
    
    void Attacked(Transform EnemyAttacking, AttackArea BodySection, int Damage);   
}

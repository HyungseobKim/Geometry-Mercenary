/*!*******************************************************************
\file         AttackPoisoner.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/25/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        AttackPoisoner
\brief		  When the poisoned enemy tries to attack, give all poison damage instantly.
********************************************************************/
public class AttackPoisoner : Poisoner
{
    public SpriteRenderer pentagon;

    public override void Initialize()
    {
        base.Initialize();

        pentagon.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool ApplyDamage(GameObject target)
    {
        if (target == null)
            return false;

        target.GetComponent<UnitBase>().poisonAttack = true;
        return base.ApplyDamage(target);
    }

    public override string GetThirdAbilityDescription()
    {
        return "When the poisoned enemy tries to attack, give all poison damage instantly.";
    }
}

/*!*******************************************************************
\file         HealPoisoner.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/25/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        HealPoisoner
\brief		  When the poisoned enemy gets healing, increases poison damage.
********************************************************************/
public class HealPoisoner : Poisoner
{
    public SpriteRenderer square;

    public override void Initialize()
    {
        base.Initialize();

        square.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool ApplyDamage(GameObject target)
    {
        if (target == null)
            return false;

        target.GetComponent<UnitBase>().poisonHeal = true;
        return base.ApplyDamage(target);
    }

    public override string GetThirdAbilityDescription()
    {
        return "When the poisoned enemy gets healing, increases poison damage.";
    }
}

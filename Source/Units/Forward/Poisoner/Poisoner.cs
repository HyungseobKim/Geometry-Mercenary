/*!*******************************************************************
\file         Poisoner.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/31/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Poisoner
\brief		  Apply poison instead of damage.
********************************************************************/
public class Poisoner : Forward
{
    public SpriteRenderer cross;
    public float poisonDamageModifier = 2.0f;
    public float poisonDamageTime = 3.0f;

    public override void Initialize()
    {
        base.Initialize();

        cross.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool ApplyDamage(GameObject target)
    {
        if (target == null)
            return false;

        target.GetComponent<UnitBase>().ApplyPoison(status.power * poisonDamageModifier, poisonDamageTime);
        return false;
    }

    public override string GetSecondAbilityDescription()
    {
        return "Instead of damage, poisons the enemy.\nPoison gives " + poisonDamageModifier * 100 + "% damage over " + poisonDamageTime + " seconds.";
    }
}

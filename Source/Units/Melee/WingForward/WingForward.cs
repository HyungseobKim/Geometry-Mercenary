/*!*******************************************************************
\file         WingForward.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/01/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        WingForward
\brief		  Give 400% damage with a 50% chance.
********************************************************************/
public class WingForward : Melee
{
    public SpriteRenderer triangle;

    public override void Initialize()
    {
        base.Initialize();

        triangle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool ApplyDamage(GameObject target)
    {
        if (target == null)
            return false;

        float damage = status.power;
        bool critical = false;

        if (Random.Range(0, 2) == 1)
        {
            damage *= 4.0f;
            critical = true;
        }

        return target.GetComponent<UnitBase>().TakeDamage(damage, gameObject, critical);
    }

    public override string GetSecondAbilityDescription()
    {
        return "Give 400% damage with a 50% chance.";
    }
}

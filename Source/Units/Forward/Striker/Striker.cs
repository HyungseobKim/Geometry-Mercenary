/*!*******************************************************************
\file         Striker.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/01/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Striker
\brief		  Gives 200% damage.
********************************************************************/
public class Striker : Forward
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

        // Double damage.
        return target.GetComponent<UnitBase>().TakeDamage(status.power * 2.0f, gameObject);
    }

    public override string GetSecondAbilityDescription()
    {
        return "Gives 200% damage.";
    }
}

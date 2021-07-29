/*!*******************************************************************
\file         Bladesmith.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/26/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Shieldsmith
\brief		  Increases power too.
********************************************************************/
public class Bladesmith : Blacksmith
{
    public SpriteRenderer triangle;

    public override void Initialize()
    {
        base.Initialize();

        triangle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override void ApplyHeal(GameObject target)
    {
        if (target == null)
            return;

        // Heals, and then
        base.ApplyHeal(target);

        // increases shield.
        target.GetComponent<Status>().power += protection;
    }

    public override string GetThirdAbilityDescription()
    {
        return "Increases power too. The amount is same with protection it gives.";
    }
}

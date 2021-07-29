/*!*******************************************************************
\file         Shieldsmith.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/26/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Shieldsmith
\brief		  Give one shield too.
********************************************************************/
public class Shieldsmith : Blacksmith
{
    public SpriteRenderer pentagon;

    public override void Initialize()
    {
        base.Initialize();

        pentagon.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override void ApplyHeal(GameObject target)
    {
        if (target == null)
            return;

        // Heals, and then
        base.ApplyHeal(target);

        // increases shield.
        target.GetComponent<Status>().IncreasesShield();
    }

    public override string GetThirdAbilityDescription()
    {
        return "Give one shield too. It will block one next attack.";
    }
}

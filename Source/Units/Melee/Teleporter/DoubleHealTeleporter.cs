/*!*******************************************************************
\file         DoubleHealTeleporter.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Melee
\brief		  Half of the cooldown to take a turn.
********************************************************************/
public class DoubleHealTeleporter : Teleporter
{
    public SpriteRenderer cross;

    public override void Initialize()
    {
        base.Initialize();

        cross.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override void GetHeal(float amount)
    {
        base.GetHeal(amount * 2.0f);
    }

    public override string GetThirdAbilityDescription()
    {
        return "The amount of healing it takes becomes twice.";
    }
}

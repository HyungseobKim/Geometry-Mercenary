/*!*******************************************************************
\file         ProtectionSlark.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        ProtectionSlark
\brief		  Steal protection too.
********************************************************************/
public class ProtectionSlark : Slark
{
    public SpriteRenderer cross;

    private float steal; //! Amount of steal.

    public override void Initialize()
    {
        base.Initialize();

        cross.color = gameObject.GetComponent<SpriteRenderer>().color;

        steal = status.power / 10.0f;
    }

    public override bool ApplyDamage(GameObject target)
    {
        target.GetComponent<Status>().protection -= steal;
        status.protection += steal;

        return base.ApplyDamage(target);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Steal protection too.";
    }
}

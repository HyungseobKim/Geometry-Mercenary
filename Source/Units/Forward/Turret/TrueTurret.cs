/*!*******************************************************************
\file         TrueTurret.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        TrueTurret
\brief		  The damage it gives ignores all effects.
********************************************************************/
public class TrueTurret : Turret
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

        target.GetComponent<Status>().ChangeHealth(-status.power);

        battleUIManager.CreateDamageUI(target, status.power);

        return target.GetComponent<UnitBase>().IsDied();
    }

    public override string GetThirdAbilityDescription()
    {
        return "The damage it gives ignores all effects.";
    }
}

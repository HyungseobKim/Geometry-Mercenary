/*!*******************************************************************
\file         PowerSlark.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        PowerSlark
\brief		  Steal power too.
********************************************************************/
public class PowerSlark : Slark
{
    public SpriteRenderer pentagon;

    private float steal; //! Amount of steal.

    public override void Initialize()
    {
        base.Initialize();

        pentagon.color = gameObject.GetComponent<SpriteRenderer>().color;

        steal = status.power / 10.0f;
    }

    public override bool ApplyDamage(GameObject target)
    {
        target.GetComponent<Status>().power -= steal;
        status.power += steal;

        return base.ApplyDamage(target);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Steal power too.";
    }
}

/*!*******************************************************************
\file         HoTPriest.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        HoTPriest
\brief		  Healed ally gets a restoration for 5 seconds.
********************************************************************/
public class HoTPriest : Priest
{
    public SpriteRenderer circle;

    public override void Initialize()
    {
        base.Initialize();

        circle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override void ApplyHeal(GameObject target)
    {
        base.ApplyHeal(target);

        target.GetComponent<UnitControl>().ApplyHoT(status.power * 2.0f, 5.0f);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Healed ally gets a restoration for 5 seconds.";
    }
}

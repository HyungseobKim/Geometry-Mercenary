/*!*******************************************************************
\file         SpeedSlark.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        SpeedSlark
\brief		  Steal speed too.
********************************************************************/
public class SpeedSlark : Slark
{
    public SpriteRenderer circle;

    private float steal; //! Amount of steal.

    public override void Initialize()
    {
        base.Initialize();

        circle.color = gameObject.GetComponent<SpriteRenderer>().color;

        steal = status.power / 10.0f;
    }

    public override bool ApplyDamage(GameObject target)
    {
        target.GetComponent<Status>().speed -= steal;
        status.speed += steal;

        return base.ApplyDamage(target);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Steal speed too.";
    }
}

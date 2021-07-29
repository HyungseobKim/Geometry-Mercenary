/*!*******************************************************************
\file         Halfback.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Halfback
\brief		  Reduce target enemy's power too.
********************************************************************/
public class Halfback : DefensiveMidfielder
{
    public SpriteRenderer triangle;

    public override void Initialize()
    {
        base.Initialize();

        triangle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    protected override void UseAbility(GameObject target)
    {
        base.UseAbility(target);

        target.GetComponent<Status>().power -= (protection * 2.0f);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Reduce that enemy's power too.";
    }
}

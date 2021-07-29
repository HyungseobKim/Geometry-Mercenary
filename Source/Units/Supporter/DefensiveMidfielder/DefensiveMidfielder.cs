/*!*******************************************************************
\file         DefensiveMidfielder.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/31/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        DefensiveMidfielder
\brief		  Reduces protection of target enemy.
********************************************************************/
public class DefensiveMidfielder : Supporter
{
    public SpriteRenderer square;

    protected float protection;

    public override void Initialize()
    {
        base.Initialize();

        square.color = gameObject.GetComponent<SpriteRenderer>().color;

        protection = status.power / 10.0f;
    }

    public override void TakeTurn()
    {
        base.TakeTurn();

        UseAbility(targetEnemy);
    }

    protected virtual void UseAbility(GameObject target)
    {
        target.GetComponent<Status>().protection -= protection;
    }

    public override string GetSecondAbilityDescription()
    {
        return "Reduces protection of target enemy when each time it takes a turn. The amount is power/10.";
    }
}

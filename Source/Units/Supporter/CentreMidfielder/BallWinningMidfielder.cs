/*!*******************************************************************
\file         BallWinningMidfielder.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        BallWinningMidfielder
\brief		  Stun target enemy.
********************************************************************/
public class BallWinningMidfielder : CentreMidfielder
{
    public SpriteRenderer triangle;
    public float stunTime = 3.0f;

    public override void Initialize()
    {
        base.Initialize();

        triangle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    protected override void UseAbility(GameObject target)
    {
        base.UseAbility(target);

        target.GetComponent<UnitControl>().ApplyStun(stunTime);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Stun that enemy for " + stunTime + " seconds.";
    }
}

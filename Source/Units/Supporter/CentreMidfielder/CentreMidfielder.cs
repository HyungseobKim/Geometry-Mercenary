/*!*******************************************************************
\file         CentreMidfielder.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/31/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        CentreMidfielder
\brief		  Reduces the speed of the target.
********************************************************************/
public class CentreMidfielder : Supporter
{
    public SpriteRenderer cross;

    public override void Initialize()
    {
        base.Initialize();

        cross.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override void TakeTurn()
    {
        base.TakeTurn();

        if (targetEnemy)
            UseAbility(targetEnemy);
    }

    protected virtual void UseAbility (GameObject target)
    {
        float speed = target.GetComponent<Status>().speed;
        speed = Mathf.Clamp(speed - status.power / 20.0f, 0.0f, speed);
        target.GetComponent<Status>().speed = speed;
    }

    public override string GetSecondAbilityDescription()
    {
        return "Reduces the speed of the target when each time it takes a turn. The amount is power/20.";
    }
}

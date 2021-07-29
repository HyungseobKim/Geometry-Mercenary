/*!*******************************************************************
\file         CounterTank.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        CounterTank
\brief		  Do counterattack when adjacent enemy attack.
********************************************************************/
public class CounterTank : Tank
{
    public SpriteRenderer cross;

    public override void Initialize()
    {
        base.Initialize();

        cross.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool TakeDamage(float amount, GameObject enemy, bool critical = false)
    {
        if (tileManager.DistanceBetween(status.cellPos, enemy.GetComponent<Status>().cellPos) <= status.attackRange)
            Attack(enemy);

        return base.TakeDamage(amount, enemy, critical);
    }

    public override string GetThirdAbilityDescription()
    {
        return "When it gets to attack, do counterattack to that enemy. The enemy must be adjacent to this unit.";
    }
}

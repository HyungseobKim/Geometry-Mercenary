/*!*******************************************************************
\file         LongTank.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        LongTank
\brief		  Taunt one more enemy regardless of distance.
********************************************************************/
public class LongTank : Tank
{
    public SpriteRenderer triangle;

    public override void Initialize()
    {
        base.Initialize();

        triangle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override void TakeTurn()
    {
        base.TakeTurn();

        int count = EnemyList.Count;
        int index = Random.Range(0, count - 2); // Exclude target enemy and goal.

        GameObject target = EnemyList[index];
        index = count;

        while (target == targetEnemy || target.GetComponent<Goal>())
            target = EnemyList[--index];

        Taunt(target);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Taunt one more enemy regardless of distance.";
    }
}

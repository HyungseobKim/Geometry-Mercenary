/*!*******************************************************************
\file         HighPriest.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        HighPriest
\brief		  Heal two more allies randomly.
********************************************************************/
public class HighPriest : Priest
{
    public SpriteRenderer square;

    public override void Initialize()
    {
        base.Initialize();

        square.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override void TakeTurn()
    {
        base.TakeTurn();

        // Grab list of allies.
        if (status.enemy == true)
            AllyList = UnitManager.instance.enemyUnits;
        else
            AllyList = UnitManager.instance.playerUnits;

        int count = AllyList.Count;
        int start = Random.Range(0, count);

        GameObject target1 = null;
        GameObject target2 = null;

        for (int i = start; i < start + count; ++i)
        {
            GameObject ally = AllyList[i % count];

            if (ally == null)
                continue;

            if (ally == targetAlly)
                continue;

            if (target1 == null)
                target1 = ally;
            else if (target1 != ally)
            {
                target2 = ally;
                break;
            }
        }

        if (target1)
            HealAlly(target1);

        if (target2)
            HealAlly(target2);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Heal two more allies randomly.";
    }
}

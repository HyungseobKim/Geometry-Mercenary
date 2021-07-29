/*!*******************************************************************
\file         TotalEnganche.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        TotalEnganche
\brief		  Support entire allies regardless of distance.
********************************************************************/
public class TotalEnganche : Enganche
{
    public SpriteRenderer triangle;

    public override void Initialize()
    {
        base.Initialize();

        triangle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    protected override void SupportAllies()
    {
        // Grab list of allies.
        if (status.enemy == true)
            AllyList = UnitManager.instance.enemyUnits;
        else
            AllyList = UnitManager.instance.playerUnits;

        foreach (GameObject ally in AllyList)
        {
            if (ally == null || ally == gameObject || ally.GetComponent<Goal>())
                continue;

            SupportAlly(ally);
        }
    }

    public override string GetThirdAbilityDescription()
    {
        return "Support entire allies regardless of distance.";
    }
}

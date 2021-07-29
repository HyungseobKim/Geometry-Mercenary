/*!*******************************************************************
\file         TotalBard.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        TotalBard
\brief		  Heals entire allies. Further units get less healing.
********************************************************************/
public class TotalBard : Bard
{
    public SpriteRenderer triangle;

    public override void Initialize()
    {
        base.Initialize();

        triangle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    protected override void HealAllies()
    {
        // Grab list of allies.
        if (status.enemy == true)
            AllyList = UnitManager.instance.enemyUnits;
        else
            AllyList = UnitManager.instance.playerUnits;

        // Give heal to all allies.
        foreach (GameObject ally in AllyList)
            HealAlly(ally);
    }

    public override void ApplyHeal(GameObject target)
    {
        if (target == null)
            return;

        float distance = (float)tileManager.DistanceBetween(status.cellPos, target.GetComponent<Status>().cellPos);
        target.GetComponent<UnitBase>().GetHeal(status.power / distance);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Heals entire allies. Further units get less healing.";
    }
}

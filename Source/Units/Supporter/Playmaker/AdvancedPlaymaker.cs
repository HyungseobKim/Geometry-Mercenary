/*!*******************************************************************
\file         AdvancedPlaymaker.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/24/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        AdvancedPlaymaker
\brief		  When it gains support, apply that to one adjacent ally too.
********************************************************************/
public class AdvancedPlaymaker : Playmaker
{
    public SpriteRenderer square;
    private int canUseAbility = 1; //! Count the number of non-extra turns.

    public override void Initialize()
    {
        base.Initialize();

        square.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override void TakeTurn()
    {
        base.TakeTurn();

        // Each time it takes turn, increases it.
        canUseAbility += 1;
    }

    public override void GetSupport(int amount)
    {
        // Take effect.
        base.GetSupport(amount);
        // Total number of turns - extra turn.
        canUseAbility -= amount;

        // It means there were only regular turns.
        if (canUseAbility <= 0)
            return;

        GameObject ally = FindAllyOnAdjacentTiles();
        if (ally == null)
            return;

        ally.GetComponent<UnitControl>().extraTurn += amount;
        --canUseAbility;
    }

    public override double GetAllyValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.4;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.5;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.9;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.5;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.6;

        return 0.5;
    }

    public override string GetThirdAbilityDescription()
    {
        return "When it gains support, apply that to one adjacent ally too.";
    }
}

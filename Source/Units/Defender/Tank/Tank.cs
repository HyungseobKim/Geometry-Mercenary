/*!*******************************************************************
\file         Tank.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Tank
\brief		  Taunt one enemy when attacks.
********************************************************************/
public class Tank : Defender
{
    public SpriteRenderer pentagon;

    public override void Initialize()
    {
        base.Initialize();

        pentagon.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool Attack(GameObject target)
    {
        // Try to attack first.
        if (base.Attack(target))
            return true; // Target died.

        // If target did not die, taunt.
        Taunt(target);

        return false;
    }

    protected void Taunt(GameObject target)
    {
        target.GetComponent<UnitControl>().tauntingTarget = gameObject;
        BattleUIManager.instance.CreateTauntUI(target);
    }

    public override double GetDangerValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.6;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.1;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.1;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.1;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.5;

        return 0.4;
    }

    public override double GetAllyValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.2;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.9;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.4;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.3;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.5;

        return 0.4;
    }

    public override string GetSecondAbilityDescription()
    {
        return "When attacks, taunt that enemy.";
    }
}

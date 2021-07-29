/*!*******************************************************************
\file         Paladin.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!*******************************************************************
\class        Paladin
\brief		  Paladins restore when they attack.
********************************************************************/
public class Paladin : Defender
{
    public SpriteRenderer cross;

    public override void Initialize()
    {
        base.Initialize();

        cross.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool ApplyDamage(GameObject target)
    {
        if (target == null)
            return false;

        // Store health before attack.
        Status targetStatus = target.GetComponent<Status>();
        float health = targetStatus.health;

        // Attack.
        bool died = target.GetComponent<UnitBase>().TakeDamage(status.power, gameObject);

        // Restore health.
        GetHeal(Mathf.Clamp(health - targetStatus.health, 0.0f, targetStatus.maxHealth - targetStatus.health));

        return died;
    }

    public override double GetDangerValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.3;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.15;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.1;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.2;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.3;

        return 0.2;
    }

    public override double GetAllyValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.2;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.3;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.4;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.2;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.4;

        return 0.3;
    }

    public override string GetSecondAbilityDescription()
    {
        return "When attacks, restore as same as damage given.";
    }
}

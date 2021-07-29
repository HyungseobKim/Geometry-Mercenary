/*!*******************************************************************
\file         Slark.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Slark
\brief		  Steal health instead attack.
********************************************************************/
public class Slark : Defender
{
    public SpriteRenderer triangle;

    public override void Initialize()
    {
        base.Initialize();

        triangle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool ApplyDamage(GameObject target)
    {
        if (target == null)
            return false;

        // Store max health before steal.
        Status targetStatus = target.GetComponent<Status>();
        float maxHealth = targetStatus.maxHealth;

        // Reduce max health and health as well as.
        targetStatus.maxHealth = Mathf.Clamp(targetStatus.maxHealth - status.power, 0.0f, targetStatus.maxHealth);

        float healthChange = Mathf.Clamp(targetStatus.health - targetStatus.maxHealth, 0.0f, targetStatus.health);
        targetStatus.ChangeHealth(-healthChange); // Clamp and change health bar.

        // Increase max health.
        status.maxHealth += (maxHealth - targetStatus.maxHealth);
        GetHeal(healthChange); // Steal reduced health.

        return target.GetComponent<UnitBase>().IsDied();
    }

    public override double GetDangerValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.4;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.15;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.1;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.2;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.6;

        return 0.3;
    }

    public override double GetAllyValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.3;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.9;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.4;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.2;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.4;

        return 0.4;
    }

    public override string GetSecondAbilityDescription()
    {
        return "Instead of damage, reduce the maximum health of the enemy. If it reduced health too, restore as that amount.";
    }
}

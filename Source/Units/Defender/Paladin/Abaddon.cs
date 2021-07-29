/*!*******************************************************************
\file         Abaddon.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/24/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System;
using UnityEngine;

/*!*******************************************************************
\class        Abaddon
\brief		  Abaddons get heal instead damage if their health goes very low.
********************************************************************/
public class Abaddon : Paladin
{
    public SpriteRenderer circle;

    private bool canUseAbility = true;

    public float abilityDuration = 5.0f;
    private float borrowedTime = 0.0f;

    public float abilityUseCondition = 0.3f;
    public float abilityChargeCondition = 0.8f;

    public SpriteRenderer healthBar;
    private Color originalColor;
    private Color abilityColor = new Color(1.0f, 112.0f/255.0f, 40.0f/255.0f, 1.0f);

    public override void Initialize()
    {
        base.Initialize();

        circle.color = gameObject.GetComponent<SpriteRenderer>().color;
        originalColor = healthBar.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (borrowedTime > 0.0f)
        {
            borrowedTime -= Time.deltaTime * GameSpeedController.instance.speed;

            if (borrowedTime <= 0.0f)
                healthBar.color = originalColor;
        }
    }

    public override bool TakeDamage(float amount, GameObject enemy, bool critical = false)
    {
        // If ability is working, get heal instead damage.
        if (borrowedTime > 0.0f)
        {
            GetHeal(amount);
            return false;
        }

        // Take damage.
        amount = Mathf.Clamp(amount - status.protection, 0, amount + Math.Abs(status.protection));
        status.ChangeHealth(-amount);
        battleUIManager.CreateDamageUI(gameObject, amount, critical);

        // If portion of health is low, use ability.
        if (status.health <= status.maxHealth * abilityUseCondition && canUseAbility)
        {
            borrowedTime = abilityDuration;
            canUseAbility = false;
            healthBar.color = abilityColor;
            return false;
        }

        return IsDied();
    }

    public override void GetHeal(float amount)
    {
        base.GetHeal(amount);

        if (status.health >= status.maxHealth * abilityChargeCondition)
            canUseAbility = true;
    }

    public override double GetAllyValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.2;
        else if (scorer.GetComponent<Healer>() != null)
        {
            if (canUseAbility)
                return 0.1;
            else
                return 0.6;
        }
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.4;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.2;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.4;

        return 0.3;
    }

    public override string GetThirdAbilityDescription()
    {
        return "When health goes < " + abilityUseCondition * 100.0f + "%, all damage it takes will restore its health. Recharge when health becomes > " + abilityChargeCondition * 100.0f + "%";
    }
}

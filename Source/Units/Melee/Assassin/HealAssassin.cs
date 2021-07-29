/*!*******************************************************************
\file         HealAssassin.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        HealAssassin
\brief		  Each time it takes damage, it sneaks.
              Restore health gradually during sneaking.
********************************************************************/
public class HealAssassin : Assassin
{
    public SpriteRenderer circle;

    public override void Initialize()
    {
        base.Initialize();

        circle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    void Update()
    {
        if (status.stealth == true)
            status.ChangeHealth(status.power * Time.deltaTime);
    }

    protected override void Stealth()
    {
        base.Stealth();

        circle.color = stealthColor;
    }

    public override void Destealth()
    {
        base.Destealth();

        circle.color = originalColor;
    }

    public override bool TakeDamage(float amount, GameObject enemy, bool critical = false)
    {
        if (base.TakeDamage(amount, enemy, critical))
            return true;

        Stealth();
        return false;
    }

    public override string GetThirdAbilityDescription()
    {
        return "Each time it takes damage, it sneaks. Restore health gradually during sneaking.";
    }
}

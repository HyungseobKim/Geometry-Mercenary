/*!*******************************************************************
\file         DefensiveFullBack.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        DefensiveFullBack
\brief		  Restore health when it succeeds to evade.
********************************************************************/
public class DefensiveFullBack : FullBack
{
    public SpriteRenderer cross;

    public override void Initialize()
    {
        base.Initialize();

        cross.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool TakeDamage(float amount, GameObject enemy, bool critical = false)
    {
        bool result = base.TakeDamage(amount, enemy, critical);

        if (evade)
            GetHeal(amount);

        return result;
    }

    public override string GetThirdAbilityDescription()
    {
        return "Each time it succeeds in evading, restore health.";
    }
}

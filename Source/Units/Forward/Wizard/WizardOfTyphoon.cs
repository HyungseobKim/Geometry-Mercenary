/*!*******************************************************************
\file         WizardOfTyphoon.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/31/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        WizardOfTyphoon
\brief		  Give 50% damage to target. Adjacent enemies get 200% damage.
********************************************************************/
public class WizardOfTyphoon : Wizard
{
    public SpriteRenderer circle;

    public override void Initialize()
    {
        base.Initialize();

        circle.color = gameObject.GetComponent<SpriteRenderer>().color;

        splashDamageModifier = 2.0f;
        damageModifier = 0.5f;
    }

    public override string GetThirdAbilityDescription()
    {
        return "Target enemy takes only 50% of damage. Instead, adjacent enemies get 200% damage.";
    }
}

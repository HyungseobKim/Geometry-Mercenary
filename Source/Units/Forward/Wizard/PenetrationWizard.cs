/*!*******************************************************************
\file         PenetrationWizard.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/24/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        PenetrationWizard
\brief		  Projectile gives splash damage to all enemies which it encounters.
********************************************************************/
public class PenetrationWizard : Wizard
{
    public SpriteRenderer pentagon;

    public override void Initialize()
    {
        base.Initialize();

        pentagon.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override string GetThirdAbilityDescription()
    {
        return "Projectile gives splash damage to all enemies which it encounters.";
    }
}

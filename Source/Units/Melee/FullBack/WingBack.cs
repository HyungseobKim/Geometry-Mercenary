/*!*******************************************************************
\file         WingBack.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!*******************************************************************
\class        WingBack
\brief		  Possibility of evasion increases 10% per adjacent enemy.
********************************************************************/
public class WingBack : FullBack
{
    public SpriteRenderer triangle;

    public override void Initialize()
    {
        base.Initialize();

        triangle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool TakeDamage(float amount, GameObject enemy, bool critical = false)
    {
        bool result = base.TakeDamage(amount, enemy, critical);

        if (evade)
            Attack(enemy);

        return result;
    }

    public override string GetThirdAbilityDescription()
    {
        return "Each time succeeds to evade, gives damage to the enemy that attacked.";
    }
}

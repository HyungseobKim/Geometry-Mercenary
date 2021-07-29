/*!*******************************************************************
\file         CriticalAssassin.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!*******************************************************************
\class        CriticalAssassin
\brief		  Attack in stealth gives x5 damage.
********************************************************************/
public class CriticalAssassin : Assassin
{
    public SpriteRenderer triangle;

    public override void Initialize()
    {
        base.Initialize();

        triangle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    protected override void Stealth()
    {
        base.Stealth();

        triangle.color = stealthColor;
    }

    public override void Destealth()
    {
        base.Destealth();

        triangle.color = originalColor;
    }

    public override string GetThirdAbilityDescription()
    {
        return "Attack in stealth gives x5 damage.";
    }
}

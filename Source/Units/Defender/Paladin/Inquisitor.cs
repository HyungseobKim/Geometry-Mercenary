/*!*******************************************************************
\file         Inquisitor.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Inquisitor
\brief		  Inquisitors have long range.
********************************************************************/
public class Inquisitor : Paladin
{
    public SpriteRenderer triangle;

    public override void Initialize()
    {
        base.Initialize();

        triangle.color = gameObject.GetComponent<SpriteRenderer>().color;

        status.attackRange = 4;
    }

    public override string GetThirdAbilityDescription()
    {
        return "Attack range increases to 4.";
    }
}

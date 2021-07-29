/*!*******************************************************************
\file         DoubleEnganche.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        DoubleEnganche
\brief		  Twice supports.
********************************************************************/
public class DoubleEnganche : Enganche
{
    public SpriteRenderer square;

    public override void Initialize()
    {
        base.Initialize();

        square.color = gameObject.GetComponent<SpriteRenderer>().color;

        extraTurn *= 2;
    }

    public override string GetThirdAbilityDescription()
    {
        return "The amount of extra turn it gives to ally becomes twice.";
    }
}

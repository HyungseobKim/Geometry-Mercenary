/*!*******************************************************************
\file         ForwardSummoner.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        ForwardSummoner
\brief		  Summons forward unit.
********************************************************************/
public class ForwardSummoner : Summoner
{
    public SpriteRenderer triangle;

    public override void Initialize()
    {
        base.Initialize();

        triangle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override string GetThirdAbilityDescription()
    {
        return "The summoned creature gains the ability of the triangle.";
    }
}

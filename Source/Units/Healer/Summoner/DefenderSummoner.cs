/*!*******************************************************************
\file         DefenderSummoner.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        DefenderSummoner
\brief		  Summons defender unit.
********************************************************************/
public class DefenderSummoner : Summoner
{
    public SpriteRenderer square;

    public override void Initialize()
    {
        base.Initialize();

        square.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override string GetThirdAbilityDescription()
    {
        return "The summoned creature gains the ability of the square.";
    }
}

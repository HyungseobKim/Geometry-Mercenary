/*!*******************************************************************
\file         MeleeSummoner.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        MeleeSummoner
\brief		  Summons melee unit.
********************************************************************/
public class MeleeSummoner : Summoner
{
    public SpriteRenderer pentagon;

    public override void Initialize()
    {
        base.Initialize();

        pentagon.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override string GetThirdAbilityDescription()
    {
        return "The summoned creature gains the ability of the pentagon.";
    }
}

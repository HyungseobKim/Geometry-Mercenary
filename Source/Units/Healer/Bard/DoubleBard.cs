/*!*******************************************************************
\file         DoubleBard.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Bard
\brief		  The healing amount is twice.
********************************************************************/
public class DoubleBard : Bard
{
    public SpriteRenderer square;

    public override void Initialize()
    {
        base.Initialize();

        square.color = gameObject.GetComponent<SpriteRenderer>().color;

        healingAmount = 2.0f;
    }

    public override string GetThirdAbilityDescription()
    {
        return "The healing amount is twice.";
    }
}

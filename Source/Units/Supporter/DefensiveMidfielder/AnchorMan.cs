/*!*******************************************************************
\file         AnchorMan.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        AnchorMan
\brief		  The amount of reduced protection is twice.
********************************************************************/
public class AnchorMan : DefensiveMidfielder
{
    public SpriteRenderer pentagon;

    public override void Initialize()
    {
        base.Initialize();

        pentagon.color = gameObject.GetComponent<SpriteRenderer>().color;

        protection *= 2.0f;
    }

    public override string GetThirdAbilityDescription()
    {
        return "The amount of reduced protection is twice.";
    }
}

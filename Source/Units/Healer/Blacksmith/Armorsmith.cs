/*!*******************************************************************
\file         Armorsmith.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/26/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Armorsmith
\brief		  Protection amount is twice.
********************************************************************/
public class Armorsmith : Blacksmith
{
    public SpriteRenderer circle;

    public override void Initialize()
    {
        base.Initialize();

        circle.color = gameObject.GetComponent<SpriteRenderer>().color;

        protection *= 2.0f;
    }

    public override string GetThirdAbilityDescription()
    {
        return "Protection amount is twice.";
    }
}

/*!*******************************************************************
\file         Regista.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Regista
\brief		  Enemy's next restoration will have no effect.
********************************************************************/
public class Regista : DefensiveMidfielder
{
    public SpriteRenderer cross;

    public override void Initialize()
    {
        base.Initialize();

        cross.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    protected override void UseAbility(GameObject target)
    {
        base.UseAbility(target);

        ++target.GetComponent<Status>().healBan;
    }

    public override string GetThirdAbilityDescription()
    {
        return "Enemy's next restoration will have no effect.";
    }
}

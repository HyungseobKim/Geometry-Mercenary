/*!*******************************************************************
\file         FlexibleEnganche.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        FlexibleEnganche
\brief		  Less adjacent allies support more extra turns.
********************************************************************/
public class FlexibleEnganche : Enganche
{
    public SpriteRenderer cross;

    public override void Initialize()
    {
        base.Initialize();

        cross.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    protected override void SupportAllies()
    {
        switch(NumberofAdjacentAllies(status.cellPos))
        {
            case 6:
            case 5:
                extraTurn = 1;
                break;

            case 4:
            case 3:
                extraTurn = 2;
                break;

            case 2:
            case 1:
                extraTurn = 3;
                break;
        }

        base.SupportAllies();
    }

    protected override GameObject SupportAlly(GameObject target)
    {
        GameObject projectile = base.SupportAlly(target);

        if (projectile == null)
            return null;

        projectile.GetComponent<FlexibleSupportProjectile>().extraTurn = extraTurn;
        return projectile;
    }

    public override string GetThirdAbilityDescription()
    {
        return "Less adjacent allies support more extra turns.";
    }
}

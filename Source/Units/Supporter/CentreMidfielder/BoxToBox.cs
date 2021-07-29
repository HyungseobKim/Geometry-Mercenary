/*!*******************************************************************
\file         BoxToBox.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        BoxToBox
\brief		  Reset cooldown of target enemy.
********************************************************************/
public class BoxToBox : CentreMidfielder
{
    public SpriteRenderer square;

    public override void Initialize()
    {
        base.Initialize();

        square.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    protected override void UseAbility(GameObject target)
    {
        base.UseAbility(target);

        target.GetComponent<Status>().ResetCooldown();
    }

    public override string GetThirdAbilityDescription()
    {
        return "Reset cooldown of that enemy.";
    }
}

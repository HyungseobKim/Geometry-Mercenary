/*!*******************************************************************
\file         MovePoisoner.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/25/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        MovePoisoner
\brief		  When the poisoned enemy tries to move, increases poison damage.
********************************************************************/
public class MovePoisoner : Poisoner
{
    public SpriteRenderer circle;

    public override void Initialize()
    {
        base.Initialize();

        circle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool ApplyDamage(GameObject target)
    {
        if (target == null)
            return false;

        target.GetComponent<UnitBase>().poisonMove = true;
        return base.ApplyDamage(target);
    }

    public override string GetThirdAbilityDescription()
    {
        return "When the poisoned enemy tries to move, increases poison damage.";
    }
}

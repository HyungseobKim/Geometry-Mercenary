/*!*******************************************************************
\file         InsideForward.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        InsideForward
\brief		  Critical hit gives extra turn immediately.
********************************************************************/
public class InsideForward : WingForward
{
    public SpriteRenderer cicle;

    public override void Initialize()
    {
        base.Initialize();

        cicle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool ApplyDamage(GameObject target)
    {
        if (target == null)
            return false;

        float damage = status.power;
        bool critical = false;

        if (Random.Range(0, 2) == 1)
        {
            damage *= 4.0f;
            critical = true;

            unitControl.extraTurn += 1;
        }

        return target.GetComponent<UnitBase>().TakeDamage(damage, gameObject, critical);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Critical hit gives extra turn immediately.";
    }
}

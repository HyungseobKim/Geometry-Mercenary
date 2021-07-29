/*!*******************************************************************
\file         TargetMan.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        TargetMan
\brief		  Damage increases proportionally to the target's maximum health.
********************************************************************/
public class TargetMan : Striker
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

        // Double damage.
        float damage = status.power * 2.0f;
        damage += target.GetComponent<Status>().maxHealth * (status.power * 0.5f / 100.0f);

        return target.GetComponent<UnitBase>().TakeDamage(damage, gameObject);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Damage increases proportionally to the target's maximum health.";
    }
}

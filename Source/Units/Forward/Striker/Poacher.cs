/*!*******************************************************************
\file         Poacher.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Poacher
\brief		  Damage increases proportionally to the target's lost health.
********************************************************************/
public class Poacher : Striker
{
    public SpriteRenderer square;

    public override void Initialize()
    {
        base.Initialize();

        square.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool ApplyDamage(GameObject target)
    {
        if (target == null)
            return false;

        // Double damage.
        float damage = status.power * 2.0f;

        Status targetStatus = target.GetComponent<Status>();
        damage += (damage * (1.0f - targetStatus.health / targetStatus.maxHealth));

        return target.GetComponent<UnitBase>().TakeDamage(damage, gameObject);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Damage increases proportionally to the target's lost health.";
    }
}

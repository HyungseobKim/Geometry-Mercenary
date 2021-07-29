/*!*******************************************************************
\file         Trequartista.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Trequartista
\brief		  Damage increases proportionally to the target's current health.
********************************************************************/
public class Trequartista : Striker
{
    public SpriteRenderer cross;

    public override void Initialize()
    {
        base.Initialize();

        cross.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool ApplyDamage(GameObject target)
    {
        if (target == null)
            return false;

        // Double damage.
        float damage = status.power * 2.0f;
        damage += target.GetComponent<Status>().health * (status.power / 100.0f);

        return target.GetComponent<UnitBase>().TakeDamage(damage, gameObject);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Damage increases proportionally to the target's current health.";
    }
}

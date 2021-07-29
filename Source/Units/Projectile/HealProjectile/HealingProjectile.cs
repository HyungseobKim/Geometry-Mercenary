/*!*******************************************************************
\file         HealingProjectile.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/23/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Projectile
\brief		  Heals target instead of damaging.
********************************************************************/
public class HealingProjectile : Projectile
{
    protected override bool ArrivedToDestination()
    {
        // Not arrived yet.
        if (tilemap.WorldToCell(gameObject.transform.position) != tilemap.WorldToCell(destination))
            return false;

        if (target)
            owner.GetComponent<UnitBase>().ApplyHeal(target);

        Destroy(gameObject);
        return true;
    }
}

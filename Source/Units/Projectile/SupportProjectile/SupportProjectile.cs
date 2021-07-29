/*!*******************************************************************
\file         SupportProjectile.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/24/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        SupportProjectile
\brief		  Supports target instead of damaging.
********************************************************************/
public class SupportProjectile : Projectile
{
    protected override bool ArrivedToDestination()
    {
        // Not arrived yet.
        if (tilemap.WorldToCell(gameObject.transform.position) != tilemap.WorldToCell(destination))
            return false;

        if (target)
            owner.GetComponent<Supporter>().GiveExtraTurn(target);

        Destroy(gameObject);
        return true;
    }
}

/*!*******************************************************************
\file         ChainSplashProjectile.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        ChainSplashProjectile
\brief		  Shoot common splash projectiles to adjacent enemies.
********************************************************************/
public class ChainSplashProjectile : Projectile
{
    protected override bool ArrivedToDestination()
    {
        // Not arrived yet.
        if (tilemap.WorldToCell(gameObject.transform.position) != tilemap.WorldToCell(destination))
            return false;

        if (target)
            owner.GetComponent<ChainWizard>().ChainAttack(target, gameObject);

        Destroy(gameObject);
        return true;
    }
}

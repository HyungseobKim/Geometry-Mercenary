/*!*******************************************************************
\file         FlexibleSupportProjectile.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        FlexibleSupportProjectile
\brief		  Owner can set the amount of supports.
********************************************************************/
public class FlexibleSupportProjectile : Projectile
{
    public int extraTurn;

    protected override bool ArrivedToDestination()
    {
        // Not arrived yet.
        if (tilemap.WorldToCell(gameObject.transform.position) != tilemap.WorldToCell(destination))
            return false;

        if (target)
            target.GetComponent<UnitControl>().extraTurn += extraTurn;

        Destroy(gameObject);
        return true;
    }
}

/*!*******************************************************************
\file         CriticalAssassinProjectile.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        CriticalAssassinProjectile
\brief		  Projectile for critical assassin which gives extra damage.
********************************************************************/
public class CriticalAssassinProjectile : Projectile
{
    private float damage;
    private bool critical = false;

    /*!
     * \brief Check whether owner is in stealth or not when owenr shot this.
     */
    public override void Initialize(GameObject targetObject, GameObject ownerObject)
    {
        base.Initialize(targetObject, ownerObject);

        Status status = ownerObject.GetComponent<Status>();
        damage = status.power;

        if (status.stealth == true)
        {
            damage *= 5.0f;
            critical = true;
        }
    }

    protected override bool ArrivedToDestination()
    {
        // Not arrived yet.
        if (tilemap.WorldToCell(gameObject.transform.position) != tilemap.WorldToCell(destination))
            return false;

        if (target)
            target.GetComponent<UnitBase>().TakeDamage(damage, owner, critical);

        Destroy(gameObject);
        return true;
    }
}

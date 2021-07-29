/*!*******************************************************************
\file         Cannavaro.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/24/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Cannavaro
\brief		  Remove all enemy projectiles collide with Cannavaro.
              It does not affect melee attacks.
********************************************************************/
public class Cannavaro : Stopper
{
    public SpriteRenderer cross;

    private Sprite projectileSprite;

    public override void Initialize()
    {
        base.Initialize();

        cross.color = gameObject.GetComponent<SpriteRenderer>().color;

        projectileSprite = projectilePrefab.GetComponent<SpriteRenderer>().sprite;
    }

    public override bool InteractWithProjectile(GameObject projectile)
    {
        // Cannot ignore melee attack.
        if (projectile.GetComponent<SpriteRenderer>().sprite == projectileSprite)
            return false;

        // Ally's projectile.
        if (projectile.GetComponent<Projectile>().enemy == status.enemy)
            return false;

        // Destroy it.
        Destroy(projectile);
        return true;
    }

    public override string GetThirdAbilityDescription()
    {
        return "Remove all enemies' projectiles that pass this unit. It does not affect melee attacks.";
    }
}

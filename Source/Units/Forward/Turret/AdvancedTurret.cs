/*!*******************************************************************
\file         AdvancedTurret.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        AdvancedTurret
\brief		  If it attacks the same enemy in a row, it shoots one more projectile to that enemy.
********************************************************************/
public class AdvancedTurret : Turret
{
    public SpriteRenderer pentagon;

    private GameObject previousTarget = null;

    public override void Initialize()
    {
        base.Initialize();

        pentagon.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool Attack(GameObject target)
    {
        if (target == null)
            return false;

        if (target == previousTarget)
        {
            Vector3 position = gameObject.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0.0f);

            GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab, position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Initialize(target, gameObject);
        }

        previousTarget = target;
        return base.Attack(target);
    }

    public override string GetThirdAbilityDescription()
    {
        return "If it attacks the same enemy in a row, it shoots one more projectile to that enemy.";
    }
}

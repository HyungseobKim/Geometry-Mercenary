/*!*******************************************************************
\file         MultipleTurret.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        MultipleTurret
\brief		  Attacks two additional random target.
********************************************************************/
public class MultipleTurret : Turret
{
    public SpriteRenderer cross;

    public override void Initialize()
    {
        base.Initialize();

        cross.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool Attack(GameObject target)
    {
        if (target == null)
            return false;

        int count = EnemyList.Count;
        int start = Random.Range(0, count);

        GameObject target1 = null;
        GameObject target2 = null;

        for (int i = start; i < start + count; ++i)
        {
            GameObject enemy = EnemyList[i % count];

            if (enemy == null || enemy == target || enemy.GetComponent<Goal>())
                continue;

            if (target1 == null)
                target1 = enemy;
            else if (target1 != enemy)
            {
                target2 = enemy;
                break;
            }
        }

        if (target1)
        {
            GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab, gameObject.transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Initialize(target1, gameObject);
        }

        if (target2)
        {
            GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab, gameObject.transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Initialize(target2, gameObject);
        }

        return base.Attack(target);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Each time it attacks, attack two more random enemies which is not the goal.";
    }
}

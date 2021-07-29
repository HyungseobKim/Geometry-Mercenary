/*!*******************************************************************
\file         ChainWizard.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        ChainWizard
\brief		  Splash attacks trigger one more splash attack with 50% of damage.
********************************************************************/
public class ChainWizard : Wizard
{
    public SpriteRenderer cross;

    public GameObject rangedProjectilePrefab;

    public override void Initialize()
    {
        base.Initialize();

        cross.color = gameObject.GetComponent<SpriteRenderer>().color;

        damageModifier = 0.5f;
        splashDamageModifier = 0.25f;
    }

    public void ChainAttack(GameObject target, GameObject chainProjectile)
    {
        if (target == null)
            return;

        Vector3Int targetPos = target.GetComponent<Status>().cellPos;
        int row = targetPos.y & 1;

        for (int i = 0; i < 6; ++i)
        {
            Vector3Int cellPos = new Vector3Int(adjacentX[row, i], adjacentY[i], 0) + targetPos;
            Cell cell;

            if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
                continue;

            if (cell.owner == null)
                continue;

            if (cell.owner.GetComponent<Status>().enemy == status.enemy)
                continue;

            // Shoot normal projectile to nearby enemies.
            GameObject projectile = UnityEngine.Object.Instantiate(rangedProjectilePrefab, chainProjectile.transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Initialize(cell.owner, gameObject);
        }

        // Give 100% damage to original target.
        target.GetComponent<UnitBase>().TakeDamage(status.power, gameObject);
    }

    public override string GetThirdAbilityDescription()
    {
        return "Splash attacks trigger one more splash attack with 50% of damage.";
    }
}

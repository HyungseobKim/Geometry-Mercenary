/*!*******************************************************************
\file         CombatMedic.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        CombatMedic
\brief		  CombatMedics can heal adjacent allies.
********************************************************************/
public class CombatMedic : Paladin
{
    public SpriteRenderer pentagon;

    public override void Initialize()
    {
        base.Initialize();

        pentagon.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool ApplyDamage(GameObject target)
    {
        if (target == null)
            return false;

        // Store health before attack.
        Status targetStatus = target.GetComponent<Status>();
        float health = targetStatus.health;

        // Attack.
        bool died = target.GetComponent<UnitBase>().TakeDamage(status.power, gameObject);

        // Restore health.
        float heal = Mathf.Clamp(health - targetStatus.health, 0.0f, targetStatus.maxHealth - targetStatus.health);
        GetHeal(heal);
        HealAllies(heal);

        return died;
    }

    private void HealAllies(float amount)
    {
        int row = status.cellPos.y & 1;
        Cell cell;

        for (int i = 0; i < 6; ++i)
        {
            Vector3Int cellPos = new Vector3Int(adjacentX[row, i], adjacentY[i], 0) + status.cellPos;

            if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
                continue;

            if (cell.owner == null)
                continue;

            if (cell.owner.GetComponent<Status>().enemy == status.enemy)
                cell.owner.GetComponent<UnitBase>().GetHeal(amount);
        }
    }

    public override string GetThirdAbilityDescription()
    {
        return "Restoration heals adjacent allies too.";
    }
}

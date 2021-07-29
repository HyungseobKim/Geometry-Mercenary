/*!*******************************************************************
\file         Creature.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/31/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Creature
\brief		  Have no ability. High speed. Always rush to enemies.
********************************************************************/
public class Creature : UnitBase
{
    public override void TakeTurn()
    {
        eachTileScores.Clear();
        ComputeEnemyScore();

        bool attacked = false;

        // If best enemy is near, attack.
        if (CanAttack(targetEnemy))
        {
            Attack(targetEnemy);
            attacked = true;
        }

        Cell currentCell = tileManager.cells[status.cellPos];
        Cell enemyCell = tileManager.cells[targetEnemy.GetComponent<Status>().cellPos];

        // Move to best enemy
        if (MoveTo(Pathfinder.instance.GetNextPos(currentCell, enemyCell)) < 0)
            return; // Ambushed.

        // Already attacked, so finish the turn.
        if (attacked == true)
            return;

        // If best enemy is near, attack and finish the turn.
        if (CanAttack(targetEnemy))
        {
            Attack(targetEnemy);
            return;
        }

        // Find enemey nearby
        GameObject target = FindEnemyOnAdjacentTiles();

        // If there is, attack.
        if (target != null)
            Attack(target);
    }

    public override double GetDangerValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.4;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.3;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.2;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.5;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.2;

        return 0.3;
    }

    public override double GetAllyValue(GameObject scorer)
    {
        // No ability
        return 0.1;
    }

    public override string GetBaseAbilityDescription()
    {
        return "It has high speed, and it tries to rush to enemies.";
    }
}

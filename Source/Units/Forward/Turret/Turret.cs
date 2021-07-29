/*!*******************************************************************
\file         Turret.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/01/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Linq;
using UnityEngine;

/*!*******************************************************************
\class        Turret
\brief		  If it does not move, reduce the cooldown by 80%.
********************************************************************/
public class Turret : Forward
{
    public SpriteRenderer circle;

    // Ability
    private bool moved = false;

    public override void Initialize()
    {
        base.Initialize();

        circle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override void TakeTurn()
    {
        eachTileScores.Clear();

        // Find best enemy
        ComputeEnemyScore();
        eachTileScores.Clear();

        eachTileScores.Add(status.cellPos, 0.2); // Advantage from ability.
        ComputeThreatenScore();

        if (targetEnemy == null)
            return;

        bool attacked = false;
        int distance = TileManager.instance.DistanceBetween(status.cellPos, targetEnemy.GetComponent<Status>().cellPos);

        GameObject nearEnemy = FindEnemyInRange();

        // If best enemy is in range, attack.
        if (CanAttack(targetEnemy))
        {
            Attack(targetEnemy);

            attacked = true;
        }
        else if (distance > status.attackRange + 1)
        {
            if (nearEnemy != null)
            {
                Attack(nearEnemy);

                attacked = true;
            }
        }

        bool thisTurnMoving = false;
        
        if (attacked == false && nearEnemy == null) // If threr is no enemy in range, force to move.
        {
            int moveResult = MoveTo(Pathfinder.instance.GetNextPos(
                                TileManager.instance.cells[status.cellPos],
                                TileManager.instance.cells[targetEnemy.GetComponent<Status>().cellPos]));

            if (moveResult > 0)
                thisTurnMoving = true;
            else if (moveResult < 0)
                return; // Failed.
        }
        else
        {
            Vector3Int nextPos = eachTileScores.Aggregate((lhs, rhs) => lhs.Value > rhs.Value ? lhs : rhs).Key;

            if (nextPos != status.cellPos)
            {
                int moveResult = MoveTo(nextPos);

                if (moveResult > 0)
                    thisTurnMoving = true;
                else if (moveResult < 0)
                    return; // Failed.
            }
        }

        // Save for next turn.
        moved = thisTurnMoving;

        // Already attacked, so finish the turn.
        if (attacked == true)
            return;

        // If best enemy is in range, attack and finish the turn.
        if (CanAttack(targetEnemy))
        {
            Attack(targetEnemy);
            return;
        }

        // Attack best enemy inside range.
        Attack(FindEnemyInRange());
    }

    public override bool Attack(GameObject target)
    {
        if (moved == false)
            status.ChangeCooldown(-status.cooldownRemain * 0.8f);
        else
            moved = false;

        return base.Attack(target);
    }

    public override string GetSecondAbilityDescription()
    {
        return "If it does not move, reduce the cooldown by 80%.";
    }
}

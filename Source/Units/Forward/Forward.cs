/*!*******************************************************************
\file         Forward.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/12/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Linq;
using UnityEngine;

/*!*******************************************************************
\class        Forward
\brief		  Attack range is 4.
              If forwards can move after they attack, they try to recede from target.
********************************************************************/
public class Forward : UnitBase
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        status.attackRange = 3;
    }

    public override void TakeTurn()
    {
        eachTileScores.Clear();

        // Find best enemy & ally.
        ComputeEnemyScore();
        ComputeAllyScore();
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

            // If it succeeds or ambushed, finishes the turn.
            if (MoveAwayFrom(targetEnemy) != 0)
                return;

            attacked = true;
        }
        else if (distance > status.attackRange + 1)
        {
            if (nearEnemy != null)
            {
                Attack(nearEnemy);

                // If it succeeds or ambushed, finishes the turn.
                if (MoveAwayFrom(targetEnemy) != 0)
                    return;

                attacked = true;
            }
        }

        Vector3Int nextPos = eachTileScores.Aggregate((lhs, rhs) => lhs.Value > rhs.Value ? lhs : rhs).Key;

        if (attacked == false && nearEnemy == null) // If threr is no enemy in range, force to move.
        {
            if (MoveTo(Pathfinder.instance.GetNextPos(
                        TileManager.instance.cells[status.cellPos],
                        TileManager.instance.cells[targetEnemy.GetComponent<Status>().cellPos]
                        )
                    ) < 0)
                return; // Ambushed.
        }
        else
        {
            // Move to best position.
            if (MoveTo(nextPos) < 0)
                return; // Ambushed.
        }

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

    protected GameObject FindEnemyInRange()
    {
        if (status.enemy == true)
            EnemyList = UnitManager.instance.playerUnits;
        else
            EnemyList = UnitManager.instance.enemyUnits;

        GameObject target = null;
        double enemyScore = -1.0;

        foreach (GameObject enemy in EnemyList)
        {
            Status enemyStatus = enemy.GetComponent<Status>();
            Cell enemyCell = tileManager.cells[enemyStatus.cellPos];
            
            if (CanAttack(enemy) == false)
                continue;

            // Get unique danger value of enemy
            double score = enemy.GetComponent<UnitBase>().GetDangerValue(gameObject) + status.aggression;

            // Increases score depending on enemy's health. It never decreases the score.
            score = Mathematics.GoguensTconorm(score, 1.0 - (double)enemyStatus.health / (double)enemyStatus.maxHealth);

            // Store best enemy
            if (score > enemyScore)
            {
                target = enemy;
                enemyScore = score;
            }
        }

        return target;
    }

    protected int MoveAwayFrom(GameObject target)
    {
        Vector3Int backward = status.cellPos;
        Vector3Int enemyPos = target.GetComponent<Status>().cellPos;

        int row = backward.y & 1;

        // If they are at the same row
        if (enemyPos.y == status.cellPos.y)
        {
            // Try move horizontally
            if (enemyPos.x > status.cellPos.x)
                backward.x -= 1;
            else
                backward.x += 1;

            // TODO: try diagonal
            return MoveTo(backward);
        }
        else
        {
            // Try to move different row
            if (enemyPos.y > status.cellPos.y)
                backward.y -= 1;
            else if (enemyPos.y < status.cellPos.y)
                backward.y += 1;

            int movingResult = MoveTo(backward);
            if (movingResult != 0)
                return movingResult;

            // Try the other cell
            if (row == 0)
                backward.x -= 1;
            else
                backward.x += 1;

            return MoveTo(backward);
        }
    }

    public override double GetDangerValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.9;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.4;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.3;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.9;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.9;

        return 0.9;
    }

    public override double GetAllyValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.1;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.7;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.9;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.1;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.1;

        return 0.1;
    }

    public override string GetBaseAbilityDescription()
    {
        return "Attack range is " + status.attackRange + ".";
    }
}

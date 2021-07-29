/*!*******************************************************************
\file         Teleporter.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/01/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System;
using UnityEngine;

/*!*******************************************************************
\class        Teleporter
\brief		  Can teleport anywhere.
********************************************************************/
public class Teleporter : Melee
{
    public SpriteRenderer circle;

    public override void Initialize()
    {
        base.Initialize();

        circle.color = gameObject.GetComponent<SpriteRenderer>().color;

        status.useMovement = false;
    }

    public override void TakeTurn()
    {
        // Find best enemy & ally.
        ComputeEnemyScore();
        ComputeAllyScore();

        Vector3Int enemyPos = targetEnemy.GetComponent<Status>().cellPos;
        Vector3Int allyPos = targetAlly.GetComponent<Status>().cellPos;

        Vector3Int enemySafePosition = GetSafePosition(enemyPos);
        Vector3Int allySafePosition = GetSafePosition(allyPos);

        // If best enemy is nearby,
        if (CanAttack(targetEnemy))
        {
            // attack and then,
            Attack(targetEnemy);

            // try to move near ally.
            if (allySafePosition != allyPos)
                MoveTo(allySafePosition);

            return;
        }

        // Enemy was far,
        if (enemySafePosition != enemyPos)
        {
            // so move to near enemy and then,
            if (MoveTo(enemySafePosition) < 0)
                return;

            // attack.
            Attack(targetEnemy);
            return;
        }

        // Could not move to near enemy, so try to move near ally.
        if (allySafePosition != allyPos)
            MoveTo(allySafePosition);

        // Find enemey nearby
        GameObject target = FindEnemyOnAdjacentTiles();

        // If there is, attack.
        if (CanAttack(target))
            Attack(target);
    }

    protected override double ComputeEnemyScore()
    {
        if (status.enemy == true)
            EnemyList = UnitManager.instance.playerUnits;
        else
            EnemyList = UnitManager.instance.enemyUnits;

        Cell currentCell = tileManager.cells[status.cellPos];
        double healthScore = (double)status.health / (double)status.maxHealth;

        GameObject target = null;
        double enemyScore = -1.0;

        foreach (GameObject enemy in EnemyList)
        {
            // Get unique danger value of enemy
            double score = enemy.GetComponent<UnitBase>().GetDangerValue(gameObject) + status.aggression;

            Status enemyStatus = enemy.GetComponent<Status>();

            // Enemy cannot be detected.
            if (enemyStatus.stealth == true)
                continue;

            Cell enemyCell = tileManager.cells[enemyStatus.cellPos];

            // If enemy has negative protection, increases score.
            if (enemyStatus.protection < -1)
                score = Mathematics.GoguensTconorm(score, 1.0 + (1.0 / (double)enemyStatus.protection));
            else if (enemyStatus.protection > 1) // If enemy has prositive protection, decreases score.
                score *= (1.0 / Math.Pow(enemyStatus.protection, 0.1));

            // Increases score depending on enemy's health. It never decreases the score.
            score = Mathematics.GoguensTconorm(score, 1.0 - (double)enemyStatus.health / (double)enemyStatus.maxHealth);

            // Store best enemy
            if (score > enemyScore)
            {
                target = enemy;
                enemyScore = score;
            }
        }

        // Store target
        targetEnemy = target;
        return enemyScore;
    }

    protected override double ComputeAllyScore()
    {
        if (status.enemy == true)
        {
            AllyList = UnitManager.instance.enemyUnits;
            EnemyList = UnitManager.instance.playerUnits;
        }
        else
        {
            AllyList = UnitManager.instance.playerUnits;
            EnemyList = UnitManager.instance.enemyUnits;
        }

        if (EnemyList.Count == 1)
            return 0.0;

        Cell currentCell = tileManager.cells[status.cellPos];
        double healthScore = (double)status.health / (double)status.maxHealth;

        GameObject target = null;
        double allyScore = -1.0;

        double allyCountValue = (double)(AllyList.Count - 1) / (double)(AllyList.Count + EnemyList.Count);

        foreach (GameObject ally in AllyList)
        {
            if (ally == gameObject)
                continue;

            // Get unique ally value of enemy
            double score = ally.GetComponent<UnitBase>().GetAllyValue(gameObject) - status.aggression;
            score *= allyCountValue; // Decreases score by total number of allies.

            // If ally is healer, increases score depending on lost health.
            if (ally.GetComponent<Healer>() && ally.GetComponent<Priest>() == null)
                score = Mathematics.GoguensTconorm(score, 1.0 - healthScore);

            Status allyStatus = ally.GetComponent<Status>();
            Cell allyCell = tileManager.cells[allyStatus.cellPos];

            // Store best ally
            if (score > allyScore)
            {
                target = ally;
                allyScore = score;
            }
        }

        targetAlly = target;
        return allyScore;
    }

    private Vector3Int GetSafePosition(Vector3Int position)
    {
        int minimum = 7;
        Vector3Int safePos = position;

        int row = position.y & 1;
        Cell cell;

        for (int i = 0; i < 6; ++i)
        {
            Vector3Int cellPos = new Vector3Int(adjacentX[row, i], adjacentY[i], 0) + position;

            if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
                continue;

            // Cannot move to here.
            if (cell.owner != null)
            {
                Status enemyStatus = cell.owner.GetComponent<Status>();

                // There is an ally.
                if (enemyStatus.enemy == status.enemy)
                    continue;

                // There is an enemy can see.
                if (enemyStatus.stealth == false)
                    continue;
            }

            int num = NumberofAdjacentEnemies(cellPos);

            if (num < minimum)
            {
                minimum = num;
                safePos = cellPos;
            }
        }

        return safePos;
    }

    public override string GetSecondAbilityDescription()
    {
        return "Can move to any tile regardless of distance.";
    }
}

/*!*******************************************************************
\file         Wizard.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/31/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System;
using UnityEngine;

/*!*******************************************************************
\class        Wizard
\brief		  Give 50% damage to adjcent enemies of target.
********************************************************************/
public class Wizard : Forward
{
    public SpriteRenderer square;

    protected float splashDamageModifier = 0.5f;
    protected float damageModifier = 1.0f;

    public override void Initialize()
    {
        base.Initialize();

        square.color = gameObject.GetComponent<SpriteRenderer>().color;
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

            int distance = Pathfinder.instance.FindPath(currentCell, enemyCell, status.attackRange, status.enemy);

            // If enemy is far away, lower the score
            if (distance < 0 || distance > tileManager.maxLength + status.movement - status.attackRange)
                score = 0.0; // No path OR too far
            else
                score *= Math.Sqrt(1.0 - ((double)distance / (double)(tileManager.maxLength + status.movement - status.attackRange)));

            // If enemy has negative protection, increases score.
            if (enemyStatus.protection < -1)
                score = Mathematics.GoguensTconorm(score, 1.0 + (1.0 / (double)enemyStatus.protection));
            else if (enemyStatus.protection > 1) // If enemy has prositive protection, decreases score.
                score *= (1.0 / Math.Pow(enemyStatus.protection, 0.1));

            // Increases score depending on enemy's health. It never decreases the score.
            score = Mathematics.GoguensTconorm(score, 1.0 - (double)enemyStatus.health / (double)enemyStatus.maxHealth);

            // Increases score by number of adjacent enemies.
            score = Mathematics.GoguensTconorm(score, (double)NumberofAdjacentEnemies(enemyStatus.cellPos) / 6.0);

            // Store best enemy
            if (score > enemyScore)
            {
                target = enemy;
                enemyScore = score;
            }

            // Store next position value
            Vector3Int index = Pathfinder.instance.GetNextPos(currentCell, enemyCell);
            if (eachTileScores.ContainsKey(index))
                eachTileScores[index] = Mathematics.GoguensTconorm(eachTileScores[index], score);
            else
                eachTileScores.Add(index, score);
        }

        // Store target
        targetEnemy = target;
        return enemyScore;
    }

    public override bool ApplyDamage(GameObject target)
    {
        if (target == null)
            return false;

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

            // Give 50% damage to near enemies.
            cell.owner.GetComponent<UnitBase>().TakeDamage(status.power * splashDamageModifier, gameObject);
        }

        // Give 100% damage to original target.
        return target.GetComponent<UnitBase>().TakeDamage(status.power * damageModifier, gameObject);
    }

    public override string GetSecondAbilityDescription()
    {
        return "When attack, it gives 50% damage to enemies near the target enemy.";
    }
}

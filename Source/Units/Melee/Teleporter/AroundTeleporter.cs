/*!*******************************************************************
\file         AroundTeleporter.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        AroundTeleporter
\brief		  Attack all adjacent enemies.
********************************************************************/
public class AroundTeleporter : Teleporter
{
    public SpriteRenderer triangle;

    private Vector3Int nextPos;

    public override void Initialize()
    {
        base.Initialize();

        triangle.color = gameObject.GetComponent<SpriteRenderer>().color;

        status.useAggression = false;
    }

    public override void TakeTurn()
    {
        ComputeEnemyScore();

        // If staying is not the best, move.
        if (MoveTo(nextPos) < 0)
            return; // Ambushed.

        // Attack
        AttackAround();
    }

    protected override double ComputeEnemyScore()
    {
        double bestScore = 0.0;

        foreach (Cell cell in tileManager.cells.Values)
        {
            // Check whether there is an ambushed enemy or not.
            if (cell.owner)
            {
                Status enemyStatus = cell.owner.GetComponent<Status>();

                if (enemyStatus.enemy == status.enemy)
                    continue; // There is an ally, so cannot move to here.
                else if (enemyStatus.stealth == false)
                    continue; // There is an enemy not ambushed, so cannot move to here.
            }

            // Check whether this cell is blocked or not.
            if (status.enemy == true)
            {
                if (cell.blockEnemyUnits > 0)
                    continue;
            }
            else if (cell.blockPlayerUnits > 0)
                continue;

            double score = GetDangerValueSum(cell.cellPos);
            
            if (score > bestScore)
            {
                bestScore = score;
                nextPos = cell.cellPos;
            }
        }

        // no meaning
        return 1.0;
    }

    private double GetDangerValueSum(Vector3Int position)
    {
        double result = 0.0;

        int row = position.y & 1;
        Cell cell;

        for (int i = 0; i < 6; ++i)
        {
            Vector3Int cellPos = new Vector3Int(adjacentX[row, i], adjacentY[i], 0) + position;

            if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
                continue;

            if (cell.owner == null)
                continue;

            if (cell.owner.GetComponent<Status>().enemy != status.enemy)
                result += cell.owner.GetComponent<UnitBase>().GetDangerValue(gameObject);
        }

        return result;
    }

    private void AttackAround()
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

            if (cell.owner.GetComponent<Status>().enemy != status.enemy)
                Attack(cell.owner);
        }
    }

    public override string GetThirdAbilityDescription()
    {
        return "Attack all adjacent enemies. It always tries to move to where it can attack enemies as much as possible.";
    }
}

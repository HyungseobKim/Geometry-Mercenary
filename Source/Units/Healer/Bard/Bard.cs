/*!*******************************************************************
\file         Bard.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/30/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Linq;
using System;
using UnityEngine;

/*!*******************************************************************
\class        Bard
\brief		  Heal all adjacent allies.
********************************************************************/
public class Bard : Healer
{
    public SpriteRenderer pentagon;

    public override void Initialize()
    {
        base.Initialize();

        pentagon.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override void TakeTurn()
    {
        eachTileScores.Clear();
        ComputeAllyScore();
        ComputeThreatenScore();

        // Get position to move.
        Vector3Int nextPos = eachTileScores.Aggregate((lhs, rhs) => lhs.Value > rhs.Value ? lhs : rhs).Key;

        // If staying is not the best, move.
        if (MoveTo(nextPos) < 0)
            return; // Ambushed.

        // Use ability.
        HealAllies();
    }

    protected override double ComputeAllyScore()
    {
        eachTileScores.Add(status.cellPos, NumberofAdjacentAllies(status.cellPos) / 6.0);

        int row = status.cellPos.y & 1;
        Cell cell;

        for (int i = 0; i < 6; ++i)
        {
            Vector3Int cellPos = new Vector3Int(adjacentX[row, i], adjacentY[i], 0) + status.cellPos;

            if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
                continue;

            if (cell.owner != null)
            {
                Status enemyStatus = cell.owner.GetComponent<Status>();

                if (enemyStatus.enemy == status.enemy)
                    continue; // There is an ally, so cannot move to here.
                else if (enemyStatus.stealth == false)
                    continue; // There is an enemy not ambushed, so cannot move to here.
            }

            eachTileScores.Add(cellPos, NumberofAdjacentAllies(cellPos) / 6.0);
        }

        // no meaning
        return 1.0;
    }

    protected virtual void HealAllies()
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
                HealAlly(cell.owner);
        }
    }

    public override string GetSecondAbilityDescription()
    {
        return "Heals all near allies.";
    }
}

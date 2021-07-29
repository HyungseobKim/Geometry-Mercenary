/*!*******************************************************************
\file         Enganche.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/31/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Linq;
using UnityEngine;

/*!*******************************************************************
\class        Melee
\brief		  Support all near allies.
********************************************************************/
public class Enganche : Supporter
{
    public SpriteRenderer pentagon;

    public override void Initialize()
    {
        base.Initialize();

        pentagon.color = gameObject.GetComponent<SpriteRenderer>().color;

        status.useMovement = false;
        status.abilityRange = TileManager.instance.maxLength;
    }

    /*!
     * \brief Move to best position and use ability.
     */
    public override void TakeTurn()
    {
        eachTileScores.Clear();
        ComputeAllyScore();
        ComputeThreatenScore();

        // Grab position to move.
        Vector3Int nextPos = eachTileScores.Aggregate((lhs, rhs) => lhs.Value > rhs.Value ? lhs : rhs).Key;

        // If staying is not the best, move.
        if (MoveTo(nextPos) < 0)
            return; // Ambushed.

        // If use ability.
        SupportAllies();
    }

    /*!
     * \brief Checks only the number of adjacent allies.
     */
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

            // Cannot move to here.
            if (cell.owner != null)
                continue;

            eachTileScores.Add(cellPos, NumberofAdjacentAllies(cellPos) / 6.0);
        }

        // no meaning
        return 1.0;
    }

    /*!
     * \brief Support all allies.
     */
    protected virtual void SupportAllies()
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
                SupportAlly(cell.owner);
        }
    }

    public override string GetSecondAbilityDescription()
    {
        return "Support all near allies.";
    }
}

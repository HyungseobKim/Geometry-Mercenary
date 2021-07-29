/*!*******************************************************************
\file         Stopper.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Stopper
\brief		  Enemies cannot move to tiles adjacent to Stoppers.
********************************************************************/
public class Stopper : Defender
{
    public SpriteRenderer circle;
    public SpriteRenderer hexagon;

    public override void Initialize()
    {
        base.Initialize();

        circle.color = gameObject.GetComponent<SpriteRenderer>().color;
        hexagon.color = new Color(circle.color.r, circle.color.g, circle.color.b, 0.5f);

        BlockAdjacentTiles();
    }

    void OnDestroy()
    {
        ClearAdjacentTiles();
    }

    public override int MoveTo(Vector3Int cellPos)
    {
        ClearAdjacentTiles();
        int moved = base.MoveTo(cellPos);
        BlockAdjacentTiles();

        return moved;
    }

    public override void ForceToMoveTo(Vector3Int cellPos)
    {
        ClearAdjacentTiles();
        base.ForceToMoveTo(cellPos);
        BlockAdjacentTiles();
    }

    private void ClearAdjacentTiles()
    {
        int row = status.cellPos.y & 1;
        Cell cell;

        for (int i = 0; i < 6; ++i)
        {
            Vector3Int cellPos = new Vector3Int(adjacentX[row, i], adjacentY[i], 0) + status.cellPos;

            if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
                continue;

            if (status.enemy)
                cell.blockPlayerUnits -= 1;
            else
                cell.blockEnemyUnits -= 1;
        }
    }

    private void BlockAdjacentTiles()
    {
        int row = status.cellPos.y & 1;
        Cell cell;

        for (int i = 0; i < 6; ++i)
        {
            Vector3Int cellPos = new Vector3Int(adjacentX[row, i], adjacentY[i], 0) + status.cellPos;

            if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
                continue;

            if (status.enemy)
                cell.blockPlayerUnits += 1;
            else
                cell.blockEnemyUnits += 1;
        }
    }

    public override double GetDangerValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.1;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.2;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.1;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.1;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.5;

        return 0.1;
    }

    public override double GetAllyValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.2;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.9;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.2;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.1;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.5;

        return 0.4;
    }

    public override string GetSecondAbilityDescription()
    {
        return "Block around tiles. Enemies cannot move into the blocked area.";
    }
}

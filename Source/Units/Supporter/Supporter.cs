/*!*******************************************************************
\file         Supporter.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/23/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Linq;
using System;
using UnityEngine;

/*!*******************************************************************
\class        Supporter
\brief		  Makes near ally having turn immediately.
********************************************************************/
public class Supporter : UnitBase
{
    public int extraTurn = 1;

    public override void TakeTurn()
    {
        eachTileScores.Clear();

        // Find best enemy & ally.
        ComputeEnemyScore();
        ComputeAllyScore();
        ComputeThreatenScore();

        bool act = false;

        // Try to use ability to best scored ally.
        if (CanUseAbility(targetAlly, true))
        {
            SupportAlly(targetAlly);
            act = true;
        }

        Vector3Int nextPos = eachTileScores.Aggregate((lhs, rhs) => lhs.Value > rhs.Value ? lhs : rhs).Key;

        // If staying is not the best, move.
        if (MoveTo(nextPos) < 0)
            return; // Ambushed.

        if (act == true)
            return;

        // Try to use ability to best scored ally again.
        if (CanUseAbility(targetAlly, true))
        {
            SupportAlly(targetAlly);
            return;
        }

        // Find any ally and try to use ability.
        GameObject target = FindAllyOnAdjacentTiles();

        if (CanUseAbility(target, true))
        {
            SupportAlly(targetAlly);
            return;
        }

        // Find enemey nearby
        target = FindEnemyOnAdjacentTiles();

        // If there is, attack.
        if (CanAttack(target))
            Attack(target);
    }

    /*!
     * \brief Using power of ally when evaluate.
     */
    protected override double ComputeAllyScore()
    {
        if (status.enemy == true)
            AllyList = UnitManager.instance.enemyUnits;
        else
            AllyList = UnitManager.instance.playerUnits;

        Cell currentCell = tileManager.cells[status.cellPos];
        float power = status.power;

        GameObject target = null;
        double allyScore = -1.0;

        foreach (GameObject ally in AllyList)
        {
            if (ally == gameObject || ally.GetComponent<Goal>())
                continue;

            // Get unique ally value of enemy
            double score = ally.GetComponent<UnitBase>().GetAllyValue(gameObject) - status.aggression;

            Status allyStatus = ally.GetComponent<Status>();

            // Increases ally score depends on difference of power.
            score = Mathematics.GoguensTconorm(score, (double)Mathf.Clamp(allyStatus.power - power, 0.0f, allyStatus.power) / allyStatus.power);

            Cell allyCell = tileManager.cells[allyStatus.cellPos];

            // If ally is far away, lower the score
            int distance = Pathfinder.instance.FindPath(currentCell, allyCell, 1, status.enemy);
            if (distance < 0 || distance > tileManager.maxLength + status.movement)
                score = 0.0; // No path OR too far
            else
                score *= Math.Sqrt(1.0 - distance / (tileManager.maxLength + status.movement));

            // Store best ally
            if (score > allyScore)
            {
                target = ally;
                allyScore = score;
            }

            // Store next position value
            Vector3Int index = Pathfinder.instance.GetNextPos(currentCell, allyCell);
            if (eachTileScores.ContainsKey(index))
                eachTileScores[index] = Mathematics.GoguensTconorm(eachTileScores[index], score);
            else
                eachTileScores.Add(index, score);
        }

        targetAlly = target;
        return allyScore;
    }

    protected override GameObject FindAllyOnAdjacentTiles()
    {
        int row = status.cellPos.y & 1;

        for (int i = 0; i < 6; ++i)
        {
            Vector3Int cellPos = new Vector3Int(adjacentX[row, i], adjacentY[i], 0) + status.cellPos;
            Cell cell;

            if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
                continue;

            if (cell.owner == null)
                continue;

            if (cell.owner.GetComponent<Status>().enemy != status.enemy)
                continue;

            if (cell.owner.GetComponent<Goal>())
                continue;

            return cell.owner;
        }

        return null;
    }

    protected override bool CanUseAbility(GameObject target, bool toAlly)
    {
        if (base.CanUseAbility(target, toAlly) == false)
            return false;
        
        return (target.GetComponent<Goal>() == null);
    }

    public virtual void GiveExtraTurn(GameObject target)
    {
        target.GetComponent<UnitBase>().GetSupport(extraTurn);
    }

    protected virtual GameObject SupportAlly(GameObject target)
    {
        if (target == null)
            return null;

        if (TileManager.instance.DistanceBetween(target.GetComponent<Status>().cellPos, status.cellPos) > status.abilityRange)
            return null;

        GameObject projectile = UnityEngine.Object.Instantiate(abilityProjectilePrefab, gameObject.transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Initialize(target, gameObject);

        return projectile;
    }

    public override double GetDangerValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.7;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.3;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.2;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.8;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.7;

        return 0.7;
    }

    public override double GetAllyValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.4;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.5;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.3;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.5;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.6;

        return 0.5;
    }

    public override string GetBaseAbilityDescription()
    {
        return "Can make one near ally taking turn immediately.";
    }
}

/*!*******************************************************************
\file         Healer.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/23/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Linq;
using System;
using UnityEngine;

/*!*******************************************************************
\class        Healer
\brief		  Can heal one near ally.
********************************************************************/
public class Healer : UnitBase
{
    public float healingAmount = 1.0f;

    public override void TakeTurn()
    {
        eachTileScores.Clear();

        // Find best enemy & ally.
        ComputeEnemyScore();
        ComputeAllyScore();
        ComputeThreatenScore();

        bool act = false;

        // Try to heal best ally first.
        if (CanUseAbility(targetAlly, true))
        {
            HealAlly(targetAlly);
            act = true;
        }
        else if (CanAttack(targetEnemy))
        {
            Attack(targetEnemy);
            act = true;
        }

        Vector3Int nextPos = eachTileScores.Aggregate((lhs, rhs) => lhs.Value > rhs.Value ? lhs : rhs).Key;

        // If staying is not the best, move.
        if (MoveTo(nextPos) < 0)
            return; // Ambushed.

        // Already act and moved, so finishes the turn.
        if (act == true)
            return;

        // Try to heal best ally first.
        if (CanUseAbility(targetAlly, true))
        {
            HealAlly(targetAlly);
            return;
        }

        // Find any ally nearby.
        GameObject target = FindAllyOnAdjacentTiles();

        // If there is, heal them and finishes turn.
        if (target)
        {
            HealAlly(target);
            return;
        }

        // Try to attack best enemy.
        if (CanAttack(targetEnemy))
        {
            Attack(targetEnemy);
            return;
        }

        // Find enemey nearby
        target = FindEnemyOnAdjacentTiles();

        // If there is, attack.
        if (target != null)
            ApplyDamage(target);
    }

    protected override double ComputeAllyScore()
    {
        // Grab list of allies.
        if (status.enemy == true)
            AllyList = UnitManager.instance.enemyUnits;
        else
            AllyList = UnitManager.instance.playerUnits;

        Cell currentCell = tileManager.cells[status.cellPos];

        GameObject target = null;
        double allyScore = -1.0;

        foreach (GameObject ally in AllyList)
        {
            if (ally == gameObject)
                continue;

            // Get unique ally value of enemy
            double score = ally.GetComponent<UnitBase>().GetAllyValue(gameObject) - status.aggression;

            Status allyStatus = ally.GetComponent<Status>();

            // Increases ally score depends on lost health of ally
            score = Mathematics.GoguensTconorm(score, 1.0 - (double)allyStatus.health / (double)allyStatus.maxHealth);
            
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

    public override void ApplyHeal(GameObject target)
    {
        if (target == null)
            return;

        target.GetComponent<UnitBase>().GetHeal(status.power * healingAmount);
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
            return 0.6;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.4;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.8;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.4;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.5;

        return 0.5;
    }

    public override string GetBaseAbilityDescription()
    {
        return "Can heal one near ally instead of attack.\nThe amount is twice of power.";
    }
}

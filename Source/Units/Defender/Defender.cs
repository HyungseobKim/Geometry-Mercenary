/*!*******************************************************************
\file         Defender.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/10/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Linq;
using UnityEngine;

/*!*******************************************************************
\class        Defender
\brief		  Defenders take less damage if they do not move.
********************************************************************/
public class Defender : UnitBase
{
    // Defender ability
    private bool moved = false;

    /*!
     * \brief Check whether defenders move or no.
     */
    public override void TakeTurn()
    {
        eachTileScores.Clear();
        eachTileScores.Add(status.cellPos, 0.2); // Advantage from ability.

        // Find best enemy & ally.
        ComputeEnemyScore();
        ComputeAllyScore();

        bool attacked = false;

        // If best enemy is near, attack.
        if (CanAttack(targetEnemy))
        {
            Attack(targetEnemy);
            attacked = true;
        }

        moved = false;
        Vector3Int nextPos = eachTileScores.Aggregate((lhs, rhs) => lhs.Value > rhs.Value ? lhs : rhs).Key;

        // If staying is not the best, move.
        if (nextPos != status.cellPos)
        {
            int movingResult = MoveTo(nextPos);

            if (movingResult < 0)
            {
                moved = true;
                return;
            }

            if (movingResult > 0)
                moved = true;
        }

        // Already attacked, so finish the turn.
        if (attacked == true)
            return;

        // If best enemy is near, attack and finish the turn.
        if (CanAttack(targetEnemy))
        {
            Attack(targetEnemy);
            return;
        }

        // Find enemey nearby
        GameObject target = FindEnemyOnAdjacentTiles();

        // If there is, attack.
        if (CanAttack(target))
            Attack(target);
    }

    /*!
     * \brief If they didn't move in this turn, take 50% less damage.
     */
    public override bool TakeDamage(float amount, GameObject enemy, bool critical = false)
    {
        if (moved == false)
            amount /= 2.0f;

        return base.TakeDamage(amount, enemy, critical);
    }

    /*!
     * \brief Defenders are not dangerous enemy because of their ability.
     */
    public override double GetDangerValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.4;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.2;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.1;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.3;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.4;

        return 0.4;
    }

    /*!
     * \brief Healers will likely to help defenders.
     */
    public override double GetAllyValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.3;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.8;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.3;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.2;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.4;

        return 0.4;
    }

    public override string GetBaseAbilityDescription()
    {
        return "If not move, gain 50% damage reduction until next turn.";
    }
}

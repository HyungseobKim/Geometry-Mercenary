/*!*******************************************************************
\file         Playmaker.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/31/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Linq;
using UnityEngine;

/*!*******************************************************************
\class        Playmaker
\brief		  Can support any ally regardless of distance.
********************************************************************/
public class Playmaker : Supporter
{
    public SpriteRenderer triangle;

    public override void Initialize()
    {
        base.Initialize();

        triangle.color = gameObject.GetComponent<SpriteRenderer>().color;

        status.useMovement = false;
    }

    /*!
     * \brief Support target ally always.
     *        Not attack, and only care avoiding enemies or getting close to allies.
     */
    public override void TakeTurn()
    {
        eachTileScores.Clear();
        ComputeAllyScore();
        ComputeThreatenScore();

        // No distance limitation.
        SupportAlly(targetAlly);

        Vector3Int nextPos = eachTileScores.Aggregate((lhs, rhs) => lhs.Value > rhs.Value ? lhs : rhs).Key;

        // If staying is not the best, move.
        MoveTo(nextPos);
    }

    /*!
     * \brief Use power of ally when evaluates.
     */
    protected override double ComputeAllyScore()
    {
        if (status.enemy == true)
            AllyList = UnitManager.instance.enemyUnits;
        else
            AllyList = UnitManager.instance.playerUnits;

        Cell currentCell = tileManager.cells[status.cellPos];

        GameObject target = null;
        double allyScore = -1.0;

        foreach (GameObject ally in AllyList)
        {
            if (ally == gameObject || ally.GetComponent<Goal>())
                continue;

            // Get unique ally value of enemy
            double score = ally.GetComponent<UnitBase>().GetAllyValue(gameObject) - status.aggression;

            Status allyStatus = ally.GetComponent<Status>();

            // Increases ally score depends on power of ally
            score = Mathematics.GoguensTconorm(score, allyStatus.power / 100.0);

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

    public override double GetAllyValue(GameObject scorer)
    {
        if (scorer.GetComponent<Defender>() != null)
            return 0.4;
        else if (scorer.GetComponent<Healer>() != null)
            return 0.4;
        else if (scorer.GetComponent<Supporter>() != null)
            return 0.7;
        else if (scorer.GetComponent<Melee>() != null)
            return 0.1;
        else if (scorer.GetComponent<Forward>() != null)
            return 0.2;

        return 0.3;
    }

    public override string GetSecondAbilityDescription()
    {
        return "Can support any ally regardless of distance.";
    }
}

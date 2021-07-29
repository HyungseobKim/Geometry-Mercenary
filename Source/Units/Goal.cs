/*!*******************************************************************
\file         Goal.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/10/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System;
using UnityEngine;

/*!*******************************************************************
\class        Goal
\brief		  Goal unit which is objective of each battle.
********************************************************************/
public class Goal : UnitBase
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        status.speed = 0.0f; // Make sure it cannot take turn
        status.cooldownRemain = 1.0f;
    }

    public override void TakeTurn()
    {
        // Do nothing.
    }

    public override int MoveTo(Vector3Int cellPos)
    {
        // Can't move.
        return 0;
    }

    public override bool Attack(GameObject target)
    {
        // Can't attack.
        return false;
    }

    /*!
     * \brief The more allies of the goal, the lower danger value.
     * 
     * \param double
     *        Score of this unit.
     */
    public override double GetDangerValue(GameObject scorer)
    {
        int allyCount;
        int enemyCount;

        if (status.enemy == true)
        {
            allyCount = UnitManager.instance.enemyUnits.Count;
            enemyCount = UnitManager.instance.playerUnits.Count;
        }
        else
        {
            allyCount = UnitManager.instance.playerUnits.Count;
            enemyCount = UnitManager.instance.enemyUnits.Count;
        }

        double score = Math.Pow(1.0 - (status.health / (status.maxHealth * 2.0)), 2.0);
        score = Mathematics.GoguensTconorm(score, (double)Mathf.Clamp(enemyCount - allyCount, 0, 11) / 11.0);

        return score;
    }

    /*!
     * \brief The more allies of the goal, the lower ally value.
     * 
     * \param double
     *        Score of this unit.
     */
    public override double GetAllyValue(GameObject scorer)
    {
        int allyCount;
        int enemyCount;

        if (status.enemy == true)
        {
            allyCount = UnitManager.instance.enemyUnits.Count;
            enemyCount = UnitManager.instance.playerUnits.Count;
        }
        else
        {
            allyCount = UnitManager.instance.playerUnits.Count;
            enemyCount = UnitManager.instance.enemyUnits.Count;
        }

        double score = Math.Pow((1.0 - (status.health / status.maxHealth)), 2.0);
        score = Mathematics.GoguensTconorm(score, (double)Mathf.Clamp(enemyCount - allyCount, 0, 11) / 11.0);

        return score;
    }
}

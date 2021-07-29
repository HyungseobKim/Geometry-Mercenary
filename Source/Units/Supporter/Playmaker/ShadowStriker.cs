/*!*******************************************************************
\file         ShadowStriker.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Melee
\brief		  Execute all enemies that have lower health than its power.
********************************************************************/
public class ShadowStriker : Playmaker
{
    public SpriteRenderer pentagon;

    public override void Initialize()
    {
        base.Initialize();

        pentagon.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override void TakeTurn()
    {
        base.TakeTurn();

        // Grab list of enemies.
        if (status.enemy)
            EnemyList = UnitManager.instance.playerUnits;
        else
            EnemyList = UnitManager.instance.enemyUnits;

        // Look all enemies.
        for(int i = EnemyList.Count - 1; i >= 0; --i)
        {
            GameObject enemy = EnemyList[i];
            Status enemyStatus = enemy.GetComponent<Status>();

            // Enemy has more health than power of this unit.
            if (enemyStatus.health > status.power)
                continue;

            // Generate execute UI.
            BattleUIManager.instance.CreateExecuteUI(enemy);

            // Execute that enemy.
            enemyStatus.ChangeHealth(-enemyStatus.health);
            enemy.GetComponent<UnitBase>().IsDied();
        }
    }

    public override string GetThirdAbilityDescription()
    {
        return "Each time it takes a turn, execute all enemies that have lower health than its power.";
    }
}

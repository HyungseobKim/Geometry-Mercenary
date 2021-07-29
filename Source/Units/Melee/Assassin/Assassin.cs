/*!*******************************************************************
\file         Assassin.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/02/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Linq;
using UnityEngine;

/*!*******************************************************************
\class        Assassin
\brief		  If it does not attack, sneaks.
********************************************************************/
public class Assassin : Melee
{
    public SpriteRenderer cross;

    protected Color stealthColor;
    protected Color originalColor;
    
    private bool moved;

    public override void Initialize()
    {
        base.Initialize();

        originalColor = gameObject.GetComponent<SpriteRenderer>().color;
        
        stealthColor = originalColor;
        stealthColor.a = 0.5f;

        cross.color = originalColor;
    }

    public override void TakeTurn()
    {
        eachTileScores.Clear();
        double enemyScore = ComputeEnemyScore();
        double allyScore = ComputeAllyScore();

        bool attacked = false;

        if (enemyScore > allyScore)
        {
            // If best enemy is near, attack.
            if (CanAttack(targetEnemy))
            {
                Attack(targetEnemy);
                attacked = true;
            }
        }
        else
        {
            // Not attack this turn.
            attacked = true;
            Stealth();
        }

        Vector3Int nextPos = eachTileScores.Aggregate((lhs, rhs) => lhs.Value > rhs.Value ? lhs : rhs).Key;

        // Try to move.
        if (MoveTo(nextPos) < 0)
        {
            // There was an ambush.
            Destealth();
            return;
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
        else
        {
            // Find enemey nearby
            GameObject target = FindEnemyOnAdjacentTiles();

            // If there is, attack.
            if (target != null)
            {
                Attack(target);
                return;
            }
        }

        if (attacked == false)
            Stealth();
    }

    protected virtual void Stealth()
    {
        status.stealth = true;

        gameObject.GetComponent<SpriteRenderer>().color = stealthColor;
        cross.color = stealthColor;
    }

    public virtual void Destealth()
    {
        status.stealth = false;

        gameObject.GetComponent<SpriteRenderer>().color = originalColor;
        cross.color = originalColor;
    }

    public override bool TakeDamage(float amount, GameObject enemy, bool critical = false)
    {
        // Store original health
        float health = status.health;

        bool died = base.TakeDamage(amount, gameObject, critical);

        // If health decreased, remove stealth.
        if (status.health < health)
            Destealth();

        return died;
    }

    public override bool Attack(GameObject target)
    {
        bool result = base.Attack(target);
        
        Destealth();

        return result;
    }

    public override string GetSecondAbilityDescription()
    {
        return "If it does not attack, sneaks.";
    }
}

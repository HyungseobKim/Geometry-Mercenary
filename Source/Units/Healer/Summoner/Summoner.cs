/*!*******************************************************************
\file         Summoner.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/30/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Linq;
using UnityEngine;

/*!*******************************************************************
\class        Summoner
\brief		  Summon a creature when move. It does not attack or heal.
********************************************************************/
public class Summoner : Healer
{
    public SpriteRenderer circle;
    public GameObject summoningCreaturePrefab; //! Prefab of creature.

    public override void Initialize()
    {
        base.Initialize();

        circle.color = gameObject.GetComponent<SpriteRenderer>().color;

        status.useMovement = false;
        status.useAggression = false;
    }

    public override void TakeTurn()
    {
        eachTileScores.Clear();
        ComputeAllyScore();
        ComputeThreatenScore();

        Vector3Int position = status.cellPos;

        eachTileScores[position] = 0.0; // We want to move always.
        Vector3Int nextPos = eachTileScores.Aggregate((lhs, rhs) => lhs.Value > rhs.Value ? lhs : rhs).Key;

        if (nextPos != position)
        {
            // Try to move. If it succeeds, summon.
            if (MoveTo(nextPos) == 1)
                Summon(position);
        }
    }

    // Quaternion.Euler(Vector3.forward * 90) : way to rotate
    protected virtual void Summon(Vector3Int position)
    {
        GameObject summonedCreature = Object.Instantiate(summoningCreaturePrefab, tilemap.CellToWorld(position), Quaternion.identity);

        Status statusSummoned = summonedCreature.GetComponent<Status>();
        statusSummoned.enemy = status.enemy;

        statusSummoned.maxHealth = status.power;
        statusSummoned.health = status.power;

        statusSummoned.power = 5.0f;
        statusSummoned.speed = 50.0f;

        summonedCreature.GetComponent<UnitBase>().Initialize();
    }

    public override bool CanAttack(GameObject enemy)
    {
        // Can't attack.
        return false;
    }

    public override bool ApplyDamage(GameObject target)
    {
        // Can't attack.
        return false;
    }

    public override bool Attack(GameObject target)
    {
        // Can't attack.
        return false;
    }

    public override string GetSecondAbilityDescription()
    {
        return "Cannot attack.\nSummon creature when move.";
    }
}

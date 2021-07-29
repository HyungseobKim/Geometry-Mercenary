/*!*******************************************************************
\file         Commander.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/25/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Commander
\brief		  Commanders make all enemies move close to them.
********************************************************************/
public class Commander : Stopper
{
    public SpriteRenderer triangle;

    public override void Initialize()
    {
        base.Initialize();

        triangle.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override void TakeTurn()
    {
        base.TakeTurn();

        foreach (GameObject enemy in EnemyList)
        {
            PullUnit(enemy);
        }

        PullUnit(targetAlly);
    }

    private void PullUnit(GameObject target)
    {
        if (target == null)
            return;

        if (target.GetComponent<Goal>())
            return;

        Vector3 position = gameObject.transform.position;
        Vector3 targetPos = target.transform.position;
        Vector3 move = (position - targetPos) / (float)tileManager.DistanceBetween(tilemap.WorldToCell(position), tilemap.WorldToCell(targetPos));

        target.GetComponent<UnitBase>().MoveTo(tilemap.WorldToCell(targetPos + move));
    }

    public override string GetThirdAbilityDescription()
    {
        return "Make all enemies move ahead to this unit.";
    }
}

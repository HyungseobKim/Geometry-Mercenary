/*!*******************************************************************
\file         PenetrationProjectile.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/23/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!*******************************************************************
\class        PenetrationProjectile
\brief		  Give damage to all enemies that ecounters it.
********************************************************************/
public class PenetrationProjectile : Projectile
{
    private List<GameObject> encounteredEnemies = new List<GameObject>();

    protected override void Action()
    {
        Vector3Int cellPos = tilemap.WorldToCell(gameObject.transform.position);
        Cell cell;

        if (TileManager.instance.cells.TryGetValue(cellPos, out cell) == false)
            return; // Error.

        // Nobody here.
        if (cell.owner == null)
            return;

        // Already attacked this object.
        if (encounteredEnemies.Contains(cell.owner))
            return;

        // To prevent to attack target twice.
        if (cell.owner == target)
            return;

        encounteredEnemies.Add(cell.owner);
        owner.GetComponent<UnitBase>().ApplyDamage(cell.owner);
    }
}

/*!*******************************************************************
\file         HealingDealingProjectile.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/23/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!*******************************************************************
\class        HealingDealingProjectile
\brief		  Give damage to all enemies that it enoucnters, 
              and heals target ally instead of damaging.
********************************************************************/
public class HealingDealingProjectile : HealingProjectile
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

        encounteredEnemies.Add(cell.owner);
        owner.GetComponent<UnitBase>().ApplyDamage(cell.owner);
    }
}

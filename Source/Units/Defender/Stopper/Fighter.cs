/*!*******************************************************************
\file         Fighter.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/24/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!*******************************************************************
\class        Fighter
\brief		  Fighters push the enemy when they attack.
********************************************************************/
public class Fighter : Stopper
{
    public SpriteRenderer pentagon;

    private Dictionary<GameObject, Vector3Int> enemies = new Dictionary<GameObject, Vector3Int>();

    public override void Initialize()
    {
        base.Initialize();

        pentagon.color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public override bool ApplyDamage(GameObject target)
    {
        if (target == null)
            return false;

        if (enemies.ContainsKey(target))
            return ApplyDamageOnAmbush(target);
        
        return ApplyDamageOnProjectile(target);
    }

    private bool ApplyDamageOnProjectile(GameObject target)
    {
        if (base.ApplyDamage(target))
            return true; // Died.

        Vector3 newPos = target.transform.position * 2.0f - gameObject.transform.position;
        Vector3Int cellPos = tilemap.WorldToCell(newPos);

        Cell cell;

        // Collide with the boundary OR collide with other unit.
        if (tileManager.cells.TryGetValue(cellPos, out cell) == false || cell.owner)
        {
            enemies.Add(target, cellPos);
            target.GetComponent<UnitControl>().MoveAndMeetAmbush(newPos, gameObject);
            return false;
        }

        target.GetComponent<UnitBase>().MoveTo(cellPos);
        return false;
    }

    private bool ApplyDamageOnAmbush(GameObject target)
    {
        Cell cell;

        if (tileManager.cells.TryGetValue(enemies[target], out cell) == true)
        {
            if (cell.owner && cell.owner.GetComponent<Status>().enemy != status.enemy)
                base.ApplyDamage(cell.owner);
        }

        enemies.Remove(target);
        return base.ApplyDamage(target);
    }

    public override string GetThirdAbilityDescription()
    {
        return "When attack, push that unit. If that enemy collides with other units or boundaries, it gives additional damage.";
    }
}

/*!*******************************************************************
\file         Dazzle.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/26/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*!*******************************************************************
\class        Dazzle
\brief		  Prvent all adjacent allies from dying.
********************************************************************/
public class Dazzle : Bard
{
    public SpriteRenderer circle;

    public float aliveTime = 5.0f; //! Time to prevent dying.
    private List<GameObject> allies = new List<GameObject>(); //! List of allies affecting from ability.
    private List<float> timers = new List<float>(); //! Timers for each ally.

    public override void Initialize()
    {
        base.Initialize();

        circle.color = gameObject.GetComponent<SpriteRenderer>().color;

        AssignDazzle();
    }

    void OnDestroy()
    {
        ClearDazzle();

        CleanLists();
        for (int i = allies.Count - 1; i >= 0; --i)
            KillAlly(i);
    }

    /*!
     * \brief Ally got deadly attack.
     *        Add that ally to the list and apply ability.
     */
    public void ShallowGrave(GameObject ally)
    {
        if (ally == null)
            return;

        // Not an ally.
        if (ally.GetComponent<Status>().enemy != status.enemy)
            return;

        // Already registered.
        if (allies.Contains(ally))
            return;

        // Add to lists.
        allies.Add(ally);
        timers.Add(aliveTime);

        // Turn on effect.
        ally.GetComponent<UnitBase>().shallowGrave = true;
    }

    void Update()
    {
        CleanLists();

        for (int i = timers.Count - 1; i >= 0; --i)
        {
            // Update timer.
            timers[i] -= Time.deltaTime;

            if (timers[i] <= 0.0f)
                KillAlly(i);
        }
    }

    /*!
     * \brief Remove all null object from the lists.
     */
    private void CleanLists()
    {
        for (int i = allies.Count - 1; i >= 0; --i)
        {
            if (allies[i] == null)
            {
                allies.RemoveAt(i);
                timers.RemoveAt(i);
            }
        }
    }

    /*!
     * \brief Life time is over. Kill the ally.
     * 
     * \param index
     *        Index of the ally to kill.
     */
    private void KillAlly(int index)
    {
        GameObject ally = allies[index];

        // Reduce health to 0.
        Status allyStatus = ally.GetComponent<Status>();
        allyStatus.ChangeHealth(allyStatus.health);

        UnitBase allyBase = ally.GetComponent<UnitBase>();

        // Kill it.
        allyBase.shallowGrave = false;
        allyBase.dazzle = null;
        allyBase.IsDied();

        // Remove from lists.
        allies.RemoveAt(index);
        timers.RemoveAt(index);
    }

    /*!
     * \brief Tell all adjacent allies that there is dazzle near them.
     */
    private void AssignDazzle()
    {
        int row = status.cellPos.y & 1;
        Cell cell;

        for (int i = 0; i < 6; ++i)
        {
            Vector3Int cellPos = new Vector3Int(adjacentX[row, i], adjacentY[i], 0) + status.cellPos;

            if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
                continue;

            if (cell.owner == null || cell.owner.GetComponent<Status>().enemy != status.enemy)
                continue;

            cell.owner.GetComponent<UnitBase>().dazzle = this;
        }
    }

    private void ClearDazzle()
    {
        int row = status.cellPos.y & 1;
        Cell cell;

        for (int i = 0; i < 6; ++i)
        {
            Vector3Int cellPos = new Vector3Int(adjacentX[row, i], adjacentY[i], 0) + status.cellPos;

            if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
                continue;

            if (cell.owner == null || cell.owner.GetComponent<Status>().enemy != status.enemy)
                continue;

            cell.owner.GetComponent<UnitBase>().dazzle = null;
        }
    }

    public override int MoveTo(Vector3Int cellPos)
    {
        ClearDazzle();
        int moved = base.MoveTo(cellPos);
        AssignDazzle();

        return moved;
    }

    public override void ForceToMoveTo(Vector3Int cellPos)
    {
        ClearDazzle();
        base.ForceToMoveTo(cellPos);
        AssignDazzle();
    }

    public override string GetThirdAbilityDescription()
    {
        return "Prevents all adjacent allies from dying for " + aliveTime + " seconds.";
    }
}

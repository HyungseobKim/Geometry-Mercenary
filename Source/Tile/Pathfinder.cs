/*!*******************************************************************
\file         Pathfinder.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/19/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!*******************************************************************
\class        Pathfinder
\brief		  Find a path from given cell to destination with range.
              Store next cell for the path instead entire path.
********************************************************************/
public sealed class Pathfinder : MonoBehaviour
{
    // Singlton pattern.
    public static Pathfinder instance;
    private Pathfinder() { }
    
    private int iteration = 0; //! Keep how much times of pathfinding done for optimization.
    private PriorityQueue q = new PriorityQueue(); //! Priority queue used for open list.
    private Dictionary<Cell, Dictionary<Cell, Vector3Int>> paths = new Dictionary<Cell, Dictionary<Cell, Vector3Int>>(); //! Path to next cell.

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    /*!
     * \brief Find a path.
     * 
     * \param start
     *        Start position.
     *        
     * \param end
     *        Destination
     *        
     * \param range
     *        Attack or ability range of unit.
     *        
     * \param enenmyUnit
     *        Is this for enemy unit or player unit.
     *        Using for test blocking.
     */
    public int FindPath(Cell start, Cell end, int range, bool enemyUnit)
    {
        // Special case: don't need to move
        if (DistanceBetween(start, end) <= range)
        {
            if (paths.ContainsKey(start))
            {
                if (paths[start].ContainsKey(end))
                    paths[start][end] = start.cellPos;
                else
                    paths[start].Add(end, start.cellPos);
            }
            else
            {
                paths.Add(start, new Dictionary<Cell, Vector3Int>());
                paths[start].Add(end, start.cellPos);
            }

            return 0;
        }

        // Initialize.
        q.Clear();
        ++iteration;

        // Set first node.
        start.iteration = iteration;
        start.cost = DistanceBetween(start, end);
        start.given = 0;
        start.parent = null;
        start.status = Cell.Status.Open;

        q.Insert(start);

        while (q.Empty() == false)
        {
            Cell cell = q.GetTop();
            cell.status = Cell.Status.Closed;

            // Path found
            if (DistanceBetween(cell, end) <= range)
            {
                int length = cell.given;
                while (cell.parent != start)
                    cell = cell.parent;

                if (paths.ContainsKey(start))
                {
                    if (paths[start].ContainsKey(end))
                        paths[start][end] = cell.cellPos;
                    else
                        paths[start].Add(end, cell.cellPos);
                }
                else
                {
                    paths.Add(start, new Dictionary<Cell, Vector3Int>());
                    paths[start].Add(end, cell.cellPos);
                }

                return length;
            }

            int row = cell.cellPos.y & 1;
            for (int i = 0; i < 6; ++i)
            {
                Cell adjacent;
                if (TileManager.instance.cells.TryGetValue(cell.cellPos + new Vector3Int(UnitBase.adjacentX[row, i], UnitBase.adjacentY[i], 0), out adjacent) == false)
                    continue;

                // There is already someone.
                if (adjacent.owner != null)
                {
                    Status enemyStatus = adjacent.owner.GetComponent<Status>();

                    // This is ally, so cannot move here.
                    if (enemyStatus.enemy == enemyUnit)
                        continue;
                    else if (enemyStatus.stealth == false)
                        continue; // There is enemy which is not ambushed, so cannot move here.
                }
                
                // If this cell is block by enemy, cannot pass.
                if (enemyUnit)
                {
                    if (cell.blockEnemyUnits > 0)
                        continue;
                }
                else
                {
                    if (cell.blockPlayerUnits > 0)
                        continue;
                }

                // This node has never been visited for current pathfinding
                if ((adjacent.iteration != iteration) || (adjacent.status == Cell.Status.Default))
                {
                    adjacent.iteration = iteration;
                    adjacent.given = cell.given + 1;
                    adjacent.cost = adjacent.given + DistanceBetween(adjacent, end);
                    adjacent.parent = cell;
                    adjacent.status = Cell.Status.Open;

                    q.Insert(adjacent);
                }
                // This node is already inside open list.
                else if ((adjacent.iteration == iteration) && (adjacent.status == Cell.Status.Open))
                {
                    int new_given = cell.given + 1;
                    int new_cost = new_given + adjacent.cost - adjacent.given;

                    // If new path is better, update.
                    if (new_cost < adjacent.cost)
                    {
                        q.DecreaseKey(adjacent, new_cost);

                        adjacent.parent = cell;
                        adjacent.given = new_given;
                        adjacent.cost = new_cost;
                    }
                }
            }
        }

        // No path
        return -1;
    }

    /*!
     * \brief Helper function to find distance between two cells.
     */
    private int DistanceBetween(Cell cell, Cell end)
    {
        return TileManager.instance.DistanceBetween(cell.cellPos, end.cellPos);
    }

    /*!
     * \brief After pathfinding, return next position to move to follow that path.
     * 
     * \return Vector3Int
     *         Next position to move in cell coordinates.
     */
    public Vector3Int GetNextPos(Cell start, Cell end)
    {
        if (paths.ContainsKey(start) == false)
            return start.cellPos;
        else if (paths[start].ContainsKey(end) == false)
            return start.cellPos;

        return paths[start][end];
    }
}

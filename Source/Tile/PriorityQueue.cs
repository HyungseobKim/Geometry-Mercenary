/*!*******************************************************************
\file         PriorityQueue.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/19/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;

/*!*******************************************************************
\class        PriorityQueue
\brief		  Priority queue for A* pathfinidng.
********************************************************************/
public class PriorityQueue
{
    private List<Cell> cells = new List<Cell>(); //! Sorted collection of cells currently inside queue.
    private CellComparer comparer = new CellComparer(); //! Comparer for cells.

    /*!
     * \brief Insert new cell to the queue.
     * 
     * \param cell
     *        New cell
     */
    public void Insert(Cell cell)
    {
        cells.Insert(FindPosToInsert(cells.Count, cell.cost), cell);
    }

    /*!
     * \brief Returns the cell at the top, and remove from the list.
     * 
     * \return Cell
     *         Cell at the top.
     */
    public Cell GetTop()
    {
        Cell cell = cells[0];
        cells.RemoveAt(0);
        return cell;
    }

    /*!
     * \brief Update cost of cell, and find new position.
     * 
     * \param cell
     *        Cell to update.
     *        
     * \param new_cost
     *        New cost of the cell.
     */
    public void DecreaseKey(Cell cell, int new_cost)
    {
        int last = cells.BinarySearch(cell, comparer);
        
        if (last < 0) // Error
        {
            last = FindPosToInsert(cells.Count, cell.cost);
            cells.Insert(last, cell);
        }

        int index = FindPosToInsert(last, cell.cost);

        for (int i = last; i > index; --i)
            cells[i] = cells[i - 1];

        cells[index] = cell;
    }

    /*!
     * \brief Checks capacity of the queue.
     * 
     * \return bool
     *         If there is no element, return true.
     *         Otherwise, return false.
     */
    public bool Empty()
    {
        return cells.Count == 0;
    }

    /*!
     * \brief Clear the queue.
     */
    public void Clear()
    {
        cells.Clear();
    }

    /*!
     * \brief The number of elements inside queue.
     */
    public int Size()
    {
        return cells.Count;
    }

    /*!
     * \brief Helper function to find new position to insert on the list.
     * 
     * \param last
     *        Searching range.
     *        
     * \param cost
     *        Cost of cell to insert.
     */
    private int FindPosToInsert(int last, int cost)
    {
        int begin = 0;
        int mid = (begin + last) / 2;

        // Do binary search.
        while (begin != last)
        {
            int mid_cost = cells[mid].cost;

            if (cost == mid_cost)
                break;
            else if (mid_cost < cost)
            {
                if (begin == mid)
                    ++begin;
                else
                    begin = mid;
            }
            else
                last = mid;

            mid = (begin + last) / 2;
        }

        return mid;
    }
}

/*!*******************************************************************
\class        CellComparer
\brief		  Comparer class for custom class cell.
********************************************************************/
class CellComparer : IComparer<Cell>
{
    /*!
     * \brief Compare cost of two cells.
     */
    public int Compare(Cell lhs, Cell rhs)
    {
        return lhs.cost.CompareTo(rhs.cost);
    }
}

/*!*******************************************************************
\file         Cell.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/07/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.Tilemaps;

/*!*******************************************************************
\class        Cell
\brief		  Contains some variables for useful information from each tile.
********************************************************************/
public class Cell
{
    public Vector3Int cellPos; //! Index of this cell to the Unity Tilemap.
    public Vector3 worldPos; //! Position of center of cell in world coordinate.

    public GameObject owner = null; //! Indicates which entity is occupying this cell.

    public int blockPlayerUnits = 0; //! Number of enemy units that block this tile.
    public int blockEnemyUnits = 0; //! Number of player units that block this tile.

    /*!*******************************************************************
    \enum        Status
    \brief		 Status of cell on A* pathfinding search.
    ********************************************************************/
    public enum Status
    {
        Open,
        Closed,
        Default
    };

    public Cell parent = null; //! Reference to parent cell. Will be used to restore the path.

    public int cost = 0; //! Sum of given cost and heurstic to destination.
    public int given = 0; //! The actual cost taken to come here from start cell.

    public Status status = Status.Default; //! Variable stores status of this node.
    public int iteration = 0; //! Indicates whether this cell has been used in current search. If it has old value, this node needs to be clear.
}

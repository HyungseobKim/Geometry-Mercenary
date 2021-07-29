/*!*******************************************************************
\file         TileManager.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/07/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]

/*!*******************************************************************
\class        TileManager
\brief        Contains entire tile map and some useful methods to work on tiles.
********************************************************************/
public class TileManager : MonoBehaviour
{
    // Singleton pattern.
    public static TileManager instance;
    private TileManager() { }

    public Tilemap tilemap; //! Reference of Unity Tilemap which is being used for current game.
    public Dictionary<Vector3Int, Cell> cells = new Dictionary<Vector3Int, Cell>(); //! Container for Cell class which matches with each tile to store expanded information for tile.
    public int maxLength { get; } = 10; //! Possible maximum length between two arbitrary cells.

    private void Awake()
    {
        // Singlton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        tilemap = gameObject.GetComponent<Tilemap>();

        // Create cell for each tile from grid.
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos))
                continue;

            Cell cell = new Cell();
            cell.cellPos = pos;
            cell.worldPos = tilemap.CellToWorld(pos);

            cells.Add(pos, cell);
        }
    }

    /*!
     * \brief Find grid distance between two given cells.
     * 
     * \param cellPos1, cellPos2
     *        Position of cell in grid coordinate.
     *        
     * \return int
     *         Distance between two given cells.
     *         If they are adjacent, it would be 1.
     */
    public int DistanceBetween(Vector3Int cellPos1, Vector3Int cellPos2)
    {
        // Converts cell positions into cube coordinate.
        Vector3Int cubePos1 = HexToCube(cellPos1);
        Vector3Int cubePos2 = HexToCube(cellPos2);

        // Find distance on cube coordinate.
        return (Math.Abs(cubePos1.x - cubePos2.x) + Math.Abs(cubePos1.y - cubePos2.y) + Math.Abs(cubePos1.z - cubePos2.z)) / 2;
    }

    /*!
     * \brief Helper method for DistanceBetween method.
     * 
     * \param hex
     *        Position of cell in offset coordinate.
     *        
     * \return Vector3Int
     *         Position of cell in cube coordinate.
     */
    private Vector3Int HexToCube(Vector3Int hex)
    {
        int z = hex.y;

        int x = hex.x;
        if ((z & 1) == 1) // odd row
            x -= (z - 1) / 2;
        else
            x -= z / 2;

        int y = -x - z;

        return new Vector3Int(x, y, z);
    }
}

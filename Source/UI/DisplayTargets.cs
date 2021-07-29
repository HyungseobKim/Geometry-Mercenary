/*!*******************************************************************
\file         DisplayTargets.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/25/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*!*******************************************************************
\class        DisplayTargets
\brief        Highlights a unit which is mouse hovering during a battle.
              Show target enemy and target ally of that unit too.
********************************************************************/
public class DisplayTargets : MonoBehaviour
{
    private Tilemap tilemap; //! Refernce to tilemap.
    private Dictionary<Vector3Int, Cell> cells; //! Reference to collection of cells.

    private GameObject enemyUI; //! UI for target enemy.
    private GameObject allyUI; //! UI for target ally.
    private GameObject selectionUI; //! UI for a unit which is mouse hovering.

    public GameObject unitInfo; //! Simple information about the unit.

    private bool active = true; //! It is used for to activate only during a battle.

    // Start is called before the first frame update
    void Start()
    {
        tilemap = TileManager.instance.tilemap;
        cells = TileManager.instance.cells;

        enemyUI = GameObject.Find("TargetEnemyUI");
        allyUI = GameObject.Find("TargetAllyUI");
        selectionUI = GameObject.Find("SelectedTileUI");

        selectionUI.SetActive(false);
        enemyUI.SetActive(false);
        allyUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        selectionUI.SetActive(false);
        unitInfo.SetActive(false);
        enemyUI.SetActive(false);
        allyUI.SetActive(false);

        if (active == false)
            return;

        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);

        Cell cell;

        if (cells.TryGetValue(cellPos, out cell) == false)
            return;

        selectionUI.transform.position = tilemap.CellToWorld(cellPos);
        selectionUI.SetActive(true);

        if (cell.owner == null)
            return;

        // Show unit info UI
        if (cell.owner.transform.position.x < 0.5f)
            unitInfo.transform.position = cell.owner.transform.position + new Vector3(-1.5f, 0.0f, 0.0f);
        else
            unitInfo.transform.position = cell.owner.transform.position + new Vector3(1.5f, 0.0f, 0.0f);
        unitInfo.GetComponent<UnitInfo>().Initialize(cell.owner);

        // Show unit target UIs
        GameObject targetEnemy = cell.owner.GetComponent<UnitBase>().targetEnemy;
        GameObject targetAlly = cell.owner.GetComponent<UnitBase>().targetAlly;

        if (targetEnemy)
        {
            enemyUI.transform.position = targetEnemy.transform.position;
            enemyUI.SetActive(true);
        }

        if (targetAlly)
        {
            allyUI.transform.position = targetAlly.transform.position;
            allyUI.SetActive(true);
        }
    }

    /*!
     * \brief Turn off the UI.
     */
    public void Inactivate()
    {
        active = false;
    }

    /*!
     * \brief Turn on the UI.
     */
    public void Activate()
    {
        active = true;
    }
}

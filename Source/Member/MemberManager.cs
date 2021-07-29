/*!*******************************************************************
\file         MemberManager.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/05/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!*******************************************************************
\class        MemberManager
\brief		  Manages all player units.
              It provides interface to unit moving, changing,
              item euquipment on editor.
********************************************************************/
public class MemberManager : MonoBehaviour
{
    // Singleton pattern
    public static MemberManager instance;
    private MemberManager() { }

    // Lists for unit data
    private List<UnitData> unitsOnField = new List<UnitData>();
    private List<UnitData> otherUnits = new List<UnitData>();

    // Lists for unit UI
    public List<MemberUnitUI> memberUIs;

    // Helper
    private TileManager tileManager;
    private UnitPrefabs unitPrefabs;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        tileManager = TileManager.instance;
        unitPrefabs = UnitPrefabs.instance;
    }

    /*!
     * \brief Get initial roster of player units.
     *        Spawn first 10 units on field, and others go to substitute list.
     */
    public void Initialize(List<UnitData> unitList)
    {
        int index = 0;

        // Spawn 3 units on each of 3 rows.
        for (int y = -3; y < 0; ++y)
        {
            for (int x = -1; x < 2; ++x)
            {
                UnitData unit = unitList[index];
                unit.position = tileManager.tilemap.CellToWorld(new Vector3Int(x, y, 0));
                unit.index = index;

                unitsOnField.Add(unit);
                SpawnUnit(unit);
                
                ++index;
            }
        }

        // Spawn one more unit at the middle row.
        UnitData lastUnit = unitList[index];
        lastUnit.position = tileManager.tilemap.CellToWorld(new Vector3Int(2, -2, 0));
        lastUnit.index = index;

        unitsOnField.Add(lastUnit);
        SpawnUnit(lastUnit);

        ++index;

        // Add remain units to the list
        while (index < 15)
        {
            UnitData unit = unitList[index];
            unit.index = index;

            otherUnits.Add(unit);
            memberUIs[index - 10].SetUnitData(unit, index);

            ++index;
        }
    }

    /*!
     * \brief Returns all units to where they were before battle starts.
     */
    public void PlaceUnitsOnPosition()
    {
        foreach (UnitData unit in unitsOnField)
        {
            SpawnUnit(unit);

            // Notice to items that this unit has the item.
            if (unit.item1 != null)
                unit.item1.itemUI.equippedObject = unit.gameObject;

            if (unit.item2 != null)
                unit.item2.itemUI.equippedObject = unit.gameObject;
        }
    }

    /*!
     * \brief Spawn a unit according to given data.
     */
    private void SpawnUnit(UnitData data)
    {
        // Spawn.
        data.gameObject = Object.Instantiate(unitPrefabs.GetPrefab(data), data.position, Quaternion.identity);
        Status status = data.gameObject.GetComponent<Status>();

        // Set properites.
        status.enemy = false;
        status.power = data.power;
        status.speed = data.speed;
        status.maxHealth = data.health;
        status.protection = data.protection;
        status.indexOnMemberList = data.index;
        status.cellPos = tileManager.tilemap.WorldToCell(data.position);

        if (data.item1 != null)
            status.items.Add(data.item1);

        if (data.item2 != null)
            status.items.Add(data.item2);
    }

    /*!
     * \brief Swap unit on field with a substitute.
     * 
     * \param memberIndex
     *        Index of the unit will go to the field.
     * 
     * \param unitOnField
     *        Object of the unit will go to the substitute list.
     */
    public void MemberToField(int memberIndex, GameObject unitOnField)
    {
        int indexOnField = unitOnField.GetComponent<Status>().indexOnMemberList;
        UnitData fieldUnitData = unitsOnField[indexOnField];

        // Unequip all items from field unit.
        if (fieldUnitData.item1 != null)
        {
            UnequipFirstUpgrade(unitOnField);
            unitOnField = fieldUnitData.gameObject;
        }

        // Swap index of each unit.
        UnitData dataToSpawn = otherUnits[memberIndex - 10];
        dataToSpawn.index = indexOnField;
        fieldUnitData.index = memberIndex;

        // Spawn unit with new position.
        dataToSpawn.position = unitOnField.transform.position;
        SpawnUnit(dataToSpawn);

        // Move field unit to member list and set UI.
        otherUnits[memberIndex - 10] = fieldUnitData;
        memberUIs[memberIndex - 10].SetUnitData(fieldUnitData, memberIndex);

        // Move member unit to field list and destroy old object.
        unitsOnField[indexOnField] = dataToSpawn;
        UnitManager.instance.ReportDeath(unitOnField);
        Destroy(unitOnField);
    }

    /*!
     * \brief Equip an item to a unit.
     * 
     * \param unitOnField
     *        Object of the unit to equip the item.
     *        
     * \param item
     *        Item to equip.
     */
    public GameObject EquipItem(GameObject unitOnField, Item item)
    {
        int index = unitOnField.GetComponent<Status>().indexOnMemberList;
        UnitData unitData = unitsOnField[index];

        if (item.CanEquip(unitData) == false)
            return null;

        item.OnItemEquip(ref unitData);
        SpawnUnit(unitData);

        // If there is item 2,
        if (unitData.item2 != null) // link new object to UI of item 1.
            unitData.item1.itemUI.equippedObject = unitData.gameObject;

        Destroy(unitOnField);
        return unitData.gameObject;
    }

    /*!
     * \brief Unequip an item from a unit.
     * 
     * \param unitOnField
     *        Object of the unit to unequip the item.
     *        
     * \param item
     *        Item to unequip.
     */
    public void UnequipItem(GameObject unitOnField, Item item)
    {
        int index = unitOnField.GetComponent<Status>().indexOnMemberList;
        UnitData unitData = unitsOnField[index];

        if (unitData.item2 == item)
            UnequipSecondUpgrade(unitOnField);
        else if (unitData.item1 == item)
            UnequipFirstUpgrade(unitOnField);
    }

    /*!
     * \brief Unequip first item from a unit.
     *        If the unit also has a second item, unequip that too.
     * 
     * \param unitOnField
     *        Object of the unit to unequip the item.
     */
    public void UnequipFirstUpgrade(GameObject unitOnField)
    {
        int index = unitOnField.GetComponent<Status>().indexOnMemberList;
        UnitData unitData = unitsOnField[index];

        if (unitData.item2 != null)
            unitData.item2.OnItemUnequip(unitData);

        if (unitData.item1 != null)
            unitData.item1.OnItemUnequip(unitData);

        SpawnUnit(unitData);
        Destroy(unitOnField);
    }

    /*!
     * \brief Unequip second item from a unit.
     * 
     * \param unitOnField
     *        Object of the unit to unequip the item.
     */
    public void UnequipSecondUpgrade(GameObject unitOnField)
    {
        int index = unitOnField.GetComponent<Status>().indexOnMemberList;
        UnitData unitData = unitsOnField[index];

        unitData.item2.OnItemUnequip(unitData);

        SpawnUnit(unitData);
        Destroy(unitOnField);

        if (unitData.item1 != null)
            unitData.item1.itemUI.equippedObject = unitData.gameObject;
    }

    /*!
     * \brief Swap position of two units on field.
     * 
     * \param index1, index2
     *        Index to the list of units to swap their position.
     */
    public void SwapPosition(int index1, int index2)
    {
        UnitData unit1 = unitsOnField[index1];
        UnitData unit2 = unitsOnField[index2];

        var temp = unit1.position;
        unit1.position = unit2.position;
        unit2.position = temp;
    }

    /*!
     * \brief Save new position of an unit.
     * 
     * \param index
     *        Index to the list of units to move.
     *        
     * \param position
     *        Position to move.
     */
    public void MoveUnitTo(int index, Vector3 position)
    {
        UnitData unit = unitsOnField[index];
        unit.position = position;
    }

    /*!
     * \brief Getter method for units.
     * 
     * \param memberIndex
     *        Index to the list of units.
     *        If it is out of the range of units on field, it is index for substitute units.
     *        
     * \return UnitData
     *         Data of unit.
     */
    public UnitData GetUnitData(int memberIndex)
    {
        if (memberIndex < 0)
            return null;

        if (memberIndex < unitsOnField.Count)
            return unitsOnField[memberIndex];

        return otherUnits[memberIndex - unitsOnField.Count];
    }
}

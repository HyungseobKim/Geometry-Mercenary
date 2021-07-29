/*!*******************************************************************
\file         StartingUnitSelector.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/04/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*!*******************************************************************
\class        StartingUnitSelector
\brief		  Manages starting unit selection process.
********************************************************************/
public class StartingUnitSelector : MonoBehaviour
{
    // Singleton pattern.
    public static StartingUnitSelector instance;
    private StartingUnitSelector() { }

    public int PwrMIn = 10; //! Mininum range of power.
    public int PwrMax = 30; //! Maximum range of power.
    public int SpdMin = 10; //! Mininum range of speed.
    public int SpdMax = 30; //! Maximum range of speed.
    public int PwrSpdTotal = 40; //! Default value of power + speed.
    public int standardHealth = 100; //! Health when power + speed is the same with PwrSpdTotal.

    public List<GameObject> UIs; //! List of column UIs.
    private List<UnitData> units; //! List of unit data corresponds to each column.

    void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize data
        int size = UIs.Count;
        units = new List<UnitData>(size);

        for (int i = 0; i < size; ++i)
        {
            UnitData unit = new UnitData();
            unit.baseType = UnitData.Type.Default;

            units.Add(unit);
        }

        Reroll();

        // Assign index to UIs
        for (int i = 0; i < size; ++i)
            UIs[i].GetComponent<StartingUnitColumn>().index = i;
    }

    /*!
     * \brief Set status of all units randomly.
     */
    public void Reroll()
    {
        int size = UIs.Count;

        for (int i = 0; i < size; ++i)
        {
            UnitData unit = units[i];

            // Set values.
            unit.power = Random.Range(PwrMIn, PwrMax+1);
            unit.speed = Random.Range(SpdMin, SpdMax+1);
            unit.health = standardHealth * PwrSpdTotal / (unit.power + unit.speed); // High power+speed, low health, vice versa.
            unit.baseType = UnitData.Type.Default;
            units[i] = unit;

            GameObject UI = UIs[i];

            // Set UIs.
            UI.GetComponent<StartingUnitColumn>().Reset();
            UI.transform.Find("Health").GetComponent<Text>().text = Mathf.Floor(unit.health).ToString();
            UI.transform.Find("Speed").GetComponent<Text>().text = Mathf.Floor(unit.speed).ToString();
            UI.transform.Find("Power").GetComponent<Text>().text = Mathf.Floor(unit.power).ToString();
        }
    }

    /*!
     * \brief Check whether all units have class.
     *        If have, destroy all objects for starting unit selection, and pass decided unit information.
     */
    public void Confirm()
    {
        // Check all units have class.
        foreach (var unit in units)
        {
            if (unit.baseType == UnitData.Type.Default)
                return;
        }

        // Code for debugging.
        //for (int i = 0; i < units.Count; ++i)
        //{
        //    Debug.Log(i);
            
        //    if (units[i].baseType == UnitData.Type.Default)
        //    {
        //        return;
        //    }
        //}

        // Start game.
        UnitManager.instance.GameStart();
        MemberManager.instance.Initialize(units);
        AudioManager.instance.Play("RosterConfirm");

        // Destroy objects not need anymore.
        Destroy(GameObject.Find("StartingUICanvas"));
        Destroy(GameObject.Find("DescriptionCanvas"));

        Destroy(gameObject);
    }

    /*!
     * \brief Assign Defender to the column and related unit.
     * 
     * \param index
     *        Index of column UI and unit to assign Defender.
     */
    public void AssignDefender(int index)
    {
        var unit = units[index];

        unit.baseType = UnitData.Type.Defender;

        units[index] = unit;
    }

    /*!
     * \brief Assign Healer to the column and related unit.
     * 
     * \param index
     *        Index of column UI and unit to assign Healer.
     */
    public void AssignHealer(int index)
    {
        var unit = units[index];

        unit.baseType = UnitData.Type.Healer;

        units[index] = unit;
    }

    /*!
     * \brief Assign Supporter to the column and related unit.
     * 
     * \param index
     *        Index of column UI and unit to assign Supporter.
     */
    public void AssignSupporter(int index)
    {
        var unit = units[index];

        unit.baseType = UnitData.Type.Supporter;

        units[index] = unit;
    }

    /*!
     * \brief Assign Melee to the column and related unit.
     * 
     * \param index
     *        Index of column UI and unit to assign Melee.
     */
    public void AssignMelee(int index)
    {
        var unit = units[index];

        unit.baseType = UnitData.Type.Melee;

        units[index] = unit;
    }

    /*!
     * \brief Assign Forward to the column and related unit.
     * 
     * \param index
     *        Index of column UI and unit to assign Forward.
     */
    public void AssignForward(int index)
    {
        var unit = units[index];

        unit.baseType = UnitData.Type.Forward;

        units[index] = unit;
    }
}

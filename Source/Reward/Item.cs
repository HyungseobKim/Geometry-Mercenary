/*!*******************************************************************
\file         Item.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/09/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!*******************************************************************
\class        Item
\brief		  Item class contains upgrade and options information.
********************************************************************/
public class Item
{
    [HideInInspector]
    public UnitData.Type upgrade; //! Type of upgrade that this item provides.
    [HideInInspector]
    public int numberOfOptions; //! Number of options.
    [HideInInspector]
    public List<ItemOption> options; //! List of options this item have.
    public ItemUI itemUI; //! Reference to UI object that shows this item.

    /*!
     * \brief Choose one random upgrade, and initialize each options.
     * 
     * \param newOptions
     *        Number of options that this item will have.
     */
    public void Initialize(int newOptions)
    {
        // Get random base class.
        upgrade = (UnitData.Type)Random.Range(0, UnitData.Type.GetValues(typeof(UnitData.Type)).Length - 1);

        options = new List<ItemOption>();
        numberOfOptions = newOptions;

        if (numberOfOptions == 1)
        {
            switch ((ItemOption.Type)Random.Range(0, ItemOption.Type.GetNames(typeof(ItemOption.Type)).Length))
            {
                case ItemOption.Type.Power:
                    options.Add(new OptionPower(numberOfOptions));
                    break;

                case ItemOption.Type.Speed:
                    options.Add(new OptionSpeed(numberOfOptions));
                    break;

                case ItemOption.Type.Health:
                    options.Add(new OptionHealth(numberOfOptions));
                    break;

                case ItemOption.Type.Protection:
                    options.Add(new OptionProtection(numberOfOptions));
                    break;

                case ItemOption.Type.Regeneration:
                    options.Add(new OptionRegeneration(numberOfOptions));
                    break;

                case ItemOption.Type.Upgrade: // This is option is only available when there is only one option.
                    options.Add(new OptionUpgrade(upgrade));
                    break;
            }

            return;
        }

        for (int i = 0; i < numberOfOptions; ++i)
        {
            switch((ItemOption.Type)Random.Range(0, ItemOption.Type.GetNames(typeof(ItemOption.Type)).Length - 1))
            {
                case ItemOption.Type.Power:
                    options.Add(new OptionPower(numberOfOptions));
                    break;

                case ItemOption.Type.Speed:
                    options.Add(new OptionSpeed(numberOfOptions));
                    break;

                case ItemOption.Type.Health:
                    options.Add(new OptionHealth(numberOfOptions));
                    break;

                case ItemOption.Type.Protection:
                    options.Add(new OptionProtection(numberOfOptions));
                    break;

                case ItemOption.Type.Regeneration:
                    options.Add(new OptionRegeneration(numberOfOptions));
                    break;
            }
        }
    }

    /*!
     * \brief Apply effects of this item to given unit data.
     * 
     * \param unit
     *        Unit data to equip this item.
     */
    public void OnItemEquip(ref UnitData unit)
    {
        if (CanEquip(unit) == false)
            return; // Error.

        if (unit.item1 == null)
        {
            unit.secondType = upgrade;
            unit.item1 = this;
        }
        else if (unit.item2 == null)
        {
            unit.thirdType = upgrade;
            unit.item2 = this;
        }
        else // There is no available slot.
            return;
        
        foreach (var option in options)
            option.OnItemEquip(ref unit);
    }

    /*!
     * \brief Remove all effects of this item from unit data.
     * 
     * \param unit
     *        Unit data to unequip.
     */
    public void OnItemUnequip(UnitData unit)
    {
        if (unit.item2 == this)
        {
            unit.thirdType = UnitData.Type.Default;
            unit.item2 = null;
        }
        else if (unit.item2 != null)
            return; // Error.
        else if (unit.item1 == this)
        {
            unit.secondType = UnitData.Type.Default;
            unit.item1 = null;
        }
        else if (unit.item1 == null)
            return; // Error.

        // Release UI.
        itemUI.Unequip();

        foreach (var option in options)
            option.OnItemUnequip(ref unit);
    }

    /*!
     * \brief Call update function of each option.
     * 
     * \param dt
     *        Delta time.
     *        
     * \param unit
     *        Unit to update.
     */
    public void OnItemUpdate(float dt, GameObject unit)
    {
        foreach (var option in options)
            option.OnItemUpdate(dt, unit);
    }

    /*!
     * \brief Check whether given unit can equip this item.
     * 
     * \param unit
     *        Unit data to check.
     *        
     * \return bool
     *         Return true if given unit can equip this item.
     *         Otherwise, return false.
     */
    public bool CanEquip(UnitData unit)
    {
        // Full of items.
        if (unit.item2 != null)
            return false;

        if (options[0].type == ItemOption.Type.Upgrade)
        {
            // It must have no item.
            if (unit.item1 != null)
                return false;

            if (unit.baseType == ((OptionUpgrade)options[0]).upgrade)
                return false;
        }

        if (unit.secondType == upgrade)
            return false;

        return upgrade != unit.baseType;
    }
}

/*!*******************************************************************
\file         OptionUpgrade.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/11/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System;
using UnityEngine;

/*!*******************************************************************
\class        OptionUpgrade
\brief		  One of item options allows additional upgrade.
********************************************************************/
public class OptionUpgrade : ItemOption
{
    public UnitData.Type upgrade; //! Type of upgrade.
    private static Array types = Enum.GetValues(typeof(UnitData.Type)); //! All types of upgrade for easy iteration.

    /*!
     * \brief Constructor set type of upgrade.
     * 
     * \param baseType
     *        Another upgrade type must not duplicate with base upgrade type of this item.
     */
    public OptionUpgrade(UnitData.Type baseType)
    {
        type = Type.Upgrade;

        upgrade = (UnitData.Type)types.GetValue(UnityEngine.Random.Range(0, types.Length - 2));

        if (upgrade == baseType)
            upgrade = (UnitData.Type)types.GetValue(types.Length - 2);
    }

    public override void OnItemEquip(ref UnitData unit)
    {
        if (unit.item1.itemUI.equippedObject)
            MemberManager.instance.UnequipItem(unit.item1.itemUI.equippedObject, unit.item1);

        unit.thirdType = upgrade;
        unit.item2 = unit.item1;
    }

    public override void OnItemUnequip(ref UnitData unit)
    {
        unit.secondType = UnitData.Type.Default;
        unit.thirdType = UnitData.Type.Default;
        unit.item1 = null;
        unit.item2 = null;
    }

    public override void OnItemUpdate(float dt, GameObject unit)
    {
        // do nothing
    }

    public override UnitData.Type GetUpgradeType()
    {
        return upgrade;
    }

    public override string GetDescription()
    {
        return "";
    }

    public override string GetShortDescription()
    {
        return "";
    }
}

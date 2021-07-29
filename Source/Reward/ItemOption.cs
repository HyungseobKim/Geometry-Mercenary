/*!*******************************************************************
\file         ItemOption.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/10/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        ItemOption
\brief		  Represents each options of item.
              There are types, amount of effect, and methods to manage them.
********************************************************************/
public abstract class ItemOption
{
    /*!*******************************************************************
    \enum        Type
    \brief		 Types of options.
    ********************************************************************/
    public enum Type
    {
        Power,
        Speed,
        Health,
        Protection,
        Regeneration,
        Upgrade
    }

    public Type type; //! What this option affects.
    protected int effectAmount; //! Amount of effect.

    /*!
     * \brief Apply option effect to given unit.
     * 
     * \param unit
     *        Data of unit to aplpy this option.
     */
    public abstract void OnItemEquip(ref UnitData unit);
    /*!
     * \brief Remove effect from given unit.
     * 
     * \param unit
     *        Data of unit to remove this option.
     */
    public abstract void OnItemUnequip(ref UnitData unit);

    /*!
     * \brief Update this option.
     * 
     * \param dt
     *        Delta time.
     *        
     * \param unit
     *        Unit to update.
     */
    public abstract void OnItemUpdate(float dt, GameObject unit);

    /*!
     * \brief If the type of this option is Upgrade, returns type of upgrade.
     * 
     * \return UnitData.Type
     *         Type of upgrade.
     */
    public virtual UnitData.Type GetUpgradeType()
    {
        return UnitData.Type.Default;
    }
    /*!
     * \brief Return description for reward selection UI.
     * 
     * \return string
     *         Description of this option.
     */
    public abstract string GetDescription();
    /*!
     * \brief Return short description for inventory.
     * 
     * \return string
     *         Short description of this option.
     */
    public abstract string GetShortDescription();
}

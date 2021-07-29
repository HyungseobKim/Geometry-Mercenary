/*!*******************************************************************
\file         OptionSpeed.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/11/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        OptionSpeed
\brief		  One of item options increases speed.
********************************************************************/
public class OptionSpeed : ItemOption
{
    /*!
     * \brief Constructor set amount of effect and type.
     * 
     * \param numberOfOptions
     *        Amount of effect should be set depending on the number of options.
     */
    public OptionSpeed(int numberOfOptions)
    {
        type = Type.Power;

        switch (numberOfOptions)
        {
            case 1:
                effectAmount = 8;
                break;

            case 2:
                effectAmount = Random.Range(3, 4);
                break;

            case 3:
                effectAmount = Random.Range(2, 3);
                break;
        }
    }

    public override void OnItemEquip(ref UnitData unit)
    {
        unit.speed += effectAmount;
    }

    public override void OnItemUnequip(ref UnitData unit)
    {
        unit.speed -= effectAmount;
    }

    public override void OnItemUpdate(float dt, GameObject unit)
    {
        // do nothing
    }

    public override string GetDescription()
    {
        return "Speed +" + effectAmount;
    }

    public override string GetShortDescription()
    {
        return "Spd " + effectAmount;
    }
}
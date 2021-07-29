/*!*******************************************************************
\file         OptionHealth.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/11/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        OptionHealth
\brief		  One of item options increases health.
********************************************************************/
public class OptionHealth : ItemOption
{
    /*!
     * \brief Constructor set amount of effect and type.
     * 
     * \param numberOfOptions
     *        Amount of effect should be set depending on the number of options.
     */
    public OptionHealth(int numberOfOptions)
    {
        type = Type.Health;

        switch (numberOfOptions)
        {
            case 1:
                effectAmount = 70;
                break;

            case 2:
                effectAmount = (Random.Range(0, 2) * 5) + 30;
                break;

            case 3:
                effectAmount = (Random.Range(0, 2) * 5) + 20;
                break;
        }
    }

    public override void OnItemEquip(ref UnitData unit)
    {
        unit.health += effectAmount;
    }

    public override void OnItemUnequip(ref UnitData unit)
    {
        unit.health -= effectAmount;
    }

    public override void OnItemUpdate(float dt, GameObject unit)
    {
        // do nothing
    }

    public override string GetDescription()
    {
        return "Health +" + effectAmount;
    }

    public override string GetShortDescription()
    {
        return "HP " + effectAmount;
    }
}

/*!*******************************************************************
\file         OptionRegeneration.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/06/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        OptionRegeneration
\brief		  One of item options that keeps healing unit.
********************************************************************/
public class OptionRegeneration : ItemOption
{
    private float timer;
    public float timeToHeal = 5.0f; //! Regeneration interval.

    /*!
     * \brief Constructor set amount of effect and type.
     * 
     * \param numberOfOptions
     *        Amount of effect should be set depending on the number of options.
     */
    public OptionRegeneration(int numberOfOptions)
    {
        type = Type.Regeneration;

        switch (numberOfOptions)
        {
            case 1:
                effectAmount = 15;
                break;

            case 2:
                effectAmount = Random.Range(6, 7);
                break;

            case 3:
                effectAmount = Random.Range(4, 5);
                break;
        }
    }

    public override void OnItemEquip(ref UnitData unit)
    {
        // Initialize timer.
        timer = timeToHeal;
    }

    public override void OnItemUnequip(ref UnitData unit)
    {
        // do nothing
    }

    public override void OnItemUpdate(float dt, GameObject unit)
    {
        if (timer >= 0.0f)
        {
            // Update timer.
            timer -= dt;

            if (timer <= 0.0f)
            {
                // Heal
                unit.GetComponent<UnitBase>().GetHeal(effectAmount);
                // Reset timer.
                timer = timeToHeal;
            }
        }
    }

    public override string GetDescription()
    {
        return "Regeneration +" + effectAmount;
    }

    public override string GetShortDescription()
    {
        return "Reg " + effectAmount;
    }
}

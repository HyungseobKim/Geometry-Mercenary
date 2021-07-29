/*!*******************************************************************
\file         TacticsUI.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/15/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.UI;

/*!*******************************************************************
\class        TacticsUI
\brief		  Manages UI for tactics options and abilities.
********************************************************************/
public class TacticsUI : MonoBehaviour
{
    public Text baseAbilityDescription; //! Text UI for base ability.
    public GameObject[] upgradeButtons; //! Objects for upgrade ability UIs.
    public Text[] upgradeDescriptions; //! Texts for upgrade ability UIs.

    public GameObject movementUI; //! Object manages movement option.
    public GameObject aggressionUI; //! Object manages aggression option.

    /*!
     * \brief Set descriptions for abilities and initialize each tactics options.
     * 
     * \param data
     *        Data of unit to display.
     */
    public void Initialize(UnitData data)
    {
        // Set ability descriptions
        UnitBase unitBase = data.gameObject.GetComponent<UnitBase>();

        // Base ability description.
        baseAbilityDescription.text = unitBase.GetBaseAbilityDescription();

        // This unit is upgraded.
        if (data.secondType != UnitData.Type.Default)
        {
            // Set first upgrade ability.
            upgradeButtons[0].SetActive(true);
            upgradeDescriptions[0].text = unitBase.GetSecondAbilityDescription();

            // This unit is upgraded twice.
            if (data.thirdType != UnitData.Type.Default)
            {
                upgradeButtons[1].SetActive(true);
                upgradeDescriptions[1].text = unitBase.GetThirdAbilityDescription();
            }
            else // It doesn't have third ability.
                upgradeButtons[1].SetActive(false);
        }
        else // There is no other abilities.
        {
            upgradeButtons[0].SetActive(false);
            upgradeButtons[1].SetActive(false);
        }

        // Set options
        Status unitStatus = data.gameObject.GetComponent<Status>();

        // This unit uses movement option.
        if (unitStatus.useMovement)
        {
            movementUI.SetActive(true);
            movementUI.GetComponent<TacticsOptionUI>().Initialize(data, true);
        }
        else // This unit does not use this option
            movementUI.SetActive(false);

        // This unit uses aggression option.
        if (unitStatus.useAggression)
        {
            aggressionUI.SetActive(true);
            aggressionUI.GetComponent<TacticsOptionUI>().Initialize(data, false);
        }
        else // This unit does not use this option
            aggressionUI.SetActive(false);
    }
}

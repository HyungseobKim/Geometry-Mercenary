/*!*******************************************************************
\file         TacticsOptionUI.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/14/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        TacticsOptionUI
\brief		  Manages each category of tactics option.
********************************************************************/
public class TacticsOptionUI : MonoBehaviour
{
    public TacticsOptionButton[] optionButtons; //! All buttons under this category.
    private UnitData unitData; //! Data of unit to show tactics option.

    private bool movement; //! Indicates that category of this option is for movement or aggression.

    private TacticsOptionButton selectedButton; //! Indicates currently selected button.

    // Start is called before the first frame update
    void Awake()
    {
        double value = -0.5;

        // Set default values for each button.
        foreach (var button in optionButtons)
        {
            button.optionUI = this;
            button.value = value;
            value += 0.5;
        }
    }

    /*!
     * \brief Initialize buttons appropriate to given unit data.
     * 
     * \param data
     *        Unit to show tactics option.
     *        
     * \param movement
     *        If true, this option is for movement.
     *        If not, this option is for aggression.
     */
    public void Initialize(UnitData data, bool isMovement)
    {
        unitData = data;
        movement = isMovement;

        foreach (var button in optionButtons)
        {
            if (button.Initialize(data, movement))
                selectedButton = button;
        }
    }

    /*!
     * \brief When player selected different button, handles that.
     * 
     * \param button
     *        Selected button.
     */
    public void NewSelection(TacticsOptionButton button)
    {
        // Release previous button, and store new one.
        selectedButton.Deselect();
        selectedButton = button;

        // Change the data on unit.
        if (movement)
        {
            unitData.movement = button.value;
            unitData.gameObject.GetComponent<Status>().movement = button.value;
        }
        else
        {
            unitData.aggression = button.value;
            unitData.gameObject.GetComponent<Status>().aggression = button.value;
        }
    }
}

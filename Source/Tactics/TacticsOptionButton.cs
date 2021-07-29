/*!*******************************************************************
\file         TacticsOptionButton.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/15/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        TacticsOptionButton
\brief		  Buttons allow players to adjust tactics option for each unit.
********************************************************************/
public class TacticsOptionButton : MonoBehaviour
{
    // UI objects for feedback to user.
    public GameObject selectedBackground;
    public GameObject hoveringBackground;

    public double value; //! The amount of bonus affects to AI decision.
    public TacticsOptionUI optionUI; //! Category of tactics that this button belongs to.

    // Start is called before the first frame update
    void Start()
    {
        hoveringBackground.SetActive(false);
    }

    /*!
     * \brief Checks current value of unit and set appropriate to that.
     * 
     * \param data
     *        Unit to show tactics option.
     *        
     * \param movement
     *        If true, this button is for movement.
     *        If not, this button is for aggression.
     */
    public bool Initialize(UnitData data, bool movement)
    {
        if (movement)
        {
            if (data.movement == value)
            {
                Select();
                return true;
            }
            else
                Deselect();
        }
        else
        {
            if (data.aggression == value)
            {
                Select();
                return true;
            }
            else
                Deselect();
        }

        return false;
    }

    void OnMouseEnter()
    {
        hoveringBackground.SetActive(true);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Tells that this button is selected.
            optionUI.NewSelection(this);
            Select();
        }
    }

    void OnMouseExit()
    {
        hoveringBackground.SetActive(false);
    }

    /*!
     * \brief Helper function for when selected.
     */
    public void Select()
    {
        selectedBackground.SetActive(true);
    }

    /*!
     * \brief Helper function for when deselected.
     */
    public void Deselect()
    {
        selectedBackground.SetActive(false);
    }
}

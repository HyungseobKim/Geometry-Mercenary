/*!*******************************************************************
\file         UpgradeDescription.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/20/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        UpgradeDescription
\brief		  Description of each upgraded ability.
              Players can unequip item from this.
********************************************************************/
public class UpgradeDescription : MonoBehaviour
{
    public int upgradeNumber = 0; //! First upgrade:1, Second upgrade:2.

    // Objects for user feedback.
    public SpriteRenderer upBar;
    public SpriteRenderer downBar;
    public SpriteRenderer leftBar;
    public SpriteRenderer rightBar;

    // Some pre-defined colors.
    private static Color originalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private static Color hoveringColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

    void OnMouseEnter()
    {
        ChangeColor(hoveringColor);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Unequip this item.
            ChangeColor(originalColor);
            InputManager.instance.UnequipFromTactics(upgradeNumber);
        }
    }

    void OnMouseExit()
    {
        ChangeColor(originalColor);
    }

    /*!
     * \brief Helper function to change colors.
     */
    private void ChangeColor(Color color)
    {
        upBar.color = color;
        downBar.color = color;
        leftBar.color = color;
        rightBar.color = color;
    }
}

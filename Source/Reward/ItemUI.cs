/*!*******************************************************************
\file         ItemUI.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/12/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        ItemUI
\brief		  Represents each item in the inventory.
              Contains methods allow to use this item.
********************************************************************/
public class ItemUI : MonoBehaviour
{
    public Transform upgradeTransform; //! Position to spawn upgrade icon.
    public GameObject option; //! Position of options text.

    private Item item; //! Item of this UI.

    public GameObject equippedObject; //! The unit that has equipped this item.
    private bool clicked = false; //! Indicates whether this UI is clicked or not.
    
    // Objects for user feedback.
    public SpriteRenderer upBar;
    public SpriteRenderer downBar;
    public SpriteRenderer leftBar;
    public SpriteRenderer rightBar;
    public GameObject background;

    // Some pre-defined colors for use feedback.
    private static Color originalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private static Color clickedColor = new Color(0.2f, 0.2f, 1.0f, 1.0f);
    private static Color hoveringColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

    /*!
     * \brief Create an icon and set description text for options.
     * 
     * \param newItem
     *        New item of this UI.
     */
    public void Initialize(Item newItem)
    {
        item = newItem;
        item.itemUI = this;

        Object.Instantiate(MemberUnitUI.iconPrefabs[item.upgrade], upgradeTransform.position, Quaternion.identity);

        // Option is the other upgrade.
        if (item.options[0].type == ItemOption.Type.Upgrade)
        {
            // Spawn icon.
            Object.Instantiate(MemberUnitUI.iconPrefabs[((OptionUpgrade)item.options[0]).upgrade], option.transform.position, Quaternion.identity);

            // Text doesn't need.
            option.SetActive(false);
        }
        else // Set text.
        {
            Text text = option.GetComponent<Text>();

            text.text = item.options[0].GetShortDescription();
            for (int i = 1; i < item.numberOfOptions; ++i)
                text.text += (" / " + item.options[i].GetShortDescription());
        }

        // Unclicked as default.
        background.SetActive(false);
    }

    /*!
     * \brief Getter method for item.
     */
    public Item GetItem()
    {
        return item;
    }

    /*!
     * \brief Store new unit that will equip this item, and set UI.
     *        If there was an unit previously taken this item, unequip from that unit.
     * 
     * \param newObject
     *        New unit to equip this item.
     */
    public void Equip(GameObject newObject)
    {
        // If this item was equipped to different unit, unequip first.
        if (equippedObject && equippedObject != newObject)
            MemberManager.instance.UnequipItem(equippedObject, item);
        
        background.SetActive(true);
        equippedObject = newObject;
    }

    /*!
     * \brief Set UI when this item unequipped.
     */
    public void Unequip()
    {
        background.SetActive(false);
        equippedObject = null;
    }

    /*!
     * \brief Set UI when this UI is deselected.
     */
    public void Deselect()
    {
        ChangeColor(originalColor);
        clicked = false;
    }

    void OnMouseEnter()
    {
        if (clicked == false)
            ChangeColor(hoveringColor);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && UnitManager.instance.onBattle == false)
        {
            ChangeColor(clickedColor);
            clicked = true;

            //UnitChange.instance.ItemPressed(this);
            InputManager.instance.ItemUIPressed(this);
        }
    }

    void OnMouseExit()
    {
        if (clicked == false)
        {
            ChangeColor(originalColor);
            clicked = false;
        }
    }

    /*!
     * \brief Helper function to change color of all bars.
     */
    private void ChangeColor(Color color)
    {
        upBar.color = color;
        downBar.color = color;
        leftBar.color = color;
        rightBar.color = color;
    }
}

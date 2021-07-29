/*!*******************************************************************
\file         RewardSelection.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/10/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        RewardSelection
\brief		  Represents each possible choice of reward.
********************************************************************/
public class RewardSelection : MonoBehaviour
{
    // Vairables for item
    private Item item; //! The item it shows.

    public Transform upgradeTransform; //! Position to spawn upgrade icon.
    public GameObject[] optionUIs; //! Positions of option texts.

    private GameObject upgradeIcon = null; //! Common upgrade icon object.
    private GameObject upgradeOption = null; //! Upgrade icon object for an option.

    // Variables for mouse feedback
    public SpriteRenderer upBar;
    public SpriteRenderer downBar;
    public SpriteRenderer leftBar;
    public SpriteRenderer rightBar;

    // Some pre-defined colors for use feedback.
    private Color originalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private Color hoveringColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

    /*!
     * \brief Spawn icons and texts corresponding to new item.
     * 
     * \param newItem
     *        New item for this UI.
     */
    public void Initialize(Item newItem)
    {
        ChangeColor(originalColor);

        item = newItem;

        // Create new upgrade icon.
        upgradeIcon = SpwanIcon(MemberUnitUI.iconPrefabs[item.upgrade], upgradeTransform);

        if (item.options[0].type == ItemOption.Type.Upgrade)
        {
            // Not use texts
            foreach (GameObject UI in optionUIs)
                UI.SetActive(false);

            // Put icon instead of text.
            upgradeOption = SpwanIcon(MemberUnitUI.iconPrefabs[item.options[0].GetUpgradeType()], optionUIs[1].transform);
        }
        else
        {
            // Set option descriptions.
            for (int i = 0; i < 3; ++i)
            {
                if (i < item.numberOfOptions)
                {
                    optionUIs[i].SetActive(true);
                    optionUIs[i].GetComponent<Text>().text = item.options[i].GetDescription();
                }
                else
                    optionUIs[i].SetActive(false);
            }
        }
    }

    void OnMouseEnter()
    {
        ChangeColor(hoveringColor);
    }

    /*!
     * \brief When player clicks this, give item to inventory and finishes selection step.
     */
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Inventory.instance.NewItem(item);
            RewardManager.instance.SelectionMade();
        }
    }

    void OnMouseExit()
    {
        ChangeColor(originalColor);
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

    /*!
     * \brief Helper function to spawn icons.
     * 
     * \param prefab
     *        Prefab of icon to create.
     *        
     * \param transform
     *        Position to spawn the icon.
     */
    private GameObject SpwanIcon(GameObject prefab, Transform transform)
    {
        // Create icon.
        GameObject icon = Object.Instantiate(prefab, transform.position, Quaternion.identity);
        SpriteRenderer renderer = icon.GetComponent<SpriteRenderer>();

        // Set properties.
        icon.transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);
        renderer.sortingLayerName = "UI";
        renderer.sortingOrder = 1;
        
        return icon;
    }

    /*!
     * \brief Helper function to change color of all bars.
     */
    public void DestoryOldIcons()
    {
        // Destroy old icons.
        if (upgradeIcon)
            Destroy(upgradeIcon);

        if (upgradeOption)
            Destroy(upgradeOption);
    }
}

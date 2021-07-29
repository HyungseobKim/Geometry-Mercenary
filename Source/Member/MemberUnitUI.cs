/*!*******************************************************************
\file         MemberUnitUI.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/06/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        MemberUnitUI
\brief		  UI shows single substitute unit and allows interaction.
********************************************************************/
public class MemberUnitUI : MonoBehaviour
{
    public static Dictionary<UnitData.Type, GameObject> iconPrefabs = new Dictionary<UnitData.Type, GameObject>();

    // Status texts.
    public Text health;
    public Text speed;
    public Text power;

    // Type icon pos and object.
    public Transform baseTypePos;
    private GameObject baseTypeObject;

    // Bars form a boundary.
    public SpriteRenderer upBar;
    public SpriteRenderer downBar;
    public SpriteRenderer leftBar;
    public SpriteRenderer rightBar;

    // Colors.
    private static Color originalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    private static Color clickedColor = new Color(0.2f, 0.2f, 1.0f, 1.0f);
    private static Color hoveringColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

    private bool clicked = false; //! Indicates whether this unit has been clicked or not.

    private int index; //! Index of this unit on member manager unit list.

    // Start is called before the first frame update
    void Start()
    {
        // Load all icons.
        if (iconPrefabs.Count == 0)
        {
            iconPrefabs.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Icons/DefenderIcon", typeof(GameObject)) as GameObject);
            iconPrefabs.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Icons/HealerIcon", typeof(GameObject)) as GameObject);
            iconPrefabs.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Icons/SupporterIcon", typeof(GameObject)) as GameObject);
            iconPrefabs.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Icons/MeleeIcon", typeof(GameObject)) as GameObject);
            iconPrefabs.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Icons/ForwardIcon", typeof(GameObject)) as GameObject);
        }
    }

    /*!
     * \brief Set text UIs according to given data.
     */
    public void SetUnitData(UnitData data, int newIndex)
    {
        index = newIndex;

        health.text = Mathf.Floor(data.health).ToString();
        speed.text = Mathf.Floor(data.speed).ToString();
        power.text = Mathf.Floor(data.power).ToString();

        if (baseTypeObject != null)
            Destroy(baseTypeObject);
        
        baseTypeObject = SpawnIcon(iconPrefabs[data.baseType], baseTypePos.position);
    }

    /*!
     * \brief Create type icon.
     */
    private GameObject SpawnIcon(GameObject prefab, Vector3 position)
    {
        return Object.Instantiate(prefab, position, Quaternion.identity);
    }

    /*!
     * \brief Getter method for index.
     */
    public int GetIndex()
    {
        return index;
    }

    /*!
     * \brief When mouse enter, change color.
     */
    void OnMouseEnter()
    {
        if (clicked == false)
            ChangeColor(hoveringColor);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // If battle is not going on.
            if (UnitManager.instance.onBattle == false)
                InputManager.instance.SubstituteUIPressed(gameObject);
        }
    }

    void OnMouseExit()
    {
        if (clicked == false)
            OriginalColor();
    }

    /*!
     * \brief Change color of boundary.
     */
    private void ChangeColor(Color color)
    {
        upBar.color = color;
        downBar.color = color;
        leftBar.color = color;
        rightBar.color = color;
    }

    /*!
     * \brief Handle clicking this UI.
     */
    public void ClickedColor()
    {
        ChangeColor(clickedColor);
        clicked = true;
    }

    /*!
     * \brief Handle this UI being released.
     */
    public void OriginalColor()
    {
        ChangeColor(originalColor);
        clicked = false;
    }
}

/*!*******************************************************************
\file         Inventory.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/12/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        Inventory
\brief		  Manages items that player has currently.
********************************************************************/
public class Inventory : MonoBehaviour
{
    // Singleton pattern.
    public static Inventory instance;
    private Inventory() { }

    public GameObject itemUIPrefab; //! Prefab of item UI.

    // Position to spawn next item UI.
    private float xPos; //! Constant value.
    public float yPos;
    public float lineSpacing; //! Distancing from previous UI.

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        xPos = gameObject.transform.position.x;
    }

    /*!
     * \brief Spawn new item UI and set next position to spawn.
     */
    public void NewItem(Item item)
    {
        ItemUI ui = Object.Instantiate(itemUIPrefab, new Vector3(xPos, yPos, 0.0f), Quaternion.identity, gameObject.transform).GetComponent<ItemUI>();
        ui.Initialize(item);

        // For next item.
        yPos -= lineSpacing;
    }
}

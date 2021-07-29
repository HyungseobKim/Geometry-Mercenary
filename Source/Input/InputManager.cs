/*!*******************************************************************
\file         InputManager.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/04/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        InputManager
\brief		  Manages all inputs being used for editor.
              When the player clicks something, checks that and send an
              appropriate message and information to state machine.
********************************************************************/
public class InputManager : MonoBehaviour
{
    // Singleton pattern.
    public static InputManager instance;
    private InputManager() { }

    public StateMachine stateMachine = new StateMachine(); //! State machine manages states.

    private TileManager tileManager;

    // Currently selected thigns by player.
    public GameObject selectedUnit = null;
    public GameObject selectedSubstitute = null;
    public ItemUI selectedItem = null;

    public GameObject fieldUnitUI; //! UI indicates the unit that player selected.

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.SetCurrentState(State_Default.instance);

        tileManager = TileManager.instance;

        fieldUnitUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();

        // If there is no input, do nothing.
        if (Input.GetMouseButtonDown(0) == false)
            return;

        // Get mouse position on grid.
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int cellPos = tileManager.tilemap.WorldToCell(worldPos);

        Cell cell;

        // If click was not on the grid, do nothing.
        if (tileManager.cells.TryGetValue(cellPos, out cell) == false)
            return;

        if (cell.owner)
        {
            if (cell.owner.GetComponent<Status>().enemy)
                stateMachine.HandleMessage(Message.EnemyClicked, cell.owner);
            else
                stateMachine.HandleMessage(Message.UnitClicked, cell.owner);
        }
        else
            stateMachine.HandleMessage(Message.EmptyCellClicked, cell);
    }

    /*!
     * \brief Each time battle starts, assign global state onBattle.
     */
    public void BattleStart()
    {
        stateMachine.SetGlobalState(GlobalState_OnBattle.instance);
        stateMachine.ChangeState(State_Default.instance, null);
    }

    /*!
     * \brief Each time battle ends, remove globals state.
     */
    public void BattleEnd()
    {
        stateMachine.SetGlobalState(null);
        stateMachine.ChangeState(State_Default.instance, null);
    }

    /*!
     * \brief Player has clicked a substitute.
     */
    public void SubstituteUIPressed(GameObject substituteUI)
    {
        stateMachine.HandleMessage(Message.SubstituteClicked, substituteUI);
    }

    /*!
     * \brief Player has clicked an item.
     */
    public void ItemUIPressed(ItemUI itemUI)
    {
        stateMachine.HandleMessage(Message.ItemClicked, itemUI);
    }

    /*!
     * \brief Player is trying to unequip item from tactics UI.
     */
    public void UnequipFromTactics(int upgradeNumber)
    {
        stateMachine.HandleMessage(Message.UnequipItem, upgradeNumber);
        AudioManager.instance.Play("Unequip");
    }
}

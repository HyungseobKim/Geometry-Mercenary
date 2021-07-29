/*!*******************************************************************
\file         State_EnemySelected.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/06/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        State_EnemySelected
\brief		  Defines editor behaviors when players have clicked an enemy unit.
********************************************************************/
public class State_EnemySelected : State
{
    // Singleton pattern.
    public static State_EnemySelected instance;
    private State_EnemySelected() { }

    private static EnemyManager enemyManager;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        enemyManager = EnemyManager.instance;
    }

    /*!
     * \brief Store clicked enemy and set UI.
     */
    public override void Enter(dynamic info)
    {
        // Store this unit as new selection.
        inputManager.selectedUnit = info;

        // Move field unit UI to unit.
        inputManager.fieldUnitUI.SetActive(true);
        inputManager.fieldUnitUI.transform.position = info.transform.position;

        // Turn on tactis UI.
        int index = info.GetComponent<Status>().indexOnMemberList;
        tacticsUIManager.Initialize(enemyManager.GetUnitData(index));
    }

    /*!
     * \brief Update position of selection UI and checking whether target is still valid.
     */
    public override void StateUpdate()
    {
        // Enemy unit died, so back to default state.
        if (inputManager.selectedUnit == null)
            inputManager.stateMachine.ChangeState(State_Default.instance, null);
        else // Make sure selection UI attached to selected unit.
            inputManager.fieldUnitUI.transform.position = inputManager.selectedUnit.transform.position;
    }

    /*!
     * \brief Clear everything including UI.
     */
    public override void Exit()
    {
        TacticsUIManager.instance.Inactivate();
        inputManager.selectedUnit = null;
        inputManager.fieldUnitUI.SetActive(false);
    }

    /*!
     * \brief Players cannot do anything on enemy, so works similar with default state.
     */
    public override bool HandleMessage(Message message, dynamic info)
    {
        switch (message)
        {
            case Message.UnitClicked: // info is clicked unit.

                // If it is goal unit, ignore this message.
                if (info.GetComponent<Goal>() != null)
                    return true;

                inputManager.stateMachine.ChangeState(State_UnitSelected.instance, info);
                return true;

            case Message.EnemyClicked: // info is clicked enemy.

                // If it is goal unit, ignore this message.
                if (info.GetComponent<Goal>() != null)
                    return true;

                // If they are same unit, back to default.
                if (inputManager.selectedUnit == info)
                    inputManager.stateMachine.ChangeState(State_Default.instance, info);
                else // If they are different unit, change focus to new unit.
                    inputManager.stateMachine.ChangeState(instance, info);

                return true;

            case Message.ItemClicked: // info is clicked item.
                inputManager.stateMachine.ChangeState(State_ItemSelected.instance, info);
                return true;

            case Message.UnequipItem: // info is upgrade number.
                return true;

            case Message.SubstituteClicked: // info is clicked substitute UI.
                inputManager.stateMachine.ChangeState(State_SubstituteSelected.instance, info);
                return true;

            case Message.EmptyCellClicked:
                return true; // Do nothing.
        }

        return false;
    }
}

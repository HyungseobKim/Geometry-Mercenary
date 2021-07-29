/*!*******************************************************************
\file         State_UnitSelected.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/04/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        State_UnitSelected
\brief		  Defines editor behaviors when players have clicked a player unit.
********************************************************************/
public class State_UnitSelected : State
{
    // Singleton pattern.
    public static State_UnitSelected instance;
    private State_UnitSelected() { }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    /*!
     * \brief Store clicked player unit and set UI.
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
        tacticsUIManager.Initialize(memberManager.GetUnitData(index));
    }

    /*!
     * \brief Update position of selection UI and checking whether target is still valid.
     */
    public override void StateUpdate()
    {
        // Unit died, so back to default state.
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
     * \brief Players can unequip item from unit, move unit, or swap with substitute.
     */
    public override bool HandleMessage(Message message, dynamic info)
    {
        switch (message)
        {
            case Message.UnitClicked: // info is clicked unit.

                // If it is goal unit, ignore this message.
                if (info.GetComponent<Goal>() != null)
                    return true;

                // It they are different units, swap their positions.
                if (inputManager.selectedUnit != info)
                {
                    Status newUnitStatus = info.GetComponent<Status>();
                    Status oldUnitStatus = inputManager.selectedUnit.GetComponent<Status>();

                    Vector3Int newPos = newUnitStatus.cellPos;

                    // Swap position.
                    memberManager.SwapPosition(newUnitStatus.indexOnMemberList, oldUnitStatus.indexOnMemberList);
                    info.GetComponent<UnitBase>().ForceToMoveTo(oldUnitStatus.cellPos);
                    inputManager.selectedUnit.GetComponent<UnitBase>().ForceToMoveTo(newPos);
                }
                //else same unit = deselect.

                // Back to the default state.
                inputManager.stateMachine.ChangeState(State_Default.instance, info);
                return true;

            case Message.EnemyClicked: // info is clicked enemy.

                // If it is goal unit, ignore this message.
                if (info.GetComponent<Goal>() != null)
                    return true;

                inputManager.stateMachine.ChangeState(State_EnemySelected.instance, info);
                return true;

            case Message.ItemClicked: // info is clicked item.
                inputManager.stateMachine.ChangeState(State_ItemSelected.instance, info);
                return true;

            case Message.UnequipItem: // info is upgrade number.
                UnitData unitData = memberManager.GetUnitData(inputManager.selectedUnit.GetComponent<Status>().indexOnMemberList);

                if (info == 1)
                    memberManager.UnequipFirstUpgrade(inputManager.selectedUnit);
                else
                    memberManager.UnequipSecondUpgrade(inputManager.selectedUnit);

                // Back to the default state.
                inputManager.stateMachine.ChangeState(State_Default.instance, info);
                return true;

            case Message.SubstituteClicked: // info is member unit UI.
                memberManager.MemberToField(info.GetComponent<MemberUnitUI>().GetIndex(), inputManager.selectedUnit);

                // Back to the default state.
                inputManager.stateMachine.ChangeState(State_Default.instance, info);
                return true;

            case Message.EmptyCellClicked: // info is cell

                // Cannot move above 0.
                if (info.cellPos.y >= 0)
                    return true;

                // Tell member manager that this unit moves.
                memberManager.MoveUnitTo(inputManager.selectedUnit.GetComponent<Status>().indexOnMemberList, info.worldPos);

                // Move this unit to empty cell.
                inputManager.selectedUnit.GetComponent<UnitBase>().ForceToMoveTo(info.cellPos);

                // Back to the default state.
                inputManager.stateMachine.ChangeState(State_Default.instance, info);
                return true;
        }

        return false;
    }
}

/*!*******************************************************************
\file         State_ItemSelected.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/04/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        State_ItemSelected
\brief		  Defines editor behaviors when players have clicked an item.
********************************************************************/
public class State_ItemSelected : State
{
    // Singleton pattern.
    public static State_ItemSelected instance;
    private State_ItemSelected() { }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    /*!
     * \brief Store clicked item.
     */
    public override void Enter(dynamic info)
    {
        inputManager.selectedItem = info;
    }

    /*!
     * \brief Clear everything.
     */
    public override void Exit()
    {
        inputManager.selectedItem.Deselect();
        inputManager.selectedItem = null;
    }

    /*!
     * \brief If the player clicks a player unit, try to equip.
     *        Otherwise, nothing special.
     */
    public override bool HandleMessage(Message message, dynamic info)
    {
        switch (message)
        {
            case Message.UnitClicked: // info is clicked unit.

                // If it is goal unit, ignore this message.
                if (info.GetComponent<Goal>() != null)
                    return true;

                // try to equip item.
                GameObject newObject = memberManager.EquipItem(info, inputManager.selectedItem.GetItem());

                if (newObject)
                {
                    // It succeeds.
                    inputManager.selectedItem.Equip(newObject);

                    // Play sound.
                    AudioManager.instance.Play("Equip");

                    // Back to the default state.
                    inputManager.stateMachine.ChangeState(State_Default.instance, info);
                    return true;
                }

                // Stay on this state.
                return true;

            case Message.EnemyClicked: // info is clicked enemy.

                // If it is goal unit, ignore this message.
                if (info.GetComponent<Goal>() != null)
                    return true;

                inputManager.stateMachine.ChangeState(State_EnemySelected.instance, info);
                return true;

            case Message.ItemClicked: // info is clicked item.

                // Same item.
                if (inputManager.selectedItem == info) // Back to the default state.
                    inputManager.stateMachine.ChangeState(State_Default.instance, info);
                else // Stay this state, but with different item.
                    inputManager.stateMachine.ChangeState(instance, info);

                return true;

            case Message.SubstituteClicked: // info is member unit UI.
                inputManager.stateMachine.ChangeState(State_SubstituteSelected.instance, info);
                return true;

            case Message.EmptyCellClicked: // Back to the default state.
                inputManager.stateMachine.ChangeState(State_Default.instance, info);
                return true;
        }

        return false;
    }
}

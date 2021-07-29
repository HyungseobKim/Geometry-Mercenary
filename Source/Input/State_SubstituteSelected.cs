/*!*******************************************************************
\file         State_SubstituteSelected.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/04/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        State_SubstituteSelected
\brief		  Defines editor behaviors when players have clicked a substitute.
********************************************************************/
public class State_SubstituteSelected : State
{
    // Singleton pattern.
    public static State_SubstituteSelected instance;
    private State_SubstituteSelected() { }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    /*!
     * \brief Store clicked substitute and set UI.
     */
    public override void Enter(dynamic info)
    {
        // Store this unit as new selection.
        inputManager.selectedSubstitute = info;

        // Change color of UI.
        info.GetComponent<MemberUnitUI>().ClickedColor();
    }

    /*!
     * \brief Clear everything including UI.
     */
    public override void Exit()
    {
        inputManager.selectedSubstitute.GetComponent<MemberUnitUI>().OriginalColor();
        inputManager.selectedSubstitute = null;
    }

    /*!
     * \brief If the player clicks a player unit, try to swap them.
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

                memberManager.MemberToField(inputManager.selectedSubstitute.GetComponent<MemberUnitUI>().GetIndex(), info);

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

            case Message.SubstituteClicked: // info is clicked substitute UI.

                // Same unit = deselect.
                if (inputManager.selectedSubstitute == info)
                {
                    // Back to the default state.
                    inputManager.stateMachine.ChangeState(State_Default.instance, info);
                    return true;
                }

                // Stay this state, but with different selection.
                inputManager.stateMachine.ChangeState(instance, info);
                return true;

            case Message.EmptyCellClicked: // Back to the default state.
                inputManager.stateMachine.ChangeState(State_Default.instance, info);
                return true;
        }

        return false;
    }
}

/*!*******************************************************************
\file         GlobalState_OnBattle.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/05/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        GlobalState_OnBattle
\brief		  Defines editor behaviors that must be changed when battle is on going.
********************************************************************/
public class GlobalState_OnBattle : State
{
    // Singleton pattern.
    public static GlobalState_OnBattle instance;
    private GlobalState_OnBattle() { }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    /*!
     * \brief When battle is on going, players can only access to unit information.
     *        Ignore all other messages.
     */
    public override bool HandleMessage(Message message, dynamic info)
    {
        switch (message)
        {
            case Message.UnitClicked:

                // If it is goal unit, ignore this message.
                if (info.GetComponent<Goal>() != null)
                    return true;

                // If they are same unit, back to default.
                if (inputManager.selectedUnit == info)
                    inputManager.stateMachine.ChangeState(State_Default.instance, info);
                else // If they are different unit, change focus to new unit.
                    inputManager.stateMachine.ChangeState(State_UnitSelected.instance, info);

                return true;

            case Message.ItemClicked: // info is clicked item.
                info.Deselect(); // Deselect item immediately.
                return true;

            case Message.UnequipItem: // Ignore. Players cannot unequip item during battle.
                return true;

            case Message.SubstituteClicked: // info is clicked substitute UI.
                info.GetComponent<MemberUnitUI>().OriginalColor(); // Back to original color immediately.
                return true;

            case Message.EmptyCellClicked: // Back to default state.
                inputManager.stateMachine.ChangeState(State_Default.instance, info);
                return true;
        }

        return false;
    }
}

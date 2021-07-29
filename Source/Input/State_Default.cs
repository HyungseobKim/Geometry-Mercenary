/*!*******************************************************************
\file         State_Default.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/04/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        State_Default
\brief		  Defines editor behaviors when nothing has been clicked yet.
********************************************************************/
public class State_Default : State
{
    // Singleton pattern.
    public static State_Default instance;
    private State_Default() { }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    /*!
     * \brief If any of event occurs, changes state appropriate to the event.
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

                inputManager.stateMachine.ChangeState(State_EnemySelected.instance, info);
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

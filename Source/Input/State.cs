/*!*******************************************************************
\file         State.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/04/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        State
\brief		  Base state class that provides interface of input states.
********************************************************************/
public class State : MonoBehaviour
{
    // Reference to managers will be used frequently.
    protected static InputManager inputManager;
    protected static TacticsUIManager tacticsUIManager;
    protected static MemberManager memberManager;
    protected static TileManager tileManager;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (inputManager == null)
            inputManager = InputManager.instance;

        if (tacticsUIManager == null)
            tacticsUIManager = TacticsUIManager.instance;

        if (memberManager == null)
            memberManager = MemberManager.instance;

        if (tileManager == null)
            tileManager = TileManager.instance;
    }

    /*!
     * \brief Enter is called when this state becomes current state.
     */
    public virtual void Enter(dynamic info) { }

    /*!
     * \brief StateUpdate is called once per frame when this state is current state.
     */
    public virtual void StateUpdate() { }

    /*!
     * \brief Exit is called when this state is replaced from current state.
     */
    public virtual void Exit() { }

    /*!
     * \brief When events occurs, handle that event and return result.
     * 
     * \param message
     *        Type of event occured.
     *        
     * \param info
     *        Information related with the message.
     *        Strictly specified for each message.
     *        
     * \return bool
     *         If message handled properly, return true.
     *         If not, return false.
     */
    public virtual bool HandleMessage(Message message, dynamic info)
    {
        return false;
    }
}

/*!*******************************************************************
\enum         Message

\brief		  List of every situation can happen for editor input.

\var          Message::UnitClicked
              Occur when the player clicks a player unit on the field.

\var          Message::EnemyClicked
              Occur when the player clicks an enemy unit on filed.

\var          Message::ItemClicked
              Occur when the player clicks an item on the inventory.

\var          Message::UnequipItem
              Occur when the player clicks upgrade ability description to unequip that item.
              Only happens in State_UnitSelected, because tactics UI is visible only in that state.

\var          Message::SubstituteClicked
              Occur when the player clicks a substitute on substitutes list.

\var          Message::EmptyCellClicked
              Occur when the player clicks an empty cell.

\var          Message::Default
              Default value.
********************************************************************/
public enum Message
{
    UnitClicked,
    EnemyClicked,
    ItemClicked,
    UnequipItem,
    SubstituteClicked,
    EmptyCellClicked,
    Default
}
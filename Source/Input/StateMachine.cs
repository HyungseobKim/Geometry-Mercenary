/*!*******************************************************************
\file         StateMachine.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/04/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/

public class StateMachine
{
    // Reference to states.
    private State currentState = null;
    private State previousState = null;
    private State globalState = null;

    /*!
     * \brief Setter method for current state.
     */
    public void SetCurrentState(State state)
    {
        currentState = state;
    }

    /*!
     * \brief Setter method for previous state.
     */
    public void SetPreviousState(State state)
    {
        previousState = state;
    }

    /*!
     * \brief Setter method for global state.
     */
    public void SetGlobalState(State state)
    {
        globalState = state;
    }

    /*!
     * \brief Getter method for current state.
     */
    public State GetCurrentState()
    {
        return currentState;
    }

    /*!
     * \brief Getter method for previous state.
     */
    public State GetPreviousState()
    {
        return previousState;
    }

    /*!
     * \brief Getter method for global state.
     */
    public State GetGlobalState()
    {
        return globalState;
    }

    /*!
     * \brief Update all states.
     */
    public void Update()
    {
        if (globalState != null)
            globalState.StateUpdate();

        if (currentState != null)
            currentState.StateUpdate();
    }

    /*!
     * \brief Send message. If global state failed to handle, try with current state.
     */
    public bool HandleMessage(Message message, dynamic info)
    {
        if (globalState != null && globalState.HandleMessage(message, info))
            return true;
        
        if (currentState != null && currentState.HandleMessage(message, info))
            return true;

        return false;
    }

    /*!
     * \brief Change state and do process for that.
     */
    public void ChangeState(State newState, dynamic info)
    {
        // Store current state.
        previousState = currentState;

        // Exit current state.
        currentState.Exit();

        // Set new state.
        currentState = newState;

        // Enter new state.
        currentState.Enter(info);
    }

    /*!
     * \brief Helper function to check current state.
     */
    public bool IsInState(State state)
    {
        return currentState == state;
    }
}

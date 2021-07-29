/*!*******************************************************************
\file         GameSpeedController.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/09/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        GameSpeedController
\brief        Storing game speed.
********************************************************************/
public class GameSpeedController : MonoBehaviour
{
    // Singleton pattern
    public static GameSpeedController instance;
    private GameSpeedController() { }

    public float speed; //! Current speed of the game.
    public GameSpeedOption selectedOption = null; //! Currently selected speed.

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        speed = 1.0f;
    }

    /*!
     * \brief Change speed of the game, and releases previously selected button.
     * 
     * \param newOption
     *        Button clicked newly.
     */
    public void NewSelection(GameSpeedOption newOption)
    {
        selectedOption.Deselect();
        selectedOption = newOption;

        speed = newOption.speed;
    }
}

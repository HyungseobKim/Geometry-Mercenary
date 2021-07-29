/*!*******************************************************************
\file         PauseMenuContinueButton.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/13/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        PauseMenuContinueButton
\brief		  When player clicks this button, closes the pause menu.
********************************************************************/
public class PauseMenuContinueButton : MainMenuButton
{
    public GameObject pauseMenu; //! Pause menu object to close.

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PauseMenu.instance.ExitPauseMenu();
            OnMouseExit();
            pauseMenu.SetActive(false);
        }
    }
}

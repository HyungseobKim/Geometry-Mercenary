/*!*******************************************************************
\file         PauseMenuQuitButton.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/13/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

/*!*******************************************************************
\class        CreditsButton
\brief		  When player clicks this button, back to the main scene.
********************************************************************/
public class PauseMenuQuitButton : MainMenuButton
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Close pause menu.
            PauseMenu.instance.ExitPauseMenu();

            // Load main scene.
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);

            // Do behavior that this button does when mouse exit this button.
            OnMouseExit();
            return;
        }
    }
}

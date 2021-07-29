/*!*******************************************************************
\file         QuitButton.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/11/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        QuitButton
\brief		  When player clicks this button, quit the game.
********************************************************************/
public class QuitButton : MainMenuButton
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Application.Quit();
        }
    }
}

/*!*******************************************************************
\file         CreditsButton.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/13/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

/*!*******************************************************************
\class        CreditsButton
\brief		  When player clicks this button, open credits scene.
********************************************************************/
public class CreditsButton : MainMenuButton
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("CreditsScene", LoadSceneMode.Single);
        }
    }
}

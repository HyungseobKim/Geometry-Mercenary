/*!*******************************************************************
\file         PlayButton.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/11/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        PlayButton
\brief		  When player clicks this button, call fade out function.
********************************************************************/
public class PlayButton : MainMenuButton
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.instance.Play("Explosion");
            SceneFadeOut.instance.FadeOutStart();
            OnMouseExit();
        }
    }
}

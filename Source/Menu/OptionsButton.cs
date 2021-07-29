/*!*******************************************************************
\file         OptionsButton.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/11/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        OptionsButton
\brief		  When player clicks this button, open options menu.
********************************************************************/
public class OptionsButton : MainMenuButton
{
    private static GameObject options; //! Options menu object.

    void Start()
    {
        GameObject canvas = GameObject.Find("OptionsCanvas");
        canvas.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        options = canvas.transform.Find("Options").gameObject;
        options.SetActive(false);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseExit();
            options.SetActive(true);
        }
    }
}

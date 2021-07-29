/*!*******************************************************************
\file         MainMenuButton.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/09/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        MainMenuButton
\brief		  Base class for main menu button classes.
              Helps to manage bars for polishing UI.
********************************************************************/
public class MainMenuButton : MonoBehaviour
{
    public GameObject leftBar;
    public GameObject rightBar;

    // Start is called before the first frame update
    void Start()
    {
        leftBar.SetActive(false);
        rightBar.SetActive(false);
    }

    void OnMouseEnter()
    {
        leftBar.SetActive(true);
        rightBar.SetActive(true);
    }

    protected void OnMouseExit()
    {
        leftBar.SetActive(false);
        rightBar.SetActive(false);
    }
}

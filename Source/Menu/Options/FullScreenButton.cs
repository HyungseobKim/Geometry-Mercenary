/*!*******************************************************************
\file         FullScreenButton.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/12/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        FullScreenButton
\brief		  When player clicks this button, change full screen to
              windowed, or vice versa.
********************************************************************/
public class FullScreenButton : MonoBehaviour
{
    public FullScreenButton otherButton; //! Windowed button.
    public GameObject selection; //! UI indicates that this button is currently selected.
    public bool fullScreen; //! Indicates whether this button is making full screen or not.
    private bool selected; //! Stores whether this button is currently selected or not.

    private static float randomRange = 0.05f; //! Range of random which is being used for UI.
    private Vector3 position; //! Original position of button.

    // Start is called before the first frame update
    void Start()
    {
        // Store original position.
        position = gameObject.transform.position;

        // Since game starts as windowed as default, so if this button is windowed button, turn on the UI.
        selected = (fullScreen == Screen.fullScreen);

        if (selected == false)
            selection.SetActive(false);
    }

    void OnMouseOver()
    {
        // If it is already selected, don't do anything.
        if (selected)
            return;

        Vector3 newPos = position;

        // Shaking effect for user.
        newPos.x += Random.Range(-randomRange, randomRange);
        newPos.y += Random.Range(-randomRange, randomRange);

        gameObject.transform.position = newPos;

        if (Input.GetMouseButtonDown(0))
        {
            // Change.
            Screen.fullScreen = !Screen.fullScreen;

            selected = true;
            gameObject.transform.position = position;

            selection.SetActive(true);

            // Tell the other button that this button is now selected.
            otherButton.Deselect();
        }
    }

    void OnMouseExit()
    {
        // Back to the original position.
        gameObject.transform.position = position;
    }

    /*!
     * \brief When the other button is selected, deselect this button.
     */
    public void Deselect()
    {
        selected = false;
        selection.SetActive(false);
    }
}

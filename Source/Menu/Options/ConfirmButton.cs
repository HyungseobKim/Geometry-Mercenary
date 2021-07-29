/*!*******************************************************************
\file         ConfirmButton.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/12/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        ConfirmButton
\brief		  When player clicks this button, closes the options menu.
********************************************************************/
public class ConfirmButton : MonoBehaviour
{
    public GameObject options; //! Options menu to close.

    private static float randomRange = 0.05f; //! Range of random which is being used for UI.
    private Vector3 position; //! Original position of button.

    // Start is called before the first frame update
    void Start()
    {
        // Store original position.
        position = gameObject.transform.position;
    }

    void OnMouseOver()
    {
        Vector3 newPos = position;

        // Shaking effect for user.
        newPos.x += Random.Range(-randomRange, randomRange);
        newPos.y += Random.Range(-randomRange, randomRange);

        gameObject.transform.position = newPos;

        // If it clicked, closes.
        if (Input.GetMouseButtonDown(0))
        {
            options.SetActive(false);
        }
    }

    void OnMouseExit()
    {
        // Back to the original position.
        gameObject.transform.position = position;
    }
}

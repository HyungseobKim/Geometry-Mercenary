/*!*******************************************************************
\file         GameSpeedOption.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/09/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        GameSpeedOption
\brief        Each option of game speed. x1, x2, x3.
********************************************************************/
public class GameSpeedOption : MonoBehaviour
{
    public int speed; //! The speed that this button represents.

    public Text text; //! Text of this button.

    public GameObject hoveringBackground; //! Object for use feedback when a mouse is hovering on this button.
    public GameObject selection; //! Object for user feedback when this button has clicked.

    // Start is called before the first frame update
    void Start()
    {
        hoveringBackground.SetActive(false);

        text.text = "X" + speed;
    }

    void OnMouseEnter()
    {
        hoveringBackground.SetActive(true);
    }

    void OnMouseExit()
    {
        hoveringBackground.SetActive(false);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameSpeedController.instance.NewSelection(this);
            selection.SetActive(true);

            // Set sound speed too.
            AudioManager.instance.SetPitch("Footstep", (float)speed);
        }
    }

    /*!
     * \brief Other button selected.
     */
    public void Deselect()
    {
        selection.SetActive(false);
    }
}

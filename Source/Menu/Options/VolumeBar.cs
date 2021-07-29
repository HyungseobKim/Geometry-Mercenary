/*!*******************************************************************
\file         VolumeBar.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/16/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        VolumeBar
\brief		  When player clicks this bar, calls volume changing function.
********************************************************************/
public class VolumeBar : MonoBehaviour
{
    [HideInInspector]
    public int volume; //! Volume of this bar.

    [HideInInspector]
    public VolumeCategory volumeCategory; //! The category that this bar is belonging to.

    private SpriteRenderer spriteRenderer; //! Sprite renderer of this object.

    private Color originalColor; //! Default color.
    private Color activatedColor = new Color(1.0f, 1.0f, 1.0f, 1.0f); //! Color when this bar is activated.

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            volumeCategory.SetVolume(volume);
        }
    }

    /*!
     * \brief When user clicks a bar that indicates higher volume than this bar, it should be activated together.
     */
    public void Activate()
    {
        spriteRenderer.color = activatedColor;
    }

    /*!
     * \brief When user clicks a bar that indicates lower volume than this bar, it should be deactivated.
     */
    public void Deactivate()
    {
        spriteRenderer.color = originalColor;
    }
}

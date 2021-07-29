/*!*******************************************************************
\file         ConstantColorChanger.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/29/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

/*!*******************************************************************
\class        ConstantColorChanger
\brief		  Changing color of attached object continuously.
********************************************************************/
public class ConstantColorChanger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; //! Sprite to change color.
    private Color color; //! Color object not to keep allocate.

    public float speed; //! Speed of changing color.

    // The only hard coded variables.
    private float[] rgb = new float[] { 1.0f, 0.0f, 0.0f }; //! Current color.
    private int index = 2; //! Current color channel to update. Since color starts from red, it sould be started to change blue.
    
    private float sign = 1.0f; //! Decide whether increasing or decreasing value.
    private bool next = false; //! Indicates whether algorithm have to move on next index.

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        color = new Color(rgb[0], rgb[1], rgb[2], 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Update current color channel.
        rgb[index] += sign * speed * Time.deltaTime;

        // If value goes over 1 or less than 0,
        if (rgb[index] > 1.0f)
        {
            // Clamp and mark that we need to move on next.
            rgb[index] = 1.0f;
            next = true;
        }
        else if (rgb[index] < 0.0f)
        {
            // Clamp and mark that we need to move on next.
            rgb[index] = 0.0f;
            next = true;
        }

        // Set color.
        color.r = rgb[0];
        color.g = rgb[1];
        color.b = rgb[2];

        spriteRenderer.color = color;

        // If values reached to 1 or 0,
        if (next == true)
        {
            // reverse sign and update next color channel.
            index = (index + 1) % 3;
            sign *= -1.0f;

            next = false;
        }
    }
}

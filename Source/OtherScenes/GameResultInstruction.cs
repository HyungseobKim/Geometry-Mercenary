/*!*******************************************************************
\file         GameResultInstruction.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/13/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

/*!*******************************************************************
\class        GameResultInstruction
\brief		  Pulsing effect for instruction UI.
********************************************************************/
public class GameResultInstruction : MonoBehaviour
{
    private Text text; //! Text to component to pulse.
    private Color color; //! Current color.

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();
        color = text.color;
    }

    // Update is called once per frame
    void Update()
    {
        color.a = 0.55f + (Mathf.Sin(Time.time) * 0.45f);
        text.color = color;
    }
}

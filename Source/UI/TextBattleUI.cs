/*!*******************************************************************
\file         TextBattleUI.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/27/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

/*!*******************************************************************
\class        TextBattleUI
\brief        UIs for during battle.
              It is floating up, and will be disappeared after a few seconds.
********************************************************************/
public class TextBattleUI : MonoBehaviour
{
    public float TimeToLive = 1.0f; //! Life time.
    public float floatingSpeed = 1.0f; //! Speed of floating.

    private float Timer = 0.0f; //! Elapsed time.

    // Start is called before the first frame update
    void Start()
    {
        Timer = TimeToLive;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer < 0.0f)
        {
            Destroy(gameObject);
            return;
        }
        
        // Fading text
        Color fadingColor = GetComponent<Text>().color;
        fadingColor.a = Timer / TimeToLive;
        GetComponent<Text>().color = fadingColor;

        // Floating text
        var pos = gameObject.transform.position;
        pos.y += (floatingSpeed * Time.deltaTime);
        gameObject.transform.position = pos;
    }
}

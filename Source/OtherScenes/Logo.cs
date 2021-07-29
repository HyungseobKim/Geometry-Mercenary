/*!*******************************************************************
\file         Logo.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/13/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

/*!*******************************************************************
\class        Logo
\brief		  Show DigiPen logo for 2 seconds and move to the main scene.
********************************************************************/
public class Logo : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Wait two seconds.
        if (Time.time > 2.0f)
        {
            // Load main scene.
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
            return;
        }
    }
}

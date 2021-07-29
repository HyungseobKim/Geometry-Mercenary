/*!*******************************************************************
\file         Credits.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/13/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

/*!*******************************************************************
\class        Credits
\brief		  Scroll credits scene.
********************************************************************/
public class Credits : MonoBehaviour
{
    private Vector3 position; //! Current position.
    private float timer = 0.0f; //! Time to quit automatically.

    // Start is called before the first frame update
    void Start()
    {
        position = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // If there is any input from player or enough time passed,
        if (Input.anyKey || timer > 45.0f)
        {
            // back to the main scene.
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
            return;
        }

        // Scroll up.
        position.y += Time.deltaTime;
        gameObject.transform.position = position;
    }
}

/*!*******************************************************************
\file         GameResultUI.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/13/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

/*!*******************************************************************
\class        GameResultUI
\brief		  Rotates and scale game result UI from near zero to
              given size.
********************************************************************/
public class GameResultUI : MonoBehaviour
{
    public float scaleTime; //! Time to complete scaling.
    private int numberOfRotation; //! How many times to rotate unitl it completes scaling.

    private Vector3 scale = new Vector3(0.0f, 0.0f, 1.0f); //! Current scale.
    private float z = 0.0f; //! Since this is 2D game, rotation affects to only z component.

    public GameObject instruction; //! Instruction object.
    private float timer = 0.0f; //! Elapsed time since this object awaken.

    // Start is called before the first frame update
    void Start()
    {
        // Set random amount of rotation at the begin.
        numberOfRotation = Random.Range(1, 10);

        instruction.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If there was any kinds of input, skip and move to credits scene.
        if (Input.anyKey)
        {
            SceneManager.LoadScene("CreditsScene", LoadSceneMode.Single);
            return;
        }

        // Wait one second after result UI is fully scaled.
        if (timer < scaleTime + 1.0f)
        {
            timer += Time.deltaTime;

            if (timer >= scaleTime + 1.0f)
            {
                instruction.SetActive(true);
            }
        }

        // It is fully scaled.
        if (scale.x >= 1.0f)
            return;

        // Scale.
        scale.x += (Time.deltaTime / scaleTime);
        scale.y += (Time.deltaTime / scaleTime);
        gameObject.transform.localScale = scale;

        // Rotate.
        z += Time.deltaTime * (numberOfRotation * 360.0f / scaleTime);
        gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, z);

        if (scale.x >= 1.0f)
        {
            gameObject.transform.rotation = Quaternion.identity;
        }
    }
}

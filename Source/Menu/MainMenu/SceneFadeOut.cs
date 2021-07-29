/*!*******************************************************************
\file         StartButtonUI.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/01/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]

/*!*******************************************************************
\class        SceneFadeOut
\brief		  When fade out function called, starts fade out the scene.
              After fade out finished, enter the game scene.
********************************************************************/
public class SceneFadeOut : MonoBehaviour
{
    // Singleton pattern
    public static SceneFadeOut instance;
    private SceneFadeOut() { }

    public string sceneName;
    private bool fadeOutStart = false;

    private SpriteRenderer blackImage;
    private Color color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

    public float fadeoutTime;
    private float timer = 0.0f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        blackImage = gameObject.GetComponent<SpriteRenderer>();
        blackImage.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOutStart == false)
            return;

        timer += Time.deltaTime;

        if (timer >= fadeoutTime)
        {
            // Reset values for next time.
            fadeOutStart = false;
            
            // Change scene.
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            return;
        }

        // Change color.
        color.a = timer / fadeoutTime;
        blackImage.color = color;
    }

    /*!
     * \brief Start fade out.
     */
    public void FadeOutStart()
    {
        fadeOutStart = true;
        timer = 0.0f;
    }
}

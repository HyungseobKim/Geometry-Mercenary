/*!*******************************************************************
\file         PauseMenu.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/13/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        PauseMenu
\brief		  When player press ESC key, opens or closes the pause menu.
********************************************************************/
public class PauseMenu : MonoBehaviour
{
    // Singleton pattern
    public static PauseMenu instance;
    private PauseMenu() { }

    public GameObject pauseMenu; //! Pause menu object.
    private bool onPauseMenu; //! Indicates whether pause menu is opened or not.

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
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onPauseMenu = !onPauseMenu;
            pauseMenu.SetActive(onPauseMenu);
        }
    }

    public void ExitPauseMenu()
    {
        onPauseMenu = false;
    }
}

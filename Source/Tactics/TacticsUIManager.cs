/*!*******************************************************************
\file         TacticsUIManager.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/14/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!*******************************************************************
\class        TacticsUIManager
\brief		  Turn on UIs and call initialization function.
********************************************************************/
public class TacticsUIManager : MonoBehaviour
{
    // Singleton pattern
    public static TacticsUIManager instance;
    private TacticsUIManager() { }

    public GameObject tacticsUI; //! Actual tactis UI object manages tactics options.

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
        tacticsUI.SetActive(false);
    }

    /*!
     * \brief Initialize tactisc option UI.
     * 
     * \param data
     *        Data of unit to display.
     */
    public void Initialize(UnitData data)
    {
        tacticsUI.SetActive(true);
        tacticsUI.GetComponent<TacticsUI>().Initialize(data);
    }

    /*!
     * \brief Turn off the UI.
     */
    public void Inactivate()
    {
        tacticsUI.SetActive(false);
    }
}

/*!*******************************************************************
\file         UnitInfo.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/29/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.UI;

/*!*******************************************************************
\class        UnitInfo
\brief        Showing simple information about unit when player hovers mouse on.
********************************************************************/
public class UnitInfo : MonoBehaviour
{
    public Text healthText; //! Text for health.
    public Text powerText; //! Text for power.
    public Text speedText; //! Text for speed.
    public Text protectionText; //! Text for protection.

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    /*!
     * \brief Set all texts and turn on the UI.
     * 
     * \param unit
     *        Data of the unit to display.
     */
    public void Initialize(GameObject unit)
    {
        Status status = unit.GetComponent<Status>();

        healthText.text = Mathf.Floor(status.health) + "/" + Mathf.Floor(status.maxHealth);
        powerText.text = Mathf.Floor(status.power).ToString();
        speedText.text = Mathf.Floor(status.speed).ToString();
        protectionText.text = Mathf.Floor(status.protection).ToString();
        
        gameObject.SetActive(true);
    }
}

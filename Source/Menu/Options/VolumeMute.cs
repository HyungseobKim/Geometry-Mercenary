/*!*******************************************************************
\file         VolumeMute.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/18/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

/*!*******************************************************************
\class        VolumeMute
\brief		  Mute button which mute or unmute volume category that
              it is belonging to.
********************************************************************/
public class VolumeMute : MonoBehaviour
{
    public VolumeCategory volumeCategory; //! The category that this bar is belonging to.

    public GameObject muteIcon; //! UI indicates state that this category is muted.
    private bool mute = false; //! Indicates whether this category is muted or not.

    // Start is called before the first frame update
    void Start()
    {
        volumeCategory.volumeMute = this;
        muteIcon.SetActive(false);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mute = !mute;
            volumeCategory.SetMute(mute);
            muteIcon.SetActive(mute);
        }
    }

    /*!
     * \brief When player clicks volume bar, it this category should be unmuted.
     */
    public void Unmute()
    {
        mute = false;
        volumeCategory.SetMute(false);
        muteIcon.SetActive(false);
    }
}

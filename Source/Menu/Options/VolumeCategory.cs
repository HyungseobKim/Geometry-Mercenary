/*!*******************************************************************
\file         VolumeCategory.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/16/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!*******************************************************************
\class        VolumeCategory
\brief		  Base class for each category of volume.
              When players clicks it's one of volume bars, call
              change volume behavior function.
              If player clicks mute button, call mute behavior function.
********************************************************************/
public abstract class VolumeCategory : MonoBehaviour
{
    [HideInInspector]
    public VolumeMute volumeMute; //! Reference to mute object.
    public List<VolumeBar> volumeBars; //! List of all volume bars belonging to this category.

    private int currentVolume = 10; //! Current volume. Actual volume value is one of ten.

    // Start is called before the first frame update
    void Start()
    {
        // Initialize all volume bars.
        for (int i = 0; i < volumeBars.Count; ++i)
        {
            volumeBars[i].volume = i + 1;
            volumeBars[i].volumeCategory = this;
            volumeBars[i].Activate();
        }
    }

    /*!
     * \brief When player clicks volume bar, this function would be called.
     *        Activate or deactivate volume bars depending on new volume,
     *        and calls appropriate change volume method.
     */
    public void SetVolume(int volume)
    {
        // If it was muted, activate.
        volumeMute.Unmute();

        if (volume < currentVolume)
        {
            // Turn off all volumes between them.
            for (int i = volume; i < currentVolume; ++i)
                volumeBars[i].Deactivate();
        }
        else if (volume > currentVolume)
        {
            // Turn on all volumes between them.
            for (int i = currentVolume; i < volume; ++i)
                volumeBars[i].Activate();
        }
        else // Same volume.
            return; // Do nothing.

        // Apply new volume.
        CallSetVolumeMethod(volume / 10.0f);
        currentVolume = volume;
    }

    /*!
     * \brief When player clicks mute button, this function would be called.
     *        Call appropriate mute volume method.
     */
    public void SetMute(bool mute)
    {
        CallMuteMethod(mute);
    }

    /*!
     * \brief Call set volume method on AudioManager.
     *        Must be defined on child class depends on the category.
     */
    protected abstract void CallSetVolumeMethod(float volume);
    /*!
     * \brief Call mute method on AudioManager.
     *        Must be defined on child class depends on the category.
     */
    protected abstract void CallMuteMethod(bool mute);
}

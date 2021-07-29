/*!*******************************************************************
\file         VolumeCategoryMusic.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/17/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        VolumeCategoryMusic
\brief		  Defines volume category class about music.
********************************************************************/
public class VolumeCategoryMusic : VolumeCategory
{
    protected override void CallSetVolumeMethod(float volume)
    {
        AudioManager.instance.SetMusicVolume(volume);
    }

    protected override void CallMuteMethod(bool mute)
    {
        AudioManager.instance.SetMusicMute(mute);
    }
}

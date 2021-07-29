/*!*******************************************************************
\file         BarScaler.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         05/10/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        BarScaler
\brief        Manages scale of bars like health bar, cooldown bar.
********************************************************************/
public class BarScaler : MonoBehaviour
{
    private Vector3 OriginalScale; //! Maximum scale.
    private Vector3 OriginalPos; //! Starting position.

    private float TargetScale; //! Temporal desire scale.
    private float CurrentScale; //! Scale that player can see now.

    private float InterpolationTime = 0.0f; //! Remained time to reach TargetScale.
    private float ScaleTime; //! Timer.

    // Start is called before the first frame update
    void Start()
    {
        OriginalScale = gameObject.transform.localScale;
        OriginalPos = gameObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (InterpolationTime > 0.0f)
        {
            InterpolationTime = Mathf.Clamp(InterpolationTime - Time.deltaTime, 0.0f, ScaleTime);

            float newScale = TargetScale + (CurrentScale - TargetScale) * (InterpolationTime / ScaleTime);
            SetScale(newScale);
        }
    }

    /*!
     * \brief Set given scale to actual object.
     * 
     * \param newScale
     *        New scale to apply.
     */
    private void SetScale(float newScale)
    {
        gameObject.transform.localScale = new Vector3(newScale, OriginalScale.y, OriginalScale.z);

        //float positionAdjustment = (OriginalScale.x - newScale) / 2.0f;
        //gameObject.transform.localPosition = new Vector3(OriginalPos.x - positionAdjustment, OriginalPos.y, OriginalPos.z);
    }

    /*!
     * \brief Compute new target scale and set timer.
     * 
     * \param percent
     *        New scale.
     *        
     * \param time
     *        Time should be taken to scale.
     */
    public void InterpolateToScale(float percent, float time)
    {
        CurrentScale = gameObject.transform.localScale.x;
        TargetScale = percent * OriginalScale.x;

        ScaleTime = time + 0.001f;
        InterpolationTime = ScaleTime;
    }

    /*!
     * \brief Change scale immediately.
     * 
     * \param percent
     *        New scale.
     */
    public void InterpolateImmediate(float percent)
    {
        CurrentScale = gameObject.transform.localScale.x;
        TargetScale = percent * OriginalScale.x;

        ScaleTime = 0.0f;
        InterpolationTime = 0.0f;

        SetScale(TargetScale);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjecScaleTimer : TimerController
{
    [SerializeField]
    private Transform m_TransformToScale = null;

    [SerializeField]
    private Vector3 m_scaleFrom = new Vector3();

    [SerializeField]
    private Vector3 m_scaleTo = new Vector3();

    private void OnEnable()
    {
        m_TransformToScale.localScale = m_scaleFrom;
    }

    protected new void Update()
    {
        base.Update();

        if (m_TransformToScale != null)
        {
            float lerp_percentage = 1.0f - (currentTime / startingTime);
            m_TransformToScale.localScale = Vector3.Lerp(m_scaleFrom, m_scaleTo, lerp_percentage);
        }
    }
}

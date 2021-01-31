using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactDetectedVisualController : TimerController
{
    [SerializeField]
    Material m_newMaterial = null;

    [SerializeField]
    Material m_originalMaterial = null;

    [SerializeField]
    Renderer m_renderer = null;

    [SerializeField]
    private Color m_displayColor = Color.white;

    [SerializeField]
    private AnimationCurve m_alphaCurve = new AnimationCurve(new Keyframe(0.0f, 0.0f),
                                                             new Keyframe(0.5f, 1.0f),
                                                             new Keyframe(1.0f, 0.0f));

    protected new void Start()
    {
        OnTimerFinished += DisableGameObject;
    }

    private void OnEnable()
    {
        m_originalMaterial = m_renderer.sharedMaterial;
        m_renderer.material = m_newMaterial;
        m_renderer.enabled = true;
        StartTimer();
    }

    private void OnDisable()
    {
        m_renderer.sharedMaterial = m_originalMaterial;
        m_renderer.enabled = false;
    }

    private void DisableGameObject()
    {
        enabled = false;
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();

        float percentage = currentTime / startingTime;

        m_displayColor.a = m_alphaCurve.Evaluate(percentage);
        m_renderer.material.color = m_displayColor;
    }
}

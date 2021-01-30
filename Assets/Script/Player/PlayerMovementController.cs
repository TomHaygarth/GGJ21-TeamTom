﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private int m_playerID = 0;

    [SerializeField]
    private float m_maxVelocity = 1.0f;

    [SerializeField]
    private float m_minVelocityDeadzone = 0.01f;

    [SerializeField]
    private float m_frictionCoefficient = 0.8f;

    [SerializeField]
    private Vector3 m_currentVelocity = new Vector3();

    [SerializeField]
    private InputMapping m_inputMap = null;

    private Vector2 m_inputMovementAxis = new Vector2();
    private Transform m_cachedTransform = null;

    private float CalculateFixedVelocityDelta()
    {
        return m_maxVelocity * m_frictionCoefficient * Time.smoothDeltaTime;
    }

    private void UpdateVelocityForAxis(ref float velocity_axis, float input_axis)
    {
        // If we're pressing up/right
        if (input_axis > m_inputMap.DirectionDeadzone)
        {
            float velocity_delta = CalculateFixedVelocityDelta();
            velocity_axis += velocity_delta;
            velocity_axis = Mathf.Clamp(velocity_axis, -m_maxVelocity, m_maxVelocity);

        }
        // if we're pressing down
        else if (input_axis < -m_inputMap.DirectionDeadzone)
        {
            float velocity_delta = CalculateFixedVelocityDelta();
            velocity_axis -= velocity_delta;
            velocity_axis = Mathf.Clamp(velocity_axis, -m_maxVelocity, m_maxVelocity);
        }
        // if we've stopped pressing up or down
        else
        {
            // decay the vertical velocity toward's 0
            if (velocity_axis > m_minVelocityDeadzone)
            {
                float velocity_delta = CalculateFixedVelocityDelta();
                velocity_axis -= velocity_delta;

                // for safety incase we overshoot and accidentally go negative
                if (velocity_axis < 0.0f)
                {
                    velocity_axis = 0.0f;
                }

            }
            else if (velocity_axis < -m_minVelocityDeadzone)
            {
                float velocity_delta = CalculateFixedVelocityDelta();
                velocity_axis += velocity_delta;

                // for safety incase we overshoot and accidentally go positive
                if (velocity_axis > 0.0f)
                {
                    velocity_axis = 0.0f;
                }
            }
            else
            {
                velocity_axis = 0.0f;
            }
        }
    }

    private void Start()
    {
        // cache the transform for efficiency 
        // I'm unsure if unity still doesn't cache this but calls GetComponent<Transform>()
        // under the hood but this article seems to suggest it might http://blog.collectivemass.com/2019/06/unity-myth-buster-gameobject-transform-vs-cached-transform/
        m_cachedTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(m_inputMap.UpKey) == true)
        {
            m_inputMovementAxis.y = 1.0f;
        }
        else if (Input.GetKey(m_inputMap.DownKey) == true)
        {
            m_inputMovementAxis.y = -1.0f;
        }
        else
        {
            m_inputMovementAxis.y = 0.0f;
        }

        if (Input.GetKey(m_inputMap.LeftKey) == true)
        {
            m_inputMovementAxis.x = -1.0f;
        }
        else if (Input.GetKey(m_inputMap.RightKey) == true)
        {
            m_inputMovementAxis.x = 1.0f;
        }
        else
        {
            m_inputMovementAxis.x = 0.0f;
        }

        UpdateVelocityForAxis(ref m_currentVelocity.x, m_inputMovementAxis.x);
        m_currentVelocity.y = 0.0f; // for sanity let's just make sure y is always 0
        UpdateVelocityForAxis(ref m_currentVelocity.z, m_inputMovementAxis.y);

        // get normlised version of our current velocity
        Vector3 norm_velocity = m_currentVelocity.normalized;

        m_cachedTransform.position += Vector3.Scale(norm_velocity, m_currentVelocity);
    }
}

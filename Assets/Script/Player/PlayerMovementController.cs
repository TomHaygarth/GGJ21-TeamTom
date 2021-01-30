﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private int m_playerID = 0;

    [SerializeField]
    private float m_maxVelocity = 1.0f;

    // should be
    //[SerializeField]
    //private float m_velocityDecay = 0.25f;

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
    private Rigidbody m_cachedBody = null;
    private Vector3 m_lookDirection = new Vector3();

    Stack<float> m_frictionStack = new Stack<float>();

    // Public functions
    public void PushFrictionCoefficient(float coefficient)
    {
        m_frictionStack.Push(coefficient);
    }

    public void PopFrictionCoefficient()
    {
        // make sure the original coeeficient is never lost
        if (m_frictionStack.Count > 1)
        {
            m_frictionStack.Pop();
        }
    }

    // Private functions
    private float CalculateFixedVelocityDelta()
    {
        return m_maxVelocity * m_frictionStack.Peek() * Time.smoothDeltaTime;
    }

    private bool UpdateVelocityForAxis(ref float velocity_axis, float input_axis)
    {
        bool is_moving = false;
        float clamp_scalar = Mathf.Abs(input_axis);

        // If we're pressing up/right
        if (input_axis > m_inputMap.DirectionDeadzone)
        {
            float velocity_delta = CalculateFixedVelocityDelta();
            velocity_axis += velocity_delta;
            velocity_axis = Mathf.Clamp(velocity_axis, -m_maxVelocity * clamp_scalar, m_maxVelocity * clamp_scalar);
            is_moving = true;
        }
        // if we're pressing down
        else if (input_axis < -m_inputMap.DirectionDeadzone)
        {
            float velocity_delta = CalculateFixedVelocityDelta();
            velocity_axis -= velocity_delta;
            velocity_axis = Mathf.Clamp(velocity_axis, -m_maxVelocity * clamp_scalar, m_maxVelocity * clamp_scalar);
            is_moving = true;
        }
        // decay the vertical velocity toward's 0
        if (velocity_axis > m_minVelocityDeadzone)
        {
            float velocity_delta = CalculateFixedVelocityDelta();
            velocity_axis -= velocity_delta * (1.0f - Mathf.Abs(input_axis));
            is_moving = true;

            // for safety incase we overshoot and accidentally go negative
            if (velocity_axis < 0.0f)
            {
                velocity_axis = 0.0f;
            }

        }
        else if (velocity_axis < -m_minVelocityDeadzone)
        {
            float velocity_delta = CalculateFixedVelocityDelta();
            velocity_axis += velocity_delta * (1.0f - Mathf.Abs(input_axis));
            is_moving = true;

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
        return is_moving;
    }

    private void Start()
    {
        // cache the transform for efficiency 
        // I'm unsure if unity still doesn't cache this but calls GetComponent<Transform>()
        // under the hood but this article seems to suggest it might http://blog.collectivemass.com/2019/06/unity-myth-buster-gameobject-transform-vs-cached-transform/
        m_cachedTransform = transform;
        m_cachedBody = GetComponent<Rigidbody>();
        m_frictionStack.Push(m_frictionCoefficient);

        if(m_cachedBody != null)
        {
            m_cachedBody.freezeRotation = true;
            m_cachedBody.useGravity = false;
            m_cachedBody.constraints = RigidbodyConstraints.FreezePositionY
                                     | RigidbodyConstraints.FreezeRotationX
                                     | RigidbodyConstraints.FreezeRotationY
                                     | RigidbodyConstraints.FreezeRotationZ;
        }
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
            // if no key is pressed check if they're using a gamepad instead
            m_inputMovementAxis.y = Input.GetAxis(m_inputMap.GamepadLeftstickVertical);
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
            // if no key is pressed check if they're using a gamepad instead
            m_inputMovementAxis.x = Input.GetAxis(m_inputMap.GamepadLeftstickHorizontal);
        }

        bool update_position = false;

        update_position |= UpdateVelocityForAxis(ref m_currentVelocity.x, m_inputMovementAxis.x);
        m_currentVelocity.y = 0.0f; // for sanity let's just make sure y is always 0
        update_position |= UpdateVelocityForAxis(ref m_currentVelocity.z, m_inputMovementAxis.y);

        // only update our look direction if the input is changing
        if (m_inputMovementAxis.x < -m_inputMap.DirectionDeadzone
         || m_inputMovementAxis.x > m_inputMap.DirectionDeadzone
         || m_inputMovementAxis.y < -m_inputMap.DirectionDeadzone
         || m_inputMovementAxis.y > m_inputMap.DirectionDeadzone)
        {
            m_lookDirection.x = m_inputMovementAxis.x;
            m_lookDirection.y = 0.0f;
            m_lookDirection.z = m_inputMovementAxis.y;
        }

        if (update_position == true)
        {
            // get normlised version of our current velocity
            Vector3 norm_velocity = m_currentVelocity.normalized;

            //float rotation_y = Vector3.Angle(Vector3.forward, norm_velocity);
            Vector3 look_direction = m_lookDirection.normalized;

            m_cachedTransform.rotation = Quaternion.LookRotation(look_direction, Vector3.up);

            norm_velocity.x = Mathf.Abs(norm_velocity.x);
            norm_velocity.y = 0.0f; // for sanity let's just make sure y is always 0
            norm_velocity.z = Mathf.Abs(norm_velocity.z);

            Vector3 final_velocity = Vector3.Scale(norm_velocity, m_currentVelocity);

            if (m_cachedBody != null)
            {
                m_cachedBody.velocity = final_velocity;
            }
            else
            {
                m_cachedTransform.position += Vector3.Scale(norm_velocity, m_currentVelocity);
            }
        }
    }
}

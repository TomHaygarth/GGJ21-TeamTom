using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Serializable]
    private class DefaultFrictionZone : IFrictionZone
    {
        public float FrictionCoefficient { get; set; }

        public float SpeedCap { get; set; }
    }

    [SerializeField]
    private int m_playerID = 0;

    [SerializeField]
    private float m_maxVelocity = 10.0f;

    [SerializeField]
    private float m_minVelocityDeadzone = 0.01f;

    [SerializeField]
    private float m_frictionCoefficient = 1.0f;

    [SerializeField]
    private Vector3 m_currentVelocity = new Vector3();

    [SerializeField]
    private IPlayerInputController m_input = null;

    // Hack! but we need a place to grab the input map for the player for use in the dig controller
    //public InputMapping Controls { get { return m_inputMap; } }

    private Transform m_cachedTransform = null;
    private Rigidbody m_cachedBody = null;
    private Vector3 m_lookDirection = new Vector3();

    List<IFrictionZone> m_frictionZones = new List<IFrictionZone>();

    [SerializeField]
    float m_currentSpeed = 0.0f;

    [SerializeField]
    private bool m_movementPaused = false;

    private DefaultFrictionZone m_defaultFrictionZone = new DefaultFrictionZone();

    public float CurrentSpeed { get { return m_currentSpeed; } }

    // Public functions
    public void AddFrictionZone(IFrictionZone zone)
    {
        m_frictionZones.Add(zone);
    }

    public void RemoveFrictionZone(IFrictionZone zone)
    {
        m_frictionZones.Remove(zone);
    }

    public void PausePlayerpMovement()
    {
        m_movementPaused = true;
        m_currentVelocity = Vector3.zero;
        m_cachedBody.velocity = Vector3.zero;
        m_currentSpeed = 0.0f;
    }

    public void ResumePlayerpMovement()
    {
        m_movementPaused = false;
    }

    // Private functions
    private float CalculateFixedVelocityDelta()
    {
        IFrictionZone zone = m_frictionZones[m_frictionZones.Count - 1];
        return zone.SpeedCap * zone.FrictionCoefficient * Time.smoothDeltaTime;
    }

    private bool UpdateVelocityForAxis(ref float velocity_axis, float input_axis)
    {
        bool is_moving = false;
        float clamp_scalar = Mathf.Abs(input_axis);
        IFrictionZone zone = m_frictionZones[m_frictionZones.Count - 1];
        float max_velocity = zone.SpeedCap;

        // If we're pressing up/right
        if (input_axis > m_input.DirectionDeadzone)
        {
            float velocity_delta = CalculateFixedVelocityDelta();
            velocity_axis += velocity_delta;
            velocity_axis = Mathf.Clamp(velocity_axis, -max_velocity * clamp_scalar, max_velocity * clamp_scalar);
            is_moving = true;
        }
        // if we're pressing down
        else if (input_axis < -m_input.DirectionDeadzone)
        {
            float velocity_delta = CalculateFixedVelocityDelta();
            velocity_axis -= velocity_delta;
            velocity_axis = Mathf.Clamp(velocity_axis, -max_velocity * clamp_scalar, max_velocity * clamp_scalar);
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

        if(m_cachedBody != null)
        {
            m_cachedBody.freezeRotation = true;
            m_cachedBody.useGravity = false;
            m_cachedBody.constraints = RigidbodyConstraints.FreezePositionY
                                     | RigidbodyConstraints.FreezeRotationX
                                     | RigidbodyConstraints.FreezeRotationY
                                     | RigidbodyConstraints.FreezeRotationZ;
        }

        m_defaultFrictionZone.FrictionCoefficient = m_frictionCoefficient;
        m_defaultFrictionZone.SpeedCap = m_maxVelocity;
        m_frictionZones.Add(m_defaultFrictionZone);

        m_input = GetComponent<IPlayerInputController>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        m_defaultFrictionZone.FrictionCoefficient = m_frictionCoefficient;
        m_defaultFrictionZone.SpeedCap = m_maxVelocity;
#endif
        if (m_movementPaused == true)
        {
            return;
        }

        bool update_position = false;

        update_position |= UpdateVelocityForAxis(ref m_currentVelocity.x, m_input.MovementAxis.x);
        m_currentVelocity.y = 0.0f; // for sanity let's just make sure y is always 0
        update_position |= UpdateVelocityForAxis(ref m_currentVelocity.z, m_input.MovementAxis.y);

        // only update our look direction if the input is changing
        if (m_input.MovementAxis.x < -m_input.DirectionDeadzone
         || m_input.MovementAxis.x > m_input.DirectionDeadzone
         || m_input.MovementAxis.y < -m_input.DirectionDeadzone
         || m_input.MovementAxis.y > m_input.DirectionDeadzone)
        {
            m_lookDirection.x = m_input.MovementAxis.x;
            m_lookDirection.y = 0.0f;
            m_lookDirection.z = m_input.MovementAxis.y;
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


            m_currentSpeed = final_velocity.magnitude;
        }
    }
}

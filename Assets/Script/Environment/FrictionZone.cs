using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionZone : MonoBehaviour, IFrictionZone
{
    [SerializeField]
    private float m_coefficient = 1.0f;

    [SerializeField]
    private float m_maxSpeed = 5.0f;

    public float FrictionCoefficient { get { return m_coefficient; } }

    public float SpeedCap { get { return m_maxSpeed; } }

    // Start is called before the first frame update
    void OnTriggerEnter(Collider col)
    {
        PlayerMovementController player = col.GetComponent<PlayerMovementController>();
        if (player != null)
        {
            player.AddFrictionZone(this);
        }
    }

    // Update is called once per frame
    void OnTriggerExit(Collider col)
    {
        PlayerMovementController player = col.GetComponent<PlayerMovementController>();
        if (player != null)
        {
            player.RemoveFrictionZone(this);
        }
    }
}

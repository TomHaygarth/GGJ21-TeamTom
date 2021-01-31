﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionZone : MonoBehaviour
{
    [SerializeField]
    private float m_coefficient = 1.0f;

    [SerializeField]
    private float m_maxSpeed = 5.0f;

    // Start is called before the first frame update
    void OnTriggerEnter(Collider col)
    {
        PlayerMovementController player = col.GetComponent<PlayerMovementController>();
        if (player != null)
        {
            player.PushFrictionCoefficient(m_coefficient);
            player.PushSpeedCap(m_maxSpeed);
        }
    }

    // Update is called once per frame
    void OnTriggerExit(Collider col)
    {
        PlayerMovementController player = col.GetComponent<PlayerMovementController>();
        if (player != null)
        {
            player.PopFrictionCoefficient();
            player.PopSpeedCap();
        }
    }
}
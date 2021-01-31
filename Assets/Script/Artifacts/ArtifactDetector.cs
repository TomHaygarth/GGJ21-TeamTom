using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactDetector : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_rigidBody = null;

    [SerializeField]
    private TimerController m_timer = null;

    private void Start()
    {
        m_timer.OnTimerFinished += DisableObject;
    }

    private void DisableObject()
    {
        gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;
        m_rigidBody.position = pos;
        m_rigidBody.velocity = Vector3.zero;
        m_timer.StartTimer();

    }

}

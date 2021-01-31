using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorSpawnController : MonoBehaviour
{
    [SerializeField]
    private TimerController m_spawnTimer = null;

    private Transform m_cachedTransform;

    void Start()
    {
        m_cachedTransform = transform;
        m_spawnTimer.OnTimerFinished += SpawnDetector;
        m_spawnTimer.StartTimer();
    }

    private void SpawnDetector()
    {
        // request a detector to our world position
        GameController.Instance().RequestArtifactDetectorTo(m_cachedTransform.position);

        // restart the spawn timer
        m_spawnTimer.StartTimer();
    }
}

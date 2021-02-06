using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotInputController : MonoBehaviour, IPlayerInputController
{
    private Vector2 m_inputMovementAxis = Vector2.zero;

    public Vector2 MovementAxis { get { return m_inputMovementAxis; } }

    public bool DigPressed { get; private set; }
    public bool DigReleased { get; private set; }

    public float DirectionDeadzone { get { return 0.1f; } }

    [SerializeField]
    Transform m_targetPoint;

    [SerializeField]
    Transform m_cachedTransform;

    [SerializeField]
    private DigZone[] m_digZones;

    [SerializeField]
    private NavMeshPath m_currentPath;
    private int m_pathIdx = 0;

    [SerializeField]
    private float m_waypointDistanceThreshold = 0.2f;

    [SerializeField]
    private NavMeshQueryFilter m_navMeshFilter = new NavMeshQueryFilter();

    [SerializeField]
    private float m_digTime = 6.0f;
    private float m_currentDigTime = 0.0f;

    private void ResetInputs()
    {
        m_inputMovementAxis = Vector2.zero;
        DigPressed = false;
        DigReleased = false;
    }

    private void SelectRandomDigZoneTarget()
    {
        int new_target_idx = Random.Range(0, m_digZones.Length);

        SetTargetDigZone(m_digZones[new_target_idx].transform);
    }

    private void SetTargetDigZone(Transform target)
    {
        if (m_currentPath == null)
        {
            m_currentPath = new NavMeshPath();
        }

        m_targetPoint = target;

        NavMeshHit start_hit;
        NavMesh.SamplePosition(m_cachedTransform.position, out start_hit, 2.0f, NavMesh.AllAreas);
        NavMeshHit end_hit;
        NavMesh.SamplePosition(m_targetPoint.position, out end_hit, 2.0f, NavMesh.AllAreas);

        NavMesh.CalculatePath(start_hit.position,
                              end_hit.position,
                              NavMesh.AllAreas,
                              m_currentPath);

        m_pathIdx = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_cachedTransform = transform;
        m_digZones = FindObjectsOfType<DigZone>();
        ResetInputs();
    }

    // Update is called once per frame
    void Update()
    {
        ResetInputs();

        if (m_targetPoint != null)
        {
            if (m_pathIdx < m_currentPath.corners.Length)
            {
                Vector3 flattened_pos = m_cachedTransform.position;
                flattened_pos.y = m_currentPath.corners[m_pathIdx].y;
                float distnce_sqr = (m_currentPath.corners[m_pathIdx] - flattened_pos).sqrMagnitude;

                if (m_currentPath.corners[m_pathIdx].x < m_cachedTransform.position.x)
                {
                    m_inputMovementAxis.x = -1.0f;
                }
                else if (m_currentPath.corners[m_pathIdx].x > m_cachedTransform.position.x)
                {
                    m_inputMovementAxis.x = 1.0f;
                }

                if (m_currentPath.corners[m_pathIdx].z < m_cachedTransform.position.z)
                {
                    m_inputMovementAxis.y = -1.0f;
                }
                else if (m_currentPath.corners[m_pathIdx].z > m_cachedTransform.position.z)
                {
                    m_inputMovementAxis.y = 1.0f;
                }

                if (distnce_sqr <= m_waypointDistanceThreshold * m_waypointDistanceThreshold)
                {
                    ++m_pathIdx;
                }
            }
            else
            {
                m_targetPoint = null;
                m_currentDigTime = m_digTime;
                DigPressed = true;
            }
        }
        else
        {
            m_currentDigTime -= Time.deltaTime;
            if (m_currentDigTime <= 0.0f)
            {
                DigReleased = true;
                SelectRandomDigZoneTarget();
            }
        }
    }

    void OnDrawGizmos()
    {

        if (Application.isPlaying == false)
        {
            return;
        }

        if (m_currentPath != null)
        {
            Gizmos.color = Color.green;
            //int points_left = m_pathIdx;

            for (int i = m_pathIdx; i < m_currentPath.corners.Length - 1; ++i)
            {
                Vector3 direction = m_currentPath.corners[i + 1] - m_currentPath.corners[i];
                Gizmos.DrawRay(m_currentPath.corners[i], direction);
            }
        }
    }

    void OnGUI()
    {
        if(UnityEditor.Selection.activeTransform != m_cachedTransform)
        {
            return;
        }

        GUILayout.BeginVertical();

        if (GUILayout.Button("Rebuild path"))
        {
            SetTargetDigZone(m_targetPoint);
        }
        if (GUILayout.Button("Select new dig zone"))
        {
            SelectRandomDigZoneTarget();
        }

        GUILayout.EndVertical();
    }
}

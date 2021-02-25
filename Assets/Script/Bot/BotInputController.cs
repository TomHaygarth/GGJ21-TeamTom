using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotInputController : MonoBehaviour, IPlayerInputController, IArtifactCollector
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
    private bool m_isDigging = false;

    [SerializeField]
    private List<DigZone> m_detectedDigZones = new List<DigZone>();
    [SerializeField]
    private DigZone m_currentDigZone = null;

    [SerializeField]
    private float m_botReactionTime = 0.2f;
    [SerializeField]
    private float m_reactionTimer = 0.2f;

    private void ResetInputs()
    {
        m_inputMovementAxis = Vector2.zero;
        DigPressed = false;
        DigReleased = false;
    }

    private void SelectRandomDigZoneTarget()
    {
        int new_target_idx = Random.Range(0, m_digZones.Length);
        m_currentDigZone = m_digZones[new_target_idx];
        SetTargetDigZone(m_currentDigZone.transform);
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
        if (GameSettings.EnableAI == false)
        {
            gameObject.SetActive(false);
        }
        else
        {
            m_cachedTransform = transform;
            m_digZones = FindObjectsOfType<DigZone>();
            ResetInputs();

            GameController.Instance().OnArtifactDetected += AddDetectedArtifactZone;
        }
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
                Vector3 direction = (m_currentPath.corners[m_pathIdx] - flattened_pos);
                float distnce_sqr = direction.sqrMagnitude;

                if (distnce_sqr <= m_waypointDistanceThreshold * m_waypointDistanceThreshold)
                {
                    ++m_pathIdx;
                }
                else
                {
                    Vector3 normalised = direction.normalized;
                    m_inputMovementAxis.x = normalised.x;
                    m_inputMovementAxis.y = normalised.z;
                }
            }
            else
            {
                m_targetPoint = null;

                if (m_currentDigZone.HasArtifact == true)
                {
                    m_currentDigTime = m_digTime;
                    DigPressed = true;
                    m_isDigging = true;
                }
            }
        }
        else if (m_currentDigZone != null)
        {
            m_currentDigTime -= Time.deltaTime;
            if (m_currentDigTime <= 0.0f || m_currentDigZone.HasArtifact == false)
            {
                DigReleased = true;
                m_currentDigZone = null;
                m_isDigging = false;
            }
        }

        m_reactionTimer -= Time.deltaTime;
        if (m_reactionTimer <= 0.0f)
        {
            CheckForBestTarget();
            m_reactionTimer = m_botReactionTime;
        }
    }

    private void CheckForBestTarget()
    {
        if (m_isDigging == true)
        {
            return;
        }

        if (m_detectedDigZones.Any() == false)
        {
            if (m_currentDigZone == null)
            {
                SelectRandomDigZoneTarget();
            }
        }
        else
        {
            m_detectedDigZones.Sort(delegate (DigZone x, DigZone y)
            {
                if (x.CurrentArtifact == null && y.CurrentArtifact == null) return 0;
                else if (x.CurrentArtifact == null) return 1;
                else if (y.CurrentArtifact == null) return -1;
                else if (x.CurrentArtifact.CorrectScore == y.CurrentArtifact.CorrectScore) return 0;
                else return x.CurrentArtifact.CorrectScore > y.CurrentArtifact.CorrectScore ? -1 : 1;
            });

            if (m_detectedDigZones[0].HasArtifact == true)
            {
                if (m_currentDigZone == null
                || m_currentDigZone.HasArtifact == false
                || m_currentDigZone.CurrentArtifact.CorrectScore < m_detectedDigZones[0].CurrentArtifact.CorrectScore)
                {
                    m_currentDigZone = m_detectedDigZones[0];
                    SetTargetDigZone(m_currentDigZone.transform);
                }
            }
            else
            {
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

        if (m_currentPath != null && m_currentPath.corners.Length > 0)
        {
            Gizmos.color = Color.green;
            //int points_left = m_pathIdx;

            Vector3 from = m_cachedTransform.position;
            from.y = m_currentPath.corners[0].y;
            for (int i = m_pathIdx; i < m_currentPath.corners.Length; ++i)
            {
                Vector3 direction = m_currentPath.corners[i] - from;
                Gizmos.DrawRay(from, direction);

                from = m_currentPath.corners[i];
            }
        }
    }

    public void OnArtifactCollected(ArtifactItemData artifact)
    {
        GameController.Instance().CollectedArtifact(artifact, false);
    }

    public void AddDetectedArtifactZone(DigZone zone)
    {
        if (m_detectedDigZones.Contains(zone) == false)
        {
            m_detectedDigZones.Add(zone);
            zone.OnArtifactTaken += RemoveTakenArtifactZone;
        }
    }

    public void RemoveTakenArtifactZone(DigZone zone)
    {
        zone.OnArtifactTaken -= RemoveTakenArtifactZone;
        m_detectedDigZones.Remove(zone);

        if (m_currentDigZone == zone)
        {
            m_targetPoint = null;
        }
    }

#if UNITY_EDITOR
    void OnGUI()
    {
        if (UnityEditor.Selection.activeTransform != m_cachedTransform)
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
#endif
}

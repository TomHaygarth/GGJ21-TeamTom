using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public delegate void ArtifactTypeWantedChanged(ArtifactItemType type);
    public event ArtifactTypeWantedChanged OnArtifactTypeWantedChanged = delegate { };

    [SerializeField]
    private int m_maxArtifacts = 8;

    [SerializeField]
    private Rect m_spawnZone;

    [SerializeField]
    private ArtifactController m_artifactController = null;

    //[SerializeField]
    //private ScoreController m_scoreController = null;

    [SerializeField]
    private List<ArtifactItemData> m_spawnedArtifacts = new List<ArtifactItemData>();

    [SerializeField]
    private ArtifactDetector m_baseArtifactDetector = null;

    [SerializeField]
    private List<ArtifactDetector> m_artifactDetectors = new List<ArtifactDetector>();

    [SerializeField]
    private Transform m_ArtifactDetectorsRoot = null;

    [SerializeField]
    private Transform m_collectedArtifactsRoot = null;

    [SerializeField]
    private int m_levelScore = 0;

    [SerializeField]
    private bool m_StartGameOnLoad = false;

    [SerializeField]
    private bool m_rollOnlyArtifactTypesSpawned = true;

    [SerializeField]
    private TimerController m_artifactTypeRequestTimer = null;
    [SerializeField]
    private TimerController m_levelTimer = null;

    private ArtifactItemType m_currentArtifactRequest = ArtifactItemType.Dino;

    public ArtifactItemType CurrentArtifact { get { return m_currentArtifactRequest; } }

    // A static instance that should only be set once per scene.
    // This will allow us to be able to access the game controller from anywhere
    // as long as
    private static GameController static_instance = null;

    public static GameController Instance() { return static_instance;  }

    public void StartNewGame()
    {
        m_levelScore = 0;
        m_levelTimer.StartTimer();
        RollNewArtefactType();
    }

    public void CollectedArtifact(ArtifactItemData artifact)
    {
        // remove the collected artifact fromour list
        m_spawnedArtifacts.Remove(artifact);

        // Add the artifact to our collected transform root
        artifact.transform.SetParent(m_collectedArtifactsRoot, false);

        // refresh artifact controller
        m_artifactController.RefreshActiveDigZones();

        // spawn new artifacts
        SpawnArtifacts();

        if (artifact.ItemType == m_currentArtifactRequest)
        {
            // roll new artefact
            RollNewArtefactType();
        }
    }

    public void RegisterDigZone(DigZone zone)
    {
        m_artifactController.RegisterDigZone(zone);
    }

    public void RequestArtifactDetectorTo(Vector3 pos)
    {
        ArtifactDetector next_available = null;

        foreach(ArtifactDetector detector in m_artifactDetectors)
        {
            if (detector.gameObject.activeSelf == false)
            {
                next_available = detector;
                break;
            }
        }

        if (next_available == null)
        {
            next_available = Instantiate<ArtifactDetector>(m_baseArtifactDetector,
                                                           m_ArtifactDetectorsRoot,
                                                           false);
            m_artifactDetectors.Add(next_available);
        }

        next_available.SetPosition(pos);
    }

    private void RollNewArtefactType()
    {
        if (m_rollOnlyArtifactTypesSpawned == true && m_spawnedArtifacts.Count > 0)
        {
            int rand_idx = Random.Range(0, m_spawnedArtifacts.Count);
            m_currentArtifactRequest = m_spawnedArtifacts[rand_idx].ItemType;
        }
        else
        {
            int rand_type = Random.Range(0, (int)ArtifactItemType.COUNT);
            m_currentArtifactRequest = (ArtifactItemType)rand_type;
        }
        OnArtifactTypeWantedChanged(m_currentArtifactRequest);
        m_artifactTypeRequestTimer.StartTimer();
    }

    private void Awake()
    {
        if (static_instance != null)
        {
            Debug.LogAssertion("Static instance of GameController already exists. Are there 2 GameControllers in the scene?");
            return;
        }
        static_instance = this;

        // Force a max target of 60fps
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        SpawnArtifacts();

        if (m_StartGameOnLoad == true)
        {
            StartNewGame();
        }
    }

    private void OnDestroy()
    {
        if (static_instance == this)
        {
            static_instance = null;
        }
    }

    private void SpawnArtifacts()
    {
        for (int i = m_spawnedArtifacts.Count; i < m_maxArtifacts; ++i)
        {
            ArtifactItemData artifact
                = m_artifactController.CreateNewArtifact(m_spawnZone);

            m_spawnedArtifacts.Add(artifact);
        }
    }
}

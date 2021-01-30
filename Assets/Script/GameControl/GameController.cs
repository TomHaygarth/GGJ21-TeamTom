using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
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
    private Transform m_collectedArtifactsRoot = null;

    private ArtifactItemType m_currentArtifact = ArtifactItemType.Dino;

    public ArtifactItemType CurrentArtifact { get { return m_currentArtifact; } }

    // A static instance that should only be set once per scene.
    // This will allow us to be able to access the game controller from anywhere
    // as long as
    private static GameController static_instance = null;

    public static GameController Instance() { return static_instance;  }

    public void CollectedArtifact(ArtifactItemData artifact)
    {
        // m_scoreController.ScoreArtifact(artifact);

        // remove the collected artifact fromour list
        m_spawnedArtifacts.Remove(artifact);

        // Add the artifact to our collected transform root
        artifact.transform.SetParent(m_collectedArtifactsRoot, false);

        // spawn new artifacts
        SpawnArtifacts();
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
    void Start()
    {
        SpawnArtifacts();
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

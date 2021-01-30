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

    private List<ArtifactItemData> m_spawnedArtifacts = new List<ArtifactItemData>();

    // A static instance that should only be set once per scene.
    // This will allow us to be able to access the game controller from anywhere
    // as long as
    private static GameController static_instance = null;

    public static GameController Instance() { return static_instance;  }

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
        SpawnInitialArtifacts();
    }

    private void OnDestroy()
    {
        if (static_instance == this)
        {
            static_instance = null;
        }
    }

    private void SpawnInitialArtifacts()
    {
        for (int i = 0; i < m_maxArtifacts; ++i)
        {
            ArtifactItemData artifact
                = m_artifactController.CreateNewArtifact(m_spawnZone);

            m_spawnedArtifacts.Add(artifact);
        }
    }
}

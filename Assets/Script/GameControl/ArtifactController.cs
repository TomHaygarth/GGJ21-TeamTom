using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactController : MonoBehaviour
{
    [SerializeField]
    List<ArtifactItemData> m_Artifacts;

    public ArtifactItemData CreateNewArtifact(Rect area, float y_pos = 0.0f)
    {
        if (m_Artifacts.Count == 0)
        {
            return null;
        }
        float x_pos = Random.Range(0.0f, area.width);
        float z_pos = Random.Range(0.0f, area.width);

        int rand_idx = Random.Range(0, m_Artifacts.Count);

        ArtifactItemData new_artifact = Instantiate<ArtifactItemData>(m_Artifacts[rand_idx],
                                                                      new Vector3(x_pos, y_pos, z_pos),
                                                                      false);

        return new_artifact;
    }
}

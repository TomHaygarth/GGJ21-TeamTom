using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactController : MonoBehaviour
{
    [SerializeField]
    private List<ArtifactItemData> m_Artifacts;

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
                                                                      new Vector3(area.x + x_pos, y_pos, area.y + z_pos),
                                                                      Quaternion.identity);

        return new_artifact;
    }
}

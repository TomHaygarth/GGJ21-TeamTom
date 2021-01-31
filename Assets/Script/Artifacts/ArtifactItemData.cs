using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactItemData : MonoBehaviour
{
    [SerializeField]
    private ArtifactItemType m_ItemType;
    [SerializeField]
    private int m_CorrectItemScore = 100;
    [SerializeField]
    private int m_IncorrectItemScore = -100;

    public int CorrectScore { get { return m_CorrectItemScore; } }
    public int IncorrectScore { get { return m_IncorrectItemScore; } }
    public ArtifactItemType ItemType { get { return m_ItemType; } }

    [SerializeField]
    private ArtifactDetectedVisualController m_detectedVisualController = null;
    public ArtifactDetectedVisualController DetectedVisualController { get { return m_detectedVisualController; } }
}

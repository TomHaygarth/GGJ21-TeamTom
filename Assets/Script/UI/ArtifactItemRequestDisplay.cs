using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactItemRequestDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject[] displayObjects = new GameObject[(int)ArtifactItemType.COUNT];

    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance().OnArtifactTypeWantedChanged += OnArtifactRequestChange;
    }

    private void OnArtifactRequestChange(ArtifactItemType itemType)
    {
        for (int i = 0; i < displayObjects.Length; ++i)
        {
            bool enable_object = i == (int)itemType;

            displayObjects[i].SetActive(enable_object);
        }
    }
}

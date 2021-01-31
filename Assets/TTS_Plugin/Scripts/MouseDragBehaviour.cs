using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MouseDragBehaviour : MonoBehaviour , IDragHandler//, IPointerUpHandler
{
    [SerializeField] private RectTransform rect;
    private Canvas canvas;


    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }
    public void OnDrag(PointerEventData eventdata)
    {
        Vector2 prevPos = rect.anchoredPosition;
        rect.anchoredPosition += eventdata.delta/canvas.scaleFactor;
        if (!IsRectTransformInsideSreen(rect))
        {
            rect.anchoredPosition = prevPos;
        }
    }

    
    private bool IsRectTransformInsideSreen(RectTransform rectTransform)
    {
        bool isInside = false;
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        int visibleCorners = 0;
        Rect rect2 = new Rect(0, 0, Screen.width, Screen.height);
        foreach (Vector3 corner in corners)
        {
            if (rect2.Contains(Camera.main.WorldToScreenPoint( corner )))
            {
                visibleCorners++;
            }
        }
        if (visibleCorners == 4)
        {
            isInside = true;
        }
        return isInside;
    }
}
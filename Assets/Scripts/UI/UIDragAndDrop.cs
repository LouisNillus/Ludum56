using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIDragAndDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isDragging = false;

    public UnityEvent OnMouseDown = new();

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isDragging)
        {
            OnMouseDown.Invoke();
            isDragging = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}

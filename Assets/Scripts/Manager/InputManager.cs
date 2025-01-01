using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] ModeManager modeManager;

    public void OnBeginDrag(PointerEventData eventData)
    {
        modeManager.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        modeManager.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        modeManager.OnEndDrag(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        modeManager.OnPointerClick(eventData);
    }
}
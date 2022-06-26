using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class DraggableElement : MonoBehaviour, IPointerDownHandler,
    IBeginDragHandler,IEndDragHandler, IDragHandler, IDropHandler
{
    private Canvas _mainCanvas;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Vector2 _previousPos;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _mainCanvas = FindObjectOfType<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _previousPos = _rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        
        if (eventData.pointerEnter == null)
        {
            _rectTransform.anchoredPosition = _previousPos;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = _rectTransform.anchoredPosition;
            
            _rectTransform.anchoredPosition = eventData.pointerDrag.GetComponent<DraggableElement>().GetPreviousPos();
        }
    }

    public Vector2 GetPreviousPos()
    {
        return _previousPos;
    }
}

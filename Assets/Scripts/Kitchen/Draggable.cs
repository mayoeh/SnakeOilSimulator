using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform rectTransform;
    public Vector2 originalPosition;
    public ItemData item;
    public IngredientUIManager ingredientUI;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = originalPosition;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnClick()
    {
        Debug.Log(InventoryManager.Instance);
        ingredientUI.OpenShop(item);
    }
}
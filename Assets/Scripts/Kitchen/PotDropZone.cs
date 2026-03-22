using UnityEngine;
using UnityEngine.EventSystems;

public class PotDropZone : MonoBehaviour, IDropHandler
{
    public ItemData currentItem;
    public void OnDrop(PointerEventData eventData)
    {
        Draggable ingredient = eventData.pointerDrag?.GetComponent<Draggable>();
        currentItem = ingredient.item;
        if (ingredient != null && InventoryManager.Instance.GetItemCount(currentItem) >= 1)
        {
            GameManager.healing += currentItem.healingVal;
            GameManager.strength += currentItem.strengthVal;
            GameManager.toxicity += currentItem.toxicityVal;
            GameManager.intoxication += currentItem.intoxicationVal;
            GameManager.luck += currentItem.luckVal;
            GameManager.charm += currentItem.charmVal;
            InventoryManager.Instance.RemoveItem(currentItem);
        } else
        {
            Debug.Log("You don't have any more ingredients!");
        }
    }
}
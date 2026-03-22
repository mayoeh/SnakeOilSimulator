using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Unity.VisualScripting;

public class PotDropZone : MonoBehaviour, IDropHandler
{
    public ItemData currentItem;
    public int totalIngredients;
    public GameObject warning;
    public float activeDuration = 3.0f;

    void Awake()
    {
        warning.SetActive(false);
    }

    public void ActivateForDuration()
    {
        StartCoroutine(ToggleObjectRoutine());
    }

    private IEnumerator ToggleObjectRoutine()
    {
        warning.SetActive(true);

        yield return new WaitForSeconds(activeDuration);

        warning.SetActive(false);
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (totalIngredients <= 2)
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
                totalIngredients += 1;
            } else
            {
                Debug.Log("You don't have any more ingredients!");
            }
        } else
        {
            ActivateForDuration();
            Debug.Log("Tried to add more than 3 ingredients");
        }
        
    }

    public void ResetPot()
    {
        currentItem = null;
        totalIngredients = 0;
    }
}
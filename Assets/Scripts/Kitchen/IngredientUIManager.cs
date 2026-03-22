using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientUIManager : MonoBehaviour
{
    public GameObject panel;
    public ItemData currentItem;
    public Image itemImage;
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public TMP_Text itemAmountText;



    public void OpenShop(ItemData item)
    {
        panel.SetActive(true);
        currentItem = item;

        itemImage.sprite = item.image;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;
        itemAmountText.text = "Amount: " + InventoryManager.Instance.GetItemCount(item).ToString();
    }

    public void UpdateUI()
    {
        if (currentItem == null) return;

        int count = InventoryManager.Instance.GetItemCount(currentItem);
        itemAmountText.text = "Amount: " + count.ToString();
    }

    public void CloseUI()
    {
        panel.SetActive(false);
    }

    
}
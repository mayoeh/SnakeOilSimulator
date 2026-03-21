using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUIManager : MonoBehaviour
{
    public GameObject panel;
    public ItemData currentItem;
    public Image itemImage;
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public TMP_Text itemAmountText;
    public TMP_Text itemCostText;
    public TMP_Text coinText;
    public ShopItemButton itemButton;

    public void OpenShop(ItemData item)
    {
        panel.SetActive(true);
        currentItem = item;

        itemImage.sprite = item.image;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;
        itemAmountText.text = "Amount: " + InventoryManager.Instance.GetItemCount(item).ToString();
        itemCostText.text = "Buy: " + item.price.ToString() + " Coins";
    }

    public void BuyItem()
    {
        if (InventoryManager.Instance.SpendCoins(currentItem.price))
        {
            InventoryManager.Instance.AddItem(currentItem, 1);
            coinText.text = "Coins: " +  InventoryManager.Instance.Coins.ToString();
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    public void UpdateUI()
    {
        if (currentItem == null) return;

        int count = InventoryManager.Instance.GetItemCount(currentItem);
        itemAmountText.text = "Amount: " + count.ToString();
    }

    public void CloseShop()
    {
        panel.SetActive(false);
    }
}
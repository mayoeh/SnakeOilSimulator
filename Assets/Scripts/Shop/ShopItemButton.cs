using UnityEngine;

public class ShopItemButton : MonoBehaviour
{
    public ItemData item;
    public ShopUIManager shopUI;

    public void OnClick()
    {
        shopUI.OpenShop(item);
    }
}
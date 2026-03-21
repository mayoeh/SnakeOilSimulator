using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int Coins;
    public TMP_Text coinText;

    // This builds the first time the scene is opened and then exists in every scene used after the first scene
    private Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();

    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Starting Coins
        Coins = 100;
        //coinText.text = "Coins: " +  Coins.ToString();

    }

    public void AddItem(ItemData item, int amount = 1)
    {
        if (inventory.ContainsKey(item))
            inventory[item] += amount;
        else
            inventory[item] = amount;
    }

    public int GetItemCount(ItemData item)
    {
        if (inventory.ContainsKey(item))
            return inventory[item];

        return 0;
    }

    public void RemoveItem(ItemData item, int amount = 1)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] -= amount;

            if (inventory[item] <= 0)
                inventory.Remove(item);
        }
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
    }

    public bool SpendCoins(int amount)
    {
        if (Coins < amount) return false;
        Coins -= amount;
        return true;
    }
}
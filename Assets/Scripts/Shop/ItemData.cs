using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Shop/Item")]
public class ItemData : ScriptableObject
{
    // Shop Data
    public string itemName;
    public string description;
    public int price;
    public static int amount;
    public Sprite image;

    // Stats
    public int healingVal;
    public int strengthVal;
    public int toxicityVal;
    public int intoxicationVal;
    public int luckVal;
    public int charmVal;
}
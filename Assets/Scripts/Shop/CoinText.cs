using UnityEngine;
using TMPro;

public class CoinText : MonoBehaviour
{
    public TMP_Text coinTxt;

    void Start()
    {
        coinTxt.text = "Coins: " + InventoryManager.Instance.Coins.ToString();
    }
}

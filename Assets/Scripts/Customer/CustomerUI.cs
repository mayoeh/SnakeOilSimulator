using UnityEngine;

public class CustomerUI : MonoBehaviour
{
    public void OnGoToKitchen()
    {
        GameManager.Instance.GoToKitchen();
    }
}
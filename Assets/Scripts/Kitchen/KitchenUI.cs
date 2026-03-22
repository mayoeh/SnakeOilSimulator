using UnityEngine;

public class KitchenUI : MonoBehaviour
{
    public KitchenManager kitchenManager;

    public void OnSubmitClicked()
    {
        kitchenManager.SubmitRecipe();
    }

    public void OnBackClicked()
    {
        GameManager.Instance.GoToCustomer();
    }
}
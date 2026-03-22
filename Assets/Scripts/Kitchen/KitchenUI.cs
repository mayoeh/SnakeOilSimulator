using UnityEngine;

public class KitchenUI : MonoBehaviour
{
    public KitchenManager kitchenManager;
    public PotDropZone potZone;

    public void OnSubmitClicked()
    {
        kitchenManager.SubmitRecipe();
        potZone.ResetPot();
    }

    public void OnBackClicked()
    {
        GameManager.Instance.GoToCustomer();
    }
}
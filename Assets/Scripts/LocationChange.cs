using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationChange : MonoBehaviour
{
    public string sceneName;
    public GameState currentState;
    public void changeScene()
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("Tried to change scenes");
    }

    public void GoToCustomer()
    {
        currentState = GameState.Needs;
        SceneManager.LoadScene("Customer");
    }

    public void OnContinueButton()
    {
        GameManager.Instance.StartNextDay();
        GameManager.Instance.GoToCustomer();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState currentState;
    public static int healing = 0;
    public static int strength = 0;
    public static int toxicity = 0;
    public static int intoxication = 0;
    public static int luck = 0;
    public static int charm = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GoToKitchen()
    {
        Debug.Log("GoToKitchen called");
        currentState = GameState.Cooking;
        SceneManager.LoadScene("Kitchen");
    }

    public void GoToCustomer()
    {
        currentState = GameState.Needs;
        SceneManager.LoadScene("Customer");
    }

    public void SubmitRecipe(RecipeResult tier, int coins)
    {
        // Store the result in a persistent object
        ResultData.Instance.recipeSubmitted = true;
        ResultData.Instance.resultTier = tier;
        ResultData.Instance.coinsEarned = coins;

        currentState = GameState.Result;
        SceneManager.LoadScene("Customer");
    }

}
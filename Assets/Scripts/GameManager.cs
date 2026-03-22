using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int currentDay = 1;
    public int customersServedToday = 0;

    public int maxDays = 3;
    public int customersPerDay = 3;

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

    public void OnCustomerFinished()
    {
        customersServedToday++;

        CustomerManager.Instance.NextCustomer();

        if (customersServedToday >= customersPerDay)
        {
            EndDay();
        }
        else
        {
            GoToCustomer(); // next customer
        }
    }

    void EndDay()
    {
        if (currentDay >= maxDays)
        {
            EndGame();
        }
        else
        {
            SceneManager.LoadScene("Shop");
        }
    }

    public void StartNextDay()
    {
        currentDay++;
        customersServedToday = 0;
    }

    void EndGame()
    {
        Debug.Log("Game Complete!");
        // Load ending scene if you want
        // SceneManager.LoadScene("EndScene");
    }

}
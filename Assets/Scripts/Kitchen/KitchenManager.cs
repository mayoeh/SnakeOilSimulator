using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenManager : MonoBehaviour
{
    [Header("Current Customer")]
    public Customer currentCustomer;

    [Header("Player Ingredients")]
    public List<ItemData> playerIngredients = new List<ItemData>();


    void Start()
    {
        currentCustomer = CustomerManager.Instance.currentCustomer;
    }

    public void SubmitRecipe()
    {
        // Evaluate recipe
        RecipeResult result = EvaluateRecipe();

        // Determine coins
        int coins = GetCoins(result);

        // Optional dialogue (for storing in ResultData)
        string line = GetDialogue(result);

        // Store results in persistent ResultData
        ResultData.Instance.lastScore = CalculateScore();
        ResultData.Instance.coinsEarned = coins;
        ResultData.Instance.dialogue = line;
        ResultData.Instance.resultTier = result;
        ResultData.Instance.lastResult = result != RecipeResult.Bad;
        ResultData.Instance.recipeSubmitted = true;

        // Award coins
        InventoryManager.Instance.AddCoins(coins);

        // Reset GameManager stats for next recipe
        ResetStats();

        // Load Customer scene to show results
        SceneManager.LoadScene("Customer");
    }

    private RecipeResult EvaluateRecipe()
    {
        float totalDiff = 0f;
        int count = 0;

        if (currentCustomer.healing_ideal >= 0)
        {
            totalDiff += Mathf.Abs(currentCustomer.healing_ideal - GameManager.healing);
            count++;
        }

        if (currentCustomer.strength_ideal >= 0)
        {
            totalDiff += Mathf.Abs(currentCustomer.strength_ideal - GameManager.strength);
            count++;
        }

        if (currentCustomer.luck_ideal >= 0)
        {
            totalDiff += Mathf.Abs(currentCustomer.luck_ideal - GameManager.luck);
            count++;
        }

        if (currentCustomer.charm_ideal >= 0)
        {
            totalDiff += Mathf.Abs(currentCustomer.charm_ideal - GameManager.charm);
            count++;
        }

        // Average difference
        float averageDiff = (count > 0) ? totalDiff / count : 0f;

        // Calculate score out of 100
        float score = Mathf.Clamp(100 - averageDiff * 10f, 0, 100);
        return GetTierFromScore(score);
    }

    private float CalculateScore()
    {
        float totalDiff = 0f;
        int count = 0;

        if (currentCustomer.healing_ideal >= 0)
        {
            totalDiff += Mathf.Abs(currentCustomer.healing_ideal - GameManager.healing);
            count++;
        }

        if (currentCustomer.strength_ideal >= 0)
        {
            totalDiff += Mathf.Abs(currentCustomer.strength_ideal - GameManager.strength);
            count++;
        }

        if (currentCustomer.luck_ideal >= 0)
        {
            totalDiff += Mathf.Abs(currentCustomer.luck_ideal - GameManager.luck);
            count++;
        }

        if (currentCustomer.charm_ideal >= 0)
        {
            totalDiff += Mathf.Abs(currentCustomer.charm_ideal - GameManager.charm);
            count++;
        }

        float averageDiff = (count > 0) ? totalDiff / count : 0f;
        return Mathf.Clamp(100 - averageDiff * 10f, 0, 100);
    }

    private RecipeResult GetTierFromScore(float score)
    {
        if (score >= 90) return RecipeResult.Perfect;
        if (score >= 75) return RecipeResult.Great;
        if (score >= 60) return RecipeResult.Good;
        if (score >= 40) return RecipeResult.Okay;
        return RecipeResult.Bad;
    }

    private int GetCoins(RecipeResult result)
    {
        switch (result)
        {
            case RecipeResult.Perfect: return 50;
            case RecipeResult.Great: return 40;
            case RecipeResult.Good: return 25;
            case RecipeResult.Okay: return 15;
            case RecipeResult.Bad: return 5;
            default: return 0;
        }
    }

    private string GetDialogue(RecipeResult result)
    {
        switch (result)
        {
            case RecipeResult.Perfect: return "This is incredible! Exactly what I needed!";
            case RecipeResult.Great: return "Wow, this is really good!";
            case RecipeResult.Good: return "Not bad, this will do.";
            case RecipeResult.Okay: return "Hmm… I guess it works.";
            case RecipeResult.Bad: return "What is this? This is awful!";
            default: return "";
        }
    }

    private void ResetStats()
    {
        GameManager.healing = 0;
        GameManager.strength = 0;
        GameManager.luck = 0;
        GameManager.charm = 0;
        GameManager.toxicity = 0;
        GameManager.intoxication = 0;
    }
}
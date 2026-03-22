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
        // Calculate score based on proximity to ideal
        float score = CalculateScore();

        // Determine tier for dialogue
        RecipeResult tier = GetTierFromScore(score);

        // Determine coins from score with some variation
        int coins = CalculateCoins(score);

        ResultData.Instance.lastScore = score;
        ResultData.Instance.resultTier = tier;
        ResultData.Instance.coinsEarned = coins;
        ResultData.Instance.lastResult = tier != RecipeResult.Bad;
        ResultData.Instance.recipeSubmitted = true;

        // Award coins
        InventoryManager.Instance.AddCoins(coins);

        // Reset stats for next recipe
        ResetStats();

        // Load Customer scene to show results
        SceneManager.LoadScene("Customer");
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

        float avgDiff = (count > 0) ? totalDiff / count : 0f;

        // Base score out of 100
        float score = 100 - avgDiff * 10f;

        // Clamp to 0–100
        return Mathf.Clamp(score, 0, 100);
    }

    private RecipeResult GetTierFromScore(float score)
    {
        if (score >= 90) return RecipeResult.Perfect;
        if (score >= 75) return RecipeResult.Great;
        if (score >= 60) return RecipeResult.Good;
        if (score >= 40) return RecipeResult.Okay;
        return RecipeResult.Bad;
    }

    private int CalculateCoins(float score)
    {
        // Score rounded plus 0–5 random bonus
        return Mathf.RoundToInt(score) + Random.Range(0, 6);
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
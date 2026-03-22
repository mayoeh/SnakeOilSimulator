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
        // No Submission catch
        if (playerIngredients == null || playerIngredients.Count == 0)
        {
            ResultData.Instance.feedback = "You didn't give anything...";
            ResultData.Instance.lastScore = 0;
            ResultData.Instance.resultTier = RecipeResult.Bad;
            ResultData.Instance.coinsEarned = 0;
            ResultData.Instance.lastResult = false;
            ResultData.Instance.recipeSubmitted = true;

            ResetStats();
            SceneManager.LoadScene("Customer");
            return;
        }
        // Calculate score based on proximity to ideal
        float score = CalculateScore();

        // Determine tier for dialogue
        RecipeResult tier = GetTierFromScore(score);

        // Determine coins from score with some variation
        int coins = CalculateCoins(score);

        string subtleFeedback = GenerateFeedback();
        ResultData.Instance.feedback = subtleFeedback;

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
        float penalty = 0;

        // Penalty if any minimum stat is missed
        if(GameManager.healing < currentCustomer.healing_min) penalty += 10;
        if(GameManager.strength < currentCustomer.strength_min) penalty += 10;
        if(GameManager.luck < currentCustomer.luck_min) penalty += 10;
        if(GameManager.charm < currentCustomer.charm_min) penalty += 10;

        // Penalty if over toxicity/intoxication
        if(GameManager.toxicity > currentCustomer.toxicity_max) penalty += 15;
        if(GameManager.intoxication > currentCustomer.intoxication_max) penalty += 15;

        int coins = Mathf.RoundToInt(Mathf.Clamp(score - penalty, 0, 100));

        coins = Mathf.RoundToInt(coins / 2f);
        return coins;
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

    private string GenerateFeedback()
    {
        List<string> feedback = new List<string>();

        // Compare actual vs ideal
        float tolerance = 1f; 

        if (Mathf.Abs(currentCustomer.healing_ideal - GameManager.healing) <= tolerance)
            feedback.Add("Your potion feels invigorating.");
        else if (GameManager.healing < currentCustomer.healing_ideal)
            feedback.Add("It might not restore enough vitality.");
        else
            feedback.Add("Could be too strong for healing.");

        if (Mathf.Abs(currentCustomer.strength_ideal - GameManager.strength) <= tolerance)
            feedback.Add("This pack packs quite a punch.");
        else if (GameManager.strength < currentCustomer.strength_ideal)
            feedback.Add("A bit too weak for what I needed.");
        else
            feedback.Add("A bit overpowering, careful.");

        if (Mathf.Abs(currentCustomer.luck_ideal - GameManager.luck) <= tolerance)
            feedback.Add("I feel a bit luckier already.");
        else if (GameManager.luck < currentCustomer.luck_ideal)
            feedback.Add("Not sure this will change my fortune.");
        else
            feedback.Add("Perhaps too lucky, might be risky.");

        if (Mathf.Abs(currentCustomer.charm_ideal - GameManager.charm) <= tolerance)
            feedback.Add("The aroma is delightful.");
        else if (GameManager.charm < currentCustomer.charm_ideal)
            feedback.Add("Could be more appealing.");
        else
            feedback.Add("Might be too overpowering in charm.");

        if (GameManager.toxicity > currentCustomer.toxicity_max)
            feedback.Add("Watch out — this could be dangerous.");
        if (GameManager.intoxication > currentCustomer.intoxication_max)
            feedback.Add("A bit too potent, careful.");

        // Combine a few feedback lines (choose max 2 to avoid clutter)
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < Mathf.Min(2, feedback.Count); i++)
            sb.AppendLine(feedback[i]);

        return sb.ToString().Trim();
    }
}
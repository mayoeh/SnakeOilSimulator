using System.Collections.Generic;
using UnityEngine;

public class KitchenManager : MonoBehaviour
{
    public float score;
    public Customer currentCustomer;
    public List<string> playerIngredients = new List<string>();
    
    int heal = GameManager.healing;
    int stren = GameManager.strength;
    int lk = GameManager.luck;
    int chm = GameManager.charm;
    int tox = GameManager.toxicity;
    int intox = GameManager.intoxication;

    void Start()
    {
        currentCustomer = CustomerManager.Instance.currentCustomer;
    }

    public void SubmitRecipe()
    {
        RecipeResult result = EvaluateRecipe();

        int coins = GetCoins(result);
        string line = GetDialogue(result);

        // Store results
        ResultData.lastScore = score;
        ResultData.coinsEarned = coins;
        ResultData.dialogue = line;
        ResultData.resultTier = result;
        ResultData.lastResult = result != RecipeResult.Bad;
        Debug.Log("Coins before: " + InventoryManager.Instance.Coins.ToString());
        // Apply coins
        InventoryManager.Instance.AddCoins(coins);
        Debug.Log("Coins after: " + InventoryManager.Instance.Coins.ToString());

        GameManager.Instance.SubmitRecipe();
    }

    public RecipeResult EvaluateRecipe()
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

        score = Mathf.Clamp(100 - averageDiff * 10f, 0, 100);

        if (score >= 90) return RecipeResult.Perfect;
        if (score >= 75) return RecipeResult.Great;
        if (score >= 60) return RecipeResult.Good;
        if (score >= 40) return RecipeResult.Okay;
        return RecipeResult.Bad;
    }

    int GetCoins(RecipeResult result)
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

    string GetDialogue(RecipeResult result)
    {
        switch (result)
        {
            case RecipeResult.Perfect:
                return "This is incredible! Exactly what I needed!";
            case RecipeResult.Great:
                return "Wow, this is really good!";
            case RecipeResult.Good:
                return "Not bad, this will do.";
            case RecipeResult.Okay:
                return "Hmm… I guess it works.";
            case RecipeResult.Bad:
                return "What is this? This is awful!";
            default:
                return "";
        }
    }
}
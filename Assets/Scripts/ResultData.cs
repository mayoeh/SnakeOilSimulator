using UnityEngine;

public enum RecipeResult
{
    Perfect,
    Great,
    Good,
    Okay,
    Bad
}

public class ResultData : MonoBehaviour
{
    public static ResultData Instance;

    [Header("Recipe Submission")]
    public bool recipeSubmitted = false;

    [Header("Result Info")]
    public RecipeResult resultTier; // Tier of the recipe
    public float lastScore = 0f; // Raw score out of 100
    public int coinsEarned = 0; // Coins earned from this recipe
    public string dialogue = ""; // Optional dialogue line (for temporary storage)
    public bool lastResult = false; // True if not Bad, false if Bad

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Reset()
    {
        recipeSubmitted = false;
        resultTier = RecipeResult.Okay;
        lastScore = 0f;
        coinsEarned = 0;
        dialogue = "";
        lastResult = false;
    }
}
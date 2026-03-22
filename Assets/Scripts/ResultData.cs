using UnityEngine;

public enum DialogueStage
{
    NotStarted,    // Nothing shown yet
    IntroShown,    // Intro line has been shown
    NeedsShown,    // Needs/warning line shown before submission
    ResultShown    // Result dialogue has been shown
}

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
    public RecipeResult resultTier;
    public float lastScore = 0f;
    public int coinsEarned = 0;
    public bool lastResult = false;
    public string feedback;

    [Header("Dialogue Tracking")]
    public DialogueStage dialogueStage = DialogueStage.NotStarted;

    [Header("Temporary Per-Customer Coins")]
    public int coinsThisCustomer = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetForNextCustomer()
    {
        recipeSubmitted = false;
        resultTier = RecipeResult.Okay;
        lastScore = 0f;
        coinsEarned = 0;
        coinsThisCustomer = 0;
        lastResult = false;
        dialogueStage = DialogueStage.NotStarted;
    }
}
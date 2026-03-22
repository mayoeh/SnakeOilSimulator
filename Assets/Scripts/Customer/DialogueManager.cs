using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text dialogueText;
    public TMP_Text speakerText;
    public Image portraitImage; 

    public enum DialogueState { Intro, Needs, Result }
    private DialogueState currentState;

    private string[] currentResultLines;
    private int resultLineIndex = 0;

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void Start()
    {
        var stage = ResultData.Instance.dialogueStage;

        switch (stage)
        {
            case DialogueStage.NotStarted:
                ShowIntro();
                break;
            case DialogueStage.IntroShown:
            case DialogueStage.NeedsShown:
                ShowNeeds();
                break;
            case DialogueStage.ResultShown:
                ShowResult();
                break;
        }
    }

    void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.AdvanceDialogue.performed += OnAdvance;
    }

    void OnDisable()
    {
        inputActions.UI.AdvanceDialogue.performed -= OnAdvance;
        inputActions.UI.Disable();
    }

    private void OnAdvance(InputAction.CallbackContext context)
    {
        AdvanceDialogue();
    }

    public void AdvanceDialogue()
    {
        var customer = CustomerManager.Instance.currentCustomer;

        switch (currentState)
        {
            case DialogueState.Intro:
                ShowNeeds();
                ResultData.Instance.dialogueStage = DialogueStage.IntroShown;
                break;

            case DialogueState.Needs:
                if (ResultData.Instance.recipeSubmitted)
                {
                    ShowResult();
                    ResultData.Instance.dialogueStage = DialogueStage.ResultShown;
                }
                else
                {
                    ShowNeeds();
                    //dialogueText.text = "Go to the kitchen to make a recipe to submit!";
                    ResultData.Instance.dialogueStage = DialogueStage.NeedsShown;
                }
                break;

            case DialogueState.Result:
                if (resultLineIndex < currentResultLines.Length - 1)
                {
                    resultLineIndex++;
                    dialogueText.text = currentResultLines[resultLineIndex];
                }
                else
                {
                    GameManager.Instance.OnCustomerFinished();
                    ResetForNextCustomer();
                }
                break;
        }
    }

    public void ShowIntro()
    {
        var customer = CustomerManager.Instance.currentCustomer;

        dialogueText.text = customer.intro;
        speakerText.text = customer.name;
        UpdatePortrait();
        currentState = DialogueState.Intro;
    }

    public void ShowNeeds()
    {
        var customer = CustomerManager.Instance.currentCustomer;

        dialogueText.text = customer.needs;
        UpdatePortrait();
        speakerText.text = customer.name;
        currentState = DialogueState.Needs;
    }

    public void ShowResult()
    {
        var customer = CustomerManager.Instance.currentCustomer;
        speakerText.text = customer.name;
        UpdatePortrait();

        // 1Get the correct tier lines from JSON
        string[] tierLines;
        switch (ResultData.Instance.resultTier)
        {
            case RecipeResult.Perfect: tierLines = customer.reaction_perfect; break;
            case RecipeResult.Great:    tierLines = customer.reaction_great;    break;
            case RecipeResult.Good:     tierLines = customer.reaction_good;     break;
            case RecipeResult.Okay:     tierLines = customer.reaction_okay;     break;
            case RecipeResult.Bad:      tierLines = customer.reaction_bad;      break;
            default:                    tierLines = new string[] { "..." };    break;
        }

        // Add subtle feedback about stats (you need to generate this in KitchenManager)
        string feedback = ResultData.Instance.feedback ?? "Hmm… interesting mixture.";
        
        // Add coins line
        string coinLine = $"You earned {ResultData.Instance.coinsEarned} coins!";

        // Combine everything
        currentResultLines = new string[2 + tierLines.Length];
        currentResultLines[0] = coinLine;       // Coins first
        currentResultLines[1] = feedback;       // Feedback second
        for (int i = 0; i < tierLines.Length; i++)
            currentResultLines[i + 2] = tierLines[i]; // Tiered dialogue after

        // Initialize dialogue
        resultLineIndex = 0;
        dialogueText.text = currentResultLines[resultLineIndex];

        currentState = DialogueState.Result;
    }

    private void ResetForNextCustomer()
    {
        ResultData.Instance.ResetForNextCustomer();
    }

    void UpdatePortrait()
    {
        var customer = CustomerManager.Instance.currentCustomer;

        Sprite portrait = Resources.Load<Sprite>($"Portraits/{customer.portraitName}");

        if (portrait != null)
        {
            portraitImage.sprite = portrait;
        }
    }
}
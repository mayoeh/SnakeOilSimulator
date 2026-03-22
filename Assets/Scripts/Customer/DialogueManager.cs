using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text dialogueText;
    public TMP_Text speakerText;

    public enum DialogueState { Intro, Needs, Result }
    private DialogueState currentState;

    private string[] currentResultLines;
    private int resultLineIndex = 0;
    private bool coinsAwarded = false;

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void Start()
    {
        var stage = ResultData.Instance.dialogueStage;
        var customer = CustomerManager.Instance.currentCustomer;

        // Determine initial state based on saved dialogue stage
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
                    dialogueText.text = "Go to the kitchen to make a recipe to submit!";
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
                    // Finished result lines → next customer
                    CustomerManager.Instance.NextCustomer();
                    ResetForNextCustomer();
                    ShowIntro();
                }
                break;
        }
    }

    public void ShowIntro()
    {
        var customer = CustomerManager.Instance.currentCustomer;

        dialogueText.text = customer.intro;
        speakerText.text = customer.name;
        currentState = DialogueState.Intro;
        coinsAwarded = false;
    }

    public void ShowNeeds()
    {
        var customer = CustomerManager.Instance.currentCustomer;

        dialogueText.text = customer.needs;
        speakerText.text = customer.name;
        currentState = DialogueState.Needs;
    }

    public void ShowResult()
    {
        var customer = CustomerManager.Instance.currentCustomer;
        speakerText.text = customer.name;

        // Select the correct dialogue array from JSON based on ResultData
        switch (ResultData.Instance.resultTier)
        {
            case RecipeResult.Perfect:
                currentResultLines = customer.reaction_perfect;
                break;
            case RecipeResult.Great:
                currentResultLines = customer.reaction_great;
                break;
            case RecipeResult.Good:
                currentResultLines = customer.reaction_good;
                break;
            case RecipeResult.Okay:
                currentResultLines = customer.reaction_okay;
                break;
            case RecipeResult.Bad:
                currentResultLines = customer.reaction_bad;
                break;
            default:
                currentResultLines = new string[] { "..." };
                break;
        }

        string coinLine = $"You earned {ResultData.Instance.coinsThisCustomer} coins!";
        string[] newResultLines = new string[currentResultLines.Length + 1];
        newResultLines[0] = coinLine;
        for (int i = 0; i < currentResultLines.Length; i++)
        {
            newResultLines[i + 1] = currentResultLines[i];
        }
        currentResultLines = newResultLines;

        resultLineIndex = 0;
        dialogueText.text = currentResultLines[resultLineIndex];

        // Award coins once
        if (!coinsAwarded)
        {
            InventoryManager.Instance.AddCoins(ResultData.Instance.coinsThisCustomer);
            coinsAwarded = true;
        }

        currentState = DialogueState.Result;
    }

    private void ResetForNextCustomer()
    {
        ResultData.Instance.ResetForNextCustomer();
        coinsAwarded = false;
    }
}
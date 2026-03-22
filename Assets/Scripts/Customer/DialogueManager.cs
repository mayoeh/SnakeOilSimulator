using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;
    public TMP_Text speakerText;

    public enum DialogueState { Intro, Needs, Result, Finished }
    public DialogueState currentState;
    private int resultLineIndex = 0;
    private string[] currentResultLines;
    private PlayerInputActions inputActions;
    public bool recipeSubmitted = false;

    bool resultCoinsGiven = false;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void Start()
    {
        Customer current = CustomerManager.Instance.currentCustomer;

        // Always show intro first
        ShowIntro();
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
        Customer c = CustomerManager.Instance.currentCustomer;

        switch (currentState)
        {
            case DialogueState.Intro:
                ShowNeeds();
                break;

            case DialogueState.Needs:
                if (ResultData.Instance.recipeSubmitted)
                {
                    ShowResult();
                }
                else
                {
                    dialogueText.text = "You need to submit a recipe first!";
                    // Stay in Needs state until submission
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
                    CustomerManager.Instance.NextCustomer();
                    ResetResultData();
                    ShowIntro();
                }
                break;
        }
    }

    public void ShowIntro()
    {
        dialogueText.text = CustomerManager.Instance.currentCustomer.intro;
        speakerText.text = CustomerManager.Instance.currentCustomer.name;

        currentState = DialogueState.Intro;
        resultCoinsGiven = false;
    }

    public void ShowNeeds()
    {
        dialogueText.text = CustomerManager.Instance.currentCustomer.needs;
        speakerText.text = CustomerManager.Instance.currentCustomer.name;
        currentState = DialogueState.Needs;
    }

    public void ShowResult()
    {
        var c = CustomerManager.Instance.currentCustomer;
        speakerText.text = c.name;

        // Pick dialogue lines based on ResultData.resultTier
        switch (ResultData.Instance.resultTier)
        {
            case RecipeResult.Perfect:
                currentResultLines = c.reaction_perfect;
                break;
            case RecipeResult.Great:
                currentResultLines = c.reaction_great;
                break;
            case RecipeResult.Good:
                currentResultLines = c.reaction_good;
                break;
            case RecipeResult.Okay:
                currentResultLines = c.reaction_okay;
                break;
            case RecipeResult.Bad:
                currentResultLines = c.reaction_bad;
                break;
            default:
                currentResultLines = new string[] { "..." };
                break;
        }

        resultLineIndex = 0;
        dialogueText.text = currentResultLines[resultLineIndex];

        if (!resultCoinsGiven)
        {
            InventoryManager.Instance.AddCoins(ResultData.Instance.coinsEarned);
            resultCoinsGiven = true;
        }

        currentState = DialogueState.Result;
    }

    private void ResetResultData()
    {
        if (ResultData.Instance != null)
        {
            ResultData.Instance.Reset(); // Calls Reset() in ResultData
        }
    }
}
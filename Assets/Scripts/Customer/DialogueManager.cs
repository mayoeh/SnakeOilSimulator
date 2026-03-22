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

    bool resultCoinsGiven = false;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void Start()
    {
        var state = GameManager.Instance.currentState;

        if (state == GameState.Intro)
        {
            ShowIntro();
        }
        else if (state == GameState.Cooking || state == GameState.Needs) 
        {
            ShowNeeds();
        }
        else if (state == GameState.Result)
        {
            ShowResult();
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

        switch(currentState)
        {
            case DialogueState.Intro:
                dialogueText.text = customer.needs;
                currentState = DialogueState.Needs;
                break;

            case DialogueState.Needs:
            // Cannot show results immediately, must put some check here
                ShowResult();
                break;

            case DialogueState.Result:
                if(resultLineIndex < currentResultLines.Length - 1)
                {
                    resultLineIndex++;
                    dialogueText.text = currentResultLines[resultLineIndex];
                }
                else
                {
                    CustomerManager.Instance.NextCustomer();
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

        switch (ResultData.resultTier)
        {
            case RecipeResult.Perfect:
            case RecipeResult.Great:
                currentResultLines = c.reaction_good;
                break;

            case RecipeResult.Good:
            case RecipeResult.Okay:
                currentResultLines = new string[] { ResultData.dialogue };
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
            InventoryManager.Instance.AddCoins(ResultData.coinsEarned);
            resultCoinsGiven = true;
        }
        currentState = DialogueState.Result;

    }
}
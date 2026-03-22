using UnityEngine;

public class CustomerSceneController : MonoBehaviour
{
    public DialogueManager dialogueManager;

    void Start()
    {
        var state = GameManager.Instance.currentState;

        if (state == GameState.Intro)
        {
            dialogueManager.ShowIntro();
        }
        else if (state == GameState.Cooking)
        {
            // returning from kitchen → show needs immediately
            dialogueManager.ShowNeeds();
        }
        else if (state == GameState.Result)
        {
            dialogueManager.ShowResult();
        }
    }
}

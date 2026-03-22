using UnityEngine;
using TMPro;

public class Stats : MonoBehaviour
{
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI endStatsText;

    void Update()
    {
        statsText.text =
            "Healing: " + GameManager.healing + "\n" +
            "Strength: " + GameManager.strength + "\n" +
            "Toxicity: " + GameManager.toxicity + "\n" +
            "Intoxication: " + GameManager.intoxication + "\n" +
            "Luck: " + GameManager.luck + "\n" +
            "Charm: " + GameManager.charm;
    }

    void endSceneStart()
    {
        endStatsText.text =
            "Healing: " + GameManager.healing + "\n" +
            "Strength: " + GameManager.strength + "\n" +
            "Toxicity: " + GameManager.toxicity + "\n" +
            "Intoxication: " + GameManager.intoxication + "\n" +
            "Luck: " + GameManager.luck + "\n" +
            "Charm: " + GameManager.charm;
    }
}
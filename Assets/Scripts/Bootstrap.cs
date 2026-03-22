using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Bootstrap : MonoBehaviour
{
    public string nextSceneName = "Start";

    private void Awake()
    {
        InitializeManagers();
    }

    private void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        yield return null; // wait one frame
        SceneManager.LoadScene(nextSceneName);
    }

    private void InitializeManagers()
    {
        if (GameManager.Instance == null)
            new GameObject("GameManager").AddComponent<GameManager>();

        if (InventoryManager.Instance == null)
            new GameObject("InventoryManager").AddComponent<InventoryManager>();

        if (CustomerManager.Instance == null)
            new GameObject("CustomerManager").AddComponent<CustomerManager>();
    }
}

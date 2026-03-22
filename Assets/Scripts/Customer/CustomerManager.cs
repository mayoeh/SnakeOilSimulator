using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance { get; private set; }

    public Customer currentCustomer;
    private CustomerList customerList;
    private int currentIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadCustomers();
        SetCustomer(0);
    }

    void LoadCustomers()
    {
        TextAsset json = Resources.Load<TextAsset>("customers");
        customerList = JsonUtility.FromJson<CustomerList>(json.text);

    }

    public void SetCustomer(int index)
    {
        if (index < customerList.customers.Length)
        {
            currentIndex = index;
            currentCustomer = customerList.customers[currentIndex];

            Debug.Log("Current Customer: " + currentCustomer.name);
        }
        else
        {
            Debug.Log("No more customers!");
        }
    }

    public void NextCustomer()
    {
        currentIndex++;

        if (currentIndex < customerList.customers.Length)
        {
            currentCustomer = customerList.customers[currentIndex];
            Debug.Log("Next Customer: " + currentCustomer.name);
        }
        else
        {
            Debug.Log("All customers complete!");
        }
    }
}
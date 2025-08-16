using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    public int moneyMade;

    public int currentFare; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
       if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        UpdateUI();
    }

    public void SetFare(int fare)
    {
        currentFare = fare; 
        UpdateUI();
    }

    public void CompleteFare()
    {
        moneyMade += currentFare;
        currentFare = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (MoneyUIManager.Instance != null)
        {
            MoneyUIManager.Instance.UpdateMoneyText(moneyMade, currentFare);
        }
        else
        {
            Debug.Log("XPUIManager Instance not found.");
        }
    }

}

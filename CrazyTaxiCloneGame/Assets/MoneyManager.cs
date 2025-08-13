using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;
    public int moneyMade;
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

    public void addMoney(int amount)
    {
        moneyMade += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (MoneyUIManager.Instance != null)
        {
            MoneyUIManager.Instance.UpdateMoneyText(moneyMade);
        }
        else
        {
            Debug.LogWarning("XPUIManager Instance not found.");
        }
    }

}

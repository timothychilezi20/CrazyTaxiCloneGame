using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class MoneyUIManager : MonoBehaviour
{
    public static MoneyUIManager Instance;
    public TextMeshProUGUI moneyText;

    private void Awake()
    {
       if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateMoneyText(int money)
    {
        if (moneyText != null)
        {
            moneyText.text = "R" + money; 
        }
    }
}

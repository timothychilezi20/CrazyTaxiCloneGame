using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class MoneyUIManager : MonoBehaviour
{
    public static MoneyUIManager Instance;

    public TextMeshProUGUI totalMoneyText;
    public TextMeshProUGUI fareText;

    private void Awake()
    {
      if (Instance == null)
        {
            Instance = this;
        }
    }

    public void UpdateMoneyText(int total, int fare)
    {
        if (totalMoneyText != null)
        {
            totalMoneyText.text = "R" + total;
        }
       
        if (fareText != null)
        {
            fareText.text = "R" + fare;
        }
    }
}

using TMPro;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI customersText;

    public void ShowResults(int money, int customers)
    {
        panel.SetActive(true);

        // Grade system
        if (money < 500)
        {
            resultText.text = "BAD";
            resultText.color = Color.red;
        }
        else if (money < 1000)
        {
            resultText.text = "AVERAGE";
            resultText.color = new Color(1f, 0.5f, 0f);
        }
        else if (money < 2000)
        {
            resultText.text = "GOOD";
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = "EXCELLENT!";
            resultText.color = Color.cyan;
        }

        moneyText.text = $"{money}";
        customersText.text = $"{customers}";
    }
}

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private EndGameUI endGameUI; 
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GameOver(int customersServed)
    {
        if (isGameOver) return;
        isGameOver = true;

        var car = Object.FindFirstObjectByType<CarController>();
        if (car != null) car.enabled = false;

        endGameUI.ShowResults(
            MoneyManager.instance.moneyMade,
            customersServed
        );
    }
}

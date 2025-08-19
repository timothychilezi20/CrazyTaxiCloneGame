using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;

    public static TimerScript Instance;

    private void Awake()
    {
       Instance = this;
    }

    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime <= 0)
        {
            remainingTime = 0;

            int served = PickupAndDropoff.CustomersServed;

            GameManager.Instance.GameOver(served);
        }


        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    public void AddTime(float amount)
    {
        remainingTime += amount;
    }

    public void ResetTimer(float time)
    {
        remainingTime = time;
    }
}

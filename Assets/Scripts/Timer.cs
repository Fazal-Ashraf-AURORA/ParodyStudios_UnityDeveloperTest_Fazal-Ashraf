using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public float timeRemaining = 120f; // 2 minutes in seconds
    public GameObject gameoverUI;

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            GameOver();
        }
    }

    void UpdateTimerText()
    {
        // Convert timeRemaining to minutes and seconds
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        // Format time as "mm:ss"
        timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }

    void GameOver()
    {
        // Trigger Game Over
        Debug.Log("Time's Up! Game Over!");
        gameoverUI.SetActive(true);
    }
}

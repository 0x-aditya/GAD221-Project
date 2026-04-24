using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : ScriptLibrary.Singletons.Singleton<GameTimer>
{
    public float timerLength = 60f; // in seconds
    
    private TextMeshProUGUI timerText;
    private float localTimer;
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        localTimer = timerLength;
        print(localTimer);
    }

    void Update()
    {
        if (localTimer <= 0)
        {
            timerText.text = "00:00";
            TimerEnd();
            return;
        }
        localTimer -= Time.deltaTime;
        int seconds = Mathf.FloorToInt(localTimer);
        int milliseconds = Mathf.FloorToInt((localTimer - seconds) * 100f);
        timerText.text = $"{seconds:00}:{milliseconds:00}";
        
    }

    void TimerEnd()
    {
        SceneManager.LoadScene("_Burnout/2_Scenes/MainMenu");
    }

    public void AddTime(float timeToAdd)
    {
        localTimer += timeToAdd;
    }
}

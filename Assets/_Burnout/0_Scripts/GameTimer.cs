using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float timerLength = 60f; // in seconds
    private TextMeshProUGUI timerText;
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    private float localTimer = 0f;
    void Update()
    {
        if (localTimer >= timerLength)
        {
            timerText.text = $"{timerLength / 60:00}:{timerLength % 60:00}";
            TimerEnd();
            return;
        }
        localTimer += Time.deltaTime;
        timerText.text = $"{localTimer / 60:00}:{localTimer % 60:00}";
        
    }

    void TimerEnd()
    {
        SceneManager.LoadScene("_Burnout/2_Scenes/MainMenu");
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GPAMeter : ScriptLibrary.Singletons.Singleton<GPAMeter>
{
    private TextMeshProUGUI GPABar;
    public Action OnGPAMaxed;
    void Start()
    {
        GPABar = GetComponent<TextMeshProUGUI>();
        OnGPAMaxed = () => { SceneManager.LoadScene("_Burnout/2_Scenes/WinScreen"); };
    }
    /// <summary>
    /// Changes stress value
    /// </summary>
    /// <param name="gpaValue"> value between 0 and 7 to be added or subtracted</param>
    private float currentGPA = 0f;
    public void UpdateGPA(float gpaValue)
    {
        currentGPA += gpaValue;
        currentGPA = Mathf.Clamp(currentGPA, 0f, 7f);
        GPABar.text = "GPA: " + currentGPA.ToString("0.##");
        if (currentGPA >= 7f)
        {
            OnGPAMaxed?.Invoke();
        }
    }
}


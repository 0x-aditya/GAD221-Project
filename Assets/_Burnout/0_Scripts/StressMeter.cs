using System;
using UnityEngine;
using UnityEngine.UI;
public class StressMeter : ScriptLibrary.Singletons.Singleton<StressMeter>
{
    private Image stressBar;
    public Action OnStressMaxed;
    void Start()
    {
        stressBar = GetComponent<Image>();
    }

    /// <summary>
    /// Changes stress value
    /// </summary>
    /// <param name="stressValue"> value between 0 and 1 to be added or subtracted</param>
    public void UpdateStress(float stressValue)
    {
        if (stressValue < 0)
        {
            stressBar.fillAmount = Mathf.Max(0, stressBar.fillAmount + stressValue);
        }
        else
        {
            stressBar.fillAmount = Mathf.Min(1, stressBar.fillAmount + stressValue);
        }

        if (stressBar.fillAmount >= 1)
        {
            OnStressMaxed?.Invoke();
        }
    }
}

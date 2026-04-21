using System;
using UnityEngine;
using UnityEngine.UI;
public class StressMeter : ScriptLibrary.Singletons.Singleton<StressMeter>
{
    private Image _stressBar;
    public Action OnStressMaxed;
    void Start()
    {
        _stressBar = GetComponent<Image>();
    }

    /// <summary>
    /// Changes stress value
    /// </summary>
    /// <param name="stressValue"> value between 0 and 1 to be added or subtracted</param>
    public void AddStress(float stressValue)
    {
        if (stressValue < 0)
        {
            _stressBar.fillAmount = Mathf.Max(0, _stressBar.fillAmount + stressValue);
        }
        else
        {
            _stressBar.fillAmount = Mathf.Min(1, _stressBar.fillAmount + stressValue);
        }

        if (_stressBar.fillAmount >= 1)
        {
            OnStressMaxed?.Invoke();
        }
    }
}

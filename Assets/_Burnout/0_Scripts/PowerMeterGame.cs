using UnityEngine;
using UnityEngine.UI;
using System;

public class PowerMeterGame : MonoBehaviour
{
    [SerializeField] private Image progressBarImage;
    [SerializeField] private Image feedbackOverlay;

    [Header("Settings")]
    public float chargeSpeed = 0.5f;
    [Range(0.5f, 1f)] public float winThreshold = 0.85f;
    public Color normalColor = Color.white;
    public Color overchargeColor = Color.red;
    public Color winColor = Color.green;

    public static event Action<float> OnGameWin;
    public static event Action OnGameFailure;

    private bool _isGameActive = false;
    private bool _isCharging = false;
    private float _currentPower = 0f;

    private void Start()
    {
        StartMiniGame();
    }
    void Update()
    {
        if (!_isGameActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            BeginCharging();
        }

        if (_isCharging)
        {
            _currentPower += Time.unscaledDeltaTime * chargeSpeed;

            if (progressBarImage != null)
            {
                progressBarImage.fillAmount = _currentPower;
            }

            if (_currentPower >= 1.0f)
            {
                FailGame("OVERCHARGED!");
            }

            if (Input.GetMouseButtonUp(0) && _isCharging)
            {
                StopCharging();
            }
        }
    }

    private void BeginCharging()
    {
        _isCharging = true;
        _currentPower = 0f;
        if (progressBarImage != null) progressBarImage.color = normalColor;
        if (feedbackOverlay) feedbackOverlay.color = Color.clear;
    }

    private void StopCharging()
    {
        _isCharging = false;
        CheckResult();
    }

    public void StartMiniGame()
    {
        gameObject.SetActive(true);
        _isGameActive = true;
        _isCharging = false;
        _currentPower = 0f;

        if (progressBarImage != null)
        {
            progressBarImage.fillAmount = 0f;
            progressBarImage.color = normalColor;
        }

        if (feedbackOverlay) feedbackOverlay.color = Color.clear;
    }

    private void CheckResult()
    {
        if (_currentPower >= winThreshold && _currentPower < 1.0f)
        {
            WinGame();
        }
        else
        {
            FailGame("RELEASED TOO EARLY!");
        }
    }

    private void WinGame()
    {
        _isGameActive = false;
        if (progressBarImage != null) progressBarImage.color = winColor;
        if (feedbackOverlay) feedbackOverlay.color = new Color(0, 1, 0, 0.2f);

        OnGameWin?.Invoke(_currentPower);
        Invoke(nameof(CloseGame), 0.8f);
    }

    private void FailGame(string reason)
    {
        _isCharging = false;

        if (progressBarImage != null) progressBarImage.color = overchargeColor;
        if (feedbackOverlay) feedbackOverlay.color = new Color(1, 0, 0, 0.2f);

        OnGameFailure?.Invoke();

    }

    public void CloseGame()
    {
        _isGameActive = false;
        gameObject.SetActive(false);
    }

}

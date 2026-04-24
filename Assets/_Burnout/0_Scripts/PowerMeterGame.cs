using UnityEngine;
using UnityEngine.UI;
using System;

public class PowerMeterGame : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Image progressBarImage;
    [SerializeField] private Image feedbackOverlay;
    [SerializeField] private MonoBehaviour statusText;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip failSound;
    [SerializeField] private AudioClip explodeSound;

    [Header("Settings")]
    public float chargeSpeed = 0.6f;
    public float blinkSpeed = 5f;
    [Range(0.5f, 1f)] public float winThreshold = 0.80f;
    [Range(0.5f, 1f)] public float maxWinThreshold = 0.90f;
    public Color normalColor = Color.white;
    public Color overchargeColor = Color.red;
    public Color winColor = Color.green;

    public static event Action<float> OnGameWin;
    public static event Action OnGameFailure;

    private bool _isGameActive = false;
    private bool _isCharging = false;
    private float _currentPower = 0f;
    private Color _originalTextColor;

    void Start()
    {
        CacheTextColor();
        PauseManager.Instance.PauseGame(true);
        StartMiniGame();
    }
    void Update()
    {
        if (!_isGameActive) return;

        if (!_isCharging && _currentPower <= 0.05f)
        {
            ApplyBlinkEffect();
        }

        if (Input.GetMouseButtonDown(0)) BeginCharging();

        if (_isCharging)
        {
            _currentPower += Time.unscaledDeltaTime * chargeSpeed;
            if (progressBarImage) progressBarImage.fillAmount = _currentPower;

            if (_currentPower >= 1.0f) FailGame("OVERCHARGED!", explodeSound);
        }

        if (Input.GetMouseButtonUp(0) && _isCharging) StopCharging();
    }

    private void BeginCharging()
    {
        _isCharging = true;
        _currentPower = 0f;
        UpdateStatus("CHARGING...");
        SetTextAlpha(1f);
        if (progressBarImage) progressBarImage.color = normalColor;
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

        UpdateStatus($"HOLD TO CHARGE");

        if (progressBarImage)
        {
            progressBarImage.fillAmount = 0;
            progressBarImage.color = normalColor;
        }

        if (feedbackOverlay) feedbackOverlay.color = Color.clear;

        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    private void CheckResult()
    {
        if (_currentPower >= winThreshold && _currentPower <= maxWinThreshold)
        {
            WinGame();
        }
        else if (_currentPower > maxWinThreshold)
        {
            FailGame("TOO LATE!", failSound);
        }
        else
        {
            FailGame("TOO EARLY!", failSound);
        }
    }

    private void WinGame()
    {
        _isGameActive = false;
        UpdateStatus("PERFECT!");
        SetTextAlpha(1f);
        PlaySound(winSound);
        if (progressBarImage) progressBarImage.color = winColor;
        if (feedbackOverlay) feedbackOverlay.color = new Color(0, 1, 0, 0.2f);
        PauseManager.Instance.PauseGame(false);
        StressMeter.Instance.AddStress(0.2f);
        GPAMeter.Instance.UpdateGPA(0.1f);
        OnGameWin?.Invoke(_currentPower);
        Invoke(nameof(CloseGame), 0.8f);
    }

    private void FailGame(string reason, AudioClip clip)
    {
        _isCharging = false;
        UpdateStatus(reason);
        SetTextAlpha(1f);
        PlaySound(clip);
        if (progressBarImage) progressBarImage.color = overchargeColor;
        if (feedbackOverlay) feedbackOverlay.color = new Color(1, 0, 0, 0.2f);
        OnGameFailure?.Invoke();
    }

    private void UpdateStatus(string msg)
    {
        if (statusText is Text t) t.text = msg;
        else if (statusText != null)
        {
            var prop = statusText.GetType().GetProperty("text");
            if (prop != null) prop.SetValue(statusText, msg);
        }
    }

    private void SetTextAlpha(float alpha)
    {
        if (statusText is Graphic g)
        {
            g.color = new Color(_originalTextColor.r, _originalTextColor.g, _originalTextColor.b, alpha);
        }
    }

    private void CacheTextColor()
    {
        if (statusText is Graphic g) _originalTextColor = g.color;
    }

    private void ApplyBlinkEffect()
    {
        float alpha = (Mathf.Sin(Time.unscaledTime * blinkSpeed) + 1.2f) / 2.2f;
        SetTextAlpha(alpha);
    }

    private void PlaySound(AudioClip clip)
    {
        if (sfxSource && clip) sfxSource.PlayOneShot(clip);
    }

    public void CloseGame()
    {
        _isGameActive = false;
        if (bgmSource != null) bgmSource.Stop();
        gameObject.SetActive(false);
    }
}

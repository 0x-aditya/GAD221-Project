using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;


public class SteadyHandGame : MonoBehaviour
{
    [Header("Tag Settings")]
    public string wallTagName = "Wall";
    public string pathTagName = "Path";

    [SerializeField] private RectTransform startPoint;
    [SerializeField] private RectTransform endPoint;
    [SerializeField] private RectTransform playerBall;
    [SerializeField] private Image feedbackOverlay;
    [SerializeField] private MonoBehaviour statusText;

    [Header("Settings")]
    public float restartDelay = 0.5f;
    public float shakeIntensity = 5f;
    public float blinkSpeed = 5f;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip failSound;

    public static event Action OnGameWin;
    public static event Action OnGameFailure;

    private Vector3 _originalPanelPos;
    private Color _originalTextColor;

    private bool _isGameActive = false;
    private bool _isRestarting = false;
    private bool _isDragging = false;

    private void OnEnable()
    {
        _originalPanelPos = transform.localPosition;

        if (statusText is Graphic g) _originalTextColor = g.color;


        if (playerBall != null)
        {
            EventTrigger trigger = playerBall.gameObject.GetComponent<EventTrigger>();
            if (trigger == null) trigger = playerBall.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry beginDrag = new EventTrigger.Entry { eventID = EventTriggerType.BeginDrag };
            beginDrag.callback.AddListener((data) => { OnBallBeginDrag((PointerEventData)data); });
            trigger.triggers.Add(beginDrag);

            EventTrigger.Entry drag = new EventTrigger.Entry { eventID = EventTriggerType.Drag };
            drag.callback.AddListener((data) => { OnBallDrag((PointerEventData)data); });
            trigger.triggers.Add(drag);

            EventTrigger.Entry endDrag = new EventTrigger.Entry { eventID = EventTriggerType.EndDrag };
            endDrag.callback.AddListener((data) => { OnBallEndDrag((PointerEventData)data); });
            trigger.triggers.Add(endDrag);

            Image ballImage = playerBall.GetComponent<Image>();
            if (ballImage != null) ballImage.raycastTarget = true;
        }

        StartMiniGame();
    }

    private void Update()
    {
        if (_isGameActive && !_isDragging && !_isRestarting && statusText != null)
        {
            float alpha = (Mathf.Sin(Time.unscaledTime * blinkSpeed) + 1.2f) / 2.2f;
            SetTextAlpha(alpha);
        }

    }
        
    private void OnBallBeginDrag(PointerEventData eventData)
    {
        if (!_isGameActive || _isRestarting) return;
        _isDragging = true;
        SetTextAlpha(1f);
        UpdateStatus("");
    }

    private void OnBallDrag(PointerEventData eventData)
    {
        if (!_isDragging || _isRestarting) return;

        playerBall.position = eventData.position;

        if (!IsBallSafe(eventData.position))
        {
            FailGame();
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(endPoint, eventData.position))
        {
            WinGame();
        }
    }

    private void OnBallEndDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;

        _isDragging = false;
        if (!_isRestarting && _isGameActive)
        {
            Debug.Log("Drag Ended - Returning to start");
            ResetBallToStart();
        }
    }

    private bool IsBallSafe(Vector2 testPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = testPosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        bool foundPath = false;

        foreach (var result in results)
        {
            if (result.gameObject == playerBall.gameObject) continue;

            if (result.gameObject.CompareTag(wallTagName))
            {
                Debug.Log("Hit Wall: " + result.gameObject.name);
                return false;
            }

            if (result.gameObject.CompareTag(pathTagName))
            {
                foundPath = true;
            }
        }

        return foundPath;
    }

    public void StartMiniGame()
    {
        gameObject.SetActive(true);
        _isGameActive = true;
        _isRestarting = false;
        _isDragging = false;

        ResetBallToStart();
    }

    private void FailGame()
    {
        if (_isRestarting) return;
        _isRestarting = true;
        _isDragging = false;

        UpdateStatus("OUT!");

        SetTextAlpha(1f);
        PlaySound(failSound);
        StartCoroutine(ShakeEffect());

        if (feedbackOverlay) feedbackOverlay.color = new Color(1, 0, 0, 0.5f);
        OnGameFailure?.Invoke();
        Invoke(nameof(ResetBallToStart), restartDelay);
    }

    private void ResetBallToStart()
    {
        _isRestarting = false;
        _isDragging = false;
        transform.localPosition = _originalPanelPos;

        if (playerBall != null && startPoint != null)
        {
            playerBall.position = startPoint.position;
        }

        if (feedbackOverlay && _isGameActive) feedbackOverlay.color = new Color(1, 1, 1, 0.05f);
    }

    private void SetTextAlpha(float alpha)
    {
        if (statusText is Graphic g) g.color = new Color(_originalTextColor.r, _originalTextColor.g, _originalTextColor.b, alpha);
    }

    private void PlaySound(AudioClip clip)
    {
        if (sfxSource && clip) sfxSource.PlayOneShot(clip);
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

    private void WinGame()
    {
        _isGameActive = false;
        UpdateStatus("CLEARED!");
        SetTextAlpha(1f);
        PlaySound(winSound);
        if (feedbackOverlay) feedbackOverlay.color = new Color(0, 1, 0, 0.5f);
        OnGameWin?.Invoke();
        Invoke(nameof(CloseGame), 1.0f);
    }

    private System.Collections.IEnumerator ShakeEffect()
    {
        float elapsed = 0f;
        while (elapsed < 0.2f)
        {
            transform.localPosition = _originalPanelPos + (Vector3)UnityEngine.Random.insideUnitCircle * shakeIntensity;
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.localPosition = _originalPanelPos;
    }


    private void CloseGame()
    {
        gameObject.SetActive(false);
    }
}




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

    [Header("Settings")]
    public float restartDelay = 0.5f;

    public static event Action OnGameWin;
    public static event Action OnGameFailure;

    private bool _isGameActive = false;
    private bool _isRestarting = false;
    private bool _isDragging = false;

    private void OnEnable()
    {
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
    private void OnBallBeginDrag(PointerEventData eventData)
    {
        if (!_isGameActive || _isRestarting) return;

        _isDragging = true;
        if (feedbackOverlay) feedbackOverlay.color = Color.clear;
        Debug.Log("Drag Started!");
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

        Debug.Log("Game Failed! Resetting...");

        if (feedbackOverlay) feedbackOverlay.color = new Color(1, 0, 0, 0.5f);
        OnGameFailure?.Invoke();

        Invoke(nameof(ResetBallToStart), restartDelay);
    }

    private void ResetBallToStart()
    {
        _isRestarting = false;
        _isDragging = false;
        if (playerBall != null && startPoint != null)
        {
            playerBall.position = startPoint.position;
        }

        if (feedbackOverlay && _isGameActive) feedbackOverlay.color = new Color(1, 1, 1, 0.05f);
    }

    private void WinGame()
    {
        _isGameActive = false;
        _isDragging = false;
        if (feedbackOverlay) feedbackOverlay.color = new Color(0, 1, 0, 0.5f);

        Debug.Log("Victory!");
        OnGameWin?.Invoke();

        Invoke(nameof(CloseGame), 0.5f);
    }

    private void CloseGame()
    {
        gameObject.SetActive(false);
    }
}




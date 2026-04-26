using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private GameObject minigameCanvas;
    [SerializeField] private List<GameObject> minigames = new List<GameObject>();
    [SerializeField] private GameObject minigameListCanvas;
    
    // Fatigue System
    [Header("Fatigue System")]
    [SerializeField] private float fatigueIncreaseAmount = 0.1f;
    [SerializeField] private float fatigeDecrementRate = 0.01f;
    [SerializeField] private GameObject fatigueDisplay;
    [SerializeField] private Animator fatigueVFXAnimator;
    private float _fatigue = 0f;
    private PlayerMovement _playerMovement;
    private float originalLerpSpeed = 0f;
    private bool isCurrentlySlowed = false;
    
    private void Start()
    {
        _playerMovement = FindAnyObjectByType<PlayerMovement>();
        originalLerpSpeed = _playerMovement.moveSpeed;
    }
    
    private void OnCollisionEnter2D(Collision2D pickupItem)
    {
        if(pickupItem.gameObject.CompareTag("StudyObject"))
        {
            StartMinigame();
        }
        else if (pickupItem.gameObject.CompareTag("LeisureObject"))
        {
            LeisureAction();
        }
        Destroy(pickupItem.gameObject);

    }

    private void StartMinigame()
    {
        // minigames are children of minigameListCanvas, so we can just randomly activate one of them
        minigames.Clear();
        foreach (Transform minigame in minigameListCanvas.transform)
        {
            minigames.Add(minigame.gameObject);
        }
        int randomIndex = Random.Range(0, minigames.Count);
        minigames[randomIndex].SetActive(true);
    }

    private void LeisureAction()
    {
        float stressIncrementValue = -0.2f * (1f - _fatigue);
        StressMeter.Instance.AddStress(stressIncrementValue);
        _fatigue += fatigueIncreaseAmount;
        _fatigue = Mathf.Clamp(_fatigue, 0f, 1f);
    }
    private void Update()
    {
        // Debug
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fatigueVFXAnimator.SetTrigger("effectin");
        }
        // dubug
        if (_fatigue < 0f) return;
        _fatigue -= fatigeDecrementRate * Time.deltaTime;
        _fatigue = Mathf.Clamp(_fatigue, 0f, 1f);
        bool shouldBeSlowed = _fatigue > 0.2f;
        fatigueDisplay.SetActive(shouldBeSlowed);

        if (shouldBeSlowed != isCurrentlySlowed)
        {
            isCurrentlySlowed = shouldBeSlowed;
            SlowPlayer(isCurrentlySlowed);
        }
    }
    private void SlowPlayer(bool isSlowed)
    {
        if (_playerMovement == null) return;

        if (isSlowed)
        {
            _playerMovement.moveSpeed = originalLerpSpeed / 2.25f;
            fatigueVFXAnimator.SetTrigger("effectin");
        }
        else
        {
            if (originalLerpSpeed == 0f) return;
            _playerMovement.moveSpeed = originalLerpSpeed;
            fatigueVFXAnimator.SetTrigger("effectout");
        }
        
    }
}

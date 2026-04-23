using UnityEngine;
using System.Collections.Generic;
public class MainGameAudioSystem : MonoBehaviour
{
    [Header("Main BGM")]
    [SerializeField] private AudioSource mainBgmSource;
    [SerializeField] private float bgmFadeSpeed = 2f;

    [Header("Player SFX")]
    [SerializeField] private AudioSource walkingSource;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float moveThreshold = 0.01f;

    [Header("Item SFX")]
    [SerializeField] private AudioSource pickupSource;
    [SerializeField] private AudioClip pickupClip;
    [SerializeField] private AudioSource dropSource;
    [SerializeField] private AudioClip dropClip;

    [Header("Mini-Game Detection")]
    [SerializeField] private GameObject[] miniGamePanels;

    private Transform _playerTransform;
    private Vector3 _lastPlayerPos;
    private float _targetBgmVolume;
    private List<GameObject> _knownItems = new List<GameObject>();

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            _playerTransform = playerObj.transform;
            _lastPlayerPos = _playerTransform.position;
        }

        if (mainBgmSource != null) _targetBgmVolume = mainBgmSource.volume;

        if (walkingSource != null) walkingSource.volume = 0;
    }


    void Update()
    {
        bool miniGameIsActive = IsAnyMiniGameOpen();

        HandleBgmFade(miniGameIsActive);
        HandleWalkingSound(miniGameIsActive);
        HandleItemDetection();
    }

    private bool IsAnyMiniGameOpen()
    {
        if (miniGamePanels == null) return false;

        foreach (var panel in miniGamePanels)
        {
            if (panel != null && panel.activeInHierarchy) return true;
        }
        return false;
    }

    private void HandleBgmFade(bool isPaused)
    {
        if (mainBgmSource == null) return;

        float currentTarget = isPaused ? 0 : _targetBgmVolume;
        mainBgmSource.volume = Mathf.MoveTowards(mainBgmSource.volume, currentTarget, Time.deltaTime * bgmFadeSpeed);
    }

    private void HandleWalkingSound(bool isPaused)
    {
        if (_playerTransform == null || walkingSource == null) return;

        float distanceMoved = Vector3.Distance(_playerTransform.position, _lastPlayerPos);
        bool isMoving = distanceMoved > moveThreshold;

        float targetWalkVol = (isMoving && !isPaused) ? 1.0f : 0f;
        walkingSource.volume = Mathf.MoveTowards(walkingSource.volume, targetWalkVol, Time.deltaTime * 10f);

        _lastPlayerPos = _playerTransform.position;
    }

    private void HandleItemDetection()
    {
        GameObject[] currentLeisure = GameObject.FindGameObjectsWithTag("LeisureObject");
        GameObject[] currentStudy = GameObject.FindGameObjectsWithTag("StudyObject");

        CheckForNewDrops(currentLeisure);
        CheckForNewDrops(currentStudy);

        if (_playerTransform != null)
        {
            CheckPickupDistance(currentLeisure);
            CheckPickupDistance(currentStudy);
        }
    }

    private void CheckForNewDrops(GameObject[] currentItems)
    {
        foreach (var item in currentItems)
        {
            if (!_knownItems.Contains(item))
            {
                _knownItems.Add(item);
                if (dropSource && dropClip) dropSource.PlayOneShot(dropClip);
            }
        }

        _knownItems.RemoveAll(item => item == null);
    }

    private void CheckPickupDistance(GameObject[] currentItems)
    {
        foreach (var item in currentItems)
        {
            if (Vector3.Distance(item.transform.position, _playerTransform.position) < 1.2f)
            {
                if (pickupSource && pickupClip) pickupSource.PlayOneShot(pickupClip);

                _knownItems.Remove(item);
                break;
            }
        }
    }
}

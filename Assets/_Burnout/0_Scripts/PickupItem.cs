using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private GameObject minigameCanvas;
    [SerializeField] private List<GameObject> minigames = new List<GameObject>();
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("StudyObject"))
        {
            Destroy(other.gameObject);
            int randomIndex = Random.Range(0, minigames.Count);
            minigames[randomIndex].SetActive(true);
        }
        else if (other.gameObject.CompareTag("LeisureObject"))
        {
            Destroy(other.gameObject);
            StressMeter.Instance.UpdateStress(-0.2f);
        }
    }
}

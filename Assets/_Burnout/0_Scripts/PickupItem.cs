using System;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private GameObject minigameCanvas;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("StudyObject"))
        {
            Destroy(other.gameObject);
            minigameCanvas.SetActive(true);
        }
        else if (other.gameObject.CompareTag("LeisureObject"))
        {
            Destroy(other.gameObject);
            StressMeter.Instance.UpdateStress(-0.2f);
        }
    }
}

using System;
using UnityEngine;

public class BookManager : MonoBehaviour{
    public GameObject prefab;
    public GameObject minigamePanel;

    private Book selectedLeft;
    private int correct =0;
    private bool ended = false;

    private void OnEnable()
    {
        PauseManager.Instance.PauseGame(true);

        // Reset runtime state when this manager is spawned/enabled
        correct =0;
        selectedLeft = null;
        ended = false;

        if (minigamePanel != null)
            minigamePanel.SetActive(true);

        // Re-enable all book objects under this manager
        var books = GetComponentsInChildren<Book>(true);
        foreach (var b in books)
            b.gameObject.SetActive(true);
    }

    public void SelectBook(Book book)
    {
        if (ended) return;

        if (book.isLeft)
        {
            selectedLeft = book;
            return;
        }

        if (selectedLeft == null) return;

        if (selectedLeft.bookID == book.bookID)
        {
            correct++;
            selectedLeft.gameObject.SetActive(false);
            book.gameObject.SetActive(false);
        }
        else {
            Debug.Log("Wrong!");
        }

        selectedLeft = null;

        if (correct >=3)
            EndMinigame();
    }

    private void EndMinigame()
    {
        if (ended) return;
        ended = true;

        StressMeter.Instance.AddStress(0.2f);
        GPAMeter.Instance.UpdateGPA(0.1f);
        PauseManager.Instance.PauseGame(false);
        Time.timeScale =1f;

        if (minigamePanel != null)
            minigamePanel.SetActive(false);

        Instantiate(prefab, transform.parent);
        Destroy(gameObject);
    }
}

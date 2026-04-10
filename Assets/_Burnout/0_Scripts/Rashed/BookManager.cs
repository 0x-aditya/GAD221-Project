using UnityEngine;

public class BookManager : MonoBehaviour
{
    Book selectedLeft;
    int correct = 0;

    public GameObject minigamePanel;

    public void SelectBook(Book book)
    {
        if (book.isLeft)
        {
            selectedLeft = book;
        }
        else
        {
            if (selectedLeft == null) return;

            if (selectedLeft.bookID == book.bookID)
            {
                correct++;

                selectedLeft.gameObject.SetActive(false);
                book.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Wrong!");
            }

            selectedLeft = null;
        }

        if (correct == 3)
        {
            EndMinigame();
        }
    }

    void EndMinigame()
    {
        minigamePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}

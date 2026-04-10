using UnityEngine;

public class Book : MonoBehaviour
{
    public int bookID;   // number (0,1,2,3)
    public bool isLeft;  // true = left side

    public void ClickBook()
    {
        FindObjectOfType<BookManager>().SelectBook(this);
    }
}
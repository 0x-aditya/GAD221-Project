using UnityEngine;
using UnityEngine.UI;

public class OddOneManager : MonoBehaviour
{
    public Button[] buttons;
    public Sprite commonSprite;
    public Sprite oddSprite;

    int oddIndex;

    void OnEnable()
    {
        SetupGame();
    }

    void SetupGame()
    {
        oddIndex = Random.Range(0, buttons.Length);

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == oddIndex)
                buttons[i].GetComponent<Image>().sprite = oddSprite;
            else
                buttons[i].GetComponent<Image>().sprite = commonSprite;

            int index = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => CheckAnswer(index));
        }
    }
    void CheckAnswer(int index)
    {
        if (index == oddIndex)
        {
            Debug.Log("Correct");

            // Close minigame
            gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("Wrong");

            // Do NOT close
            // (optional: give feedback)
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class OddOneManager : MonoBehaviour
{
    public GameObject prefab;
    public Button[] buttons;
    public Sprite commonSprite;
    public Sprite oddSprite;

    int oddIndex;

    void OnEnable()
    {
        PauseManager.Instance.PauseGame(true);
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
            StressMeter.Instance.AddStress(0.2f);
            GPAMeter.Instance.UpdateGPA(0.1f);
            PauseManager.Instance.PauseGame(false);
            Destroy(gameObject);
            Instantiate(prefab, transform.parent);
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
using TMPro;
using UnityEngine;

public class FlipBitsManager : ScriptLibrary.Singletons.Singleton<FlipBitsManager>
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject bit1;
    [SerializeField] private GameObject bit2;
    
    [SerializeField] private TextMeshProUGUI finalBit1;
    [SerializeField] private TextMeshProUGUI finalBit2;
    void Start()
    {
        if (bit1 == null || bit2 == null || finalBit1 == null || finalBit2 == null)
        {
            Debug.LogError("GameObjects not assigned in the inspector (FlipBitsManager)");
        }
        PauseManager.Instance.PauseGame(true);
    }
    void OnEnable()
    {
    }

    void WinGame()
    {
        Debug.Log("You win!");
        gameObject.SetActive(false);
        StressMeter.Instance.AddStress(0.2f);
        GPAMeter.Instance.UpdateGPA(0.1f);
        PauseManager.Instance.PauseGame(false);
        Instantiate(prefab, transform.parent);
        Destroy(gameObject);
    }

    void Update()
    {
        if (bit1.activeSelf && finalBit1.text == "1" && bit2.activeSelf && finalBit2.text == "1")
        {
            WinGame();
        }
        if (bit1.activeSelf && finalBit1.text == "1" && !bit2.activeSelf && finalBit2.text == "0")
        {
            WinGame();
        }
        if (!bit1.activeSelf && finalBit1.text == "0" && bit2.activeSelf && finalBit2.text == "1")
        {
            WinGame();
        }
        if (!bit1.activeSelf && finalBit1.text == "0" && !bit2.activeSelf && finalBit2.text == "0")
        {
            WinGame();
        }
    }
}

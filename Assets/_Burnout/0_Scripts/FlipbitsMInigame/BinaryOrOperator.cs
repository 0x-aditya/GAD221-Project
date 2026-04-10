using UnityEngine;

public class BinaryOrOperator : MonoBehaviour
{
    [SerializeField] GameObject bit1;
    [SerializeField] GameObject bit2;
    [SerializeField] GameObject resultBit;
    void Start()
    {

        if (bit1 == null || bit2 == null || resultBit == null)
        {
            Debug.LogError("GameObjects not assigned in the inspector (BinaryOrOperator)");
        }
    }

    void Update()
    {
        if (bit1.activeSelf || bit2.activeSelf)
        {
            resultBit.SetActive(true);
        }
        else
        {
            resultBit.SetActive(false);
        }
    }
}

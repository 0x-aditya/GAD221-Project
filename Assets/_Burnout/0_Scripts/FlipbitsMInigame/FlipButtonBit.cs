using UnityEngine;

public class FlipButtonBit : MonoBehaviour
{
    [SerializeField] private GameObject bit;

    public void flipBit()
    {
        bit.SetActive(!bit.activeSelf);
    }
}

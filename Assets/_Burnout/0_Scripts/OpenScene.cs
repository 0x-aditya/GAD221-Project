using UnityEngine;

public class OpenScene : MonoBehaviour
{
    public void SwitchScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}

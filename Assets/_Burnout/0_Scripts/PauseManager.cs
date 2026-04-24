using UnityEngine;

public class PauseManager : ScriptLibrary.Singletons.Singleton<PauseManager>
{
    [SerializeField] private GameObject[] pauseObjects;

    public void PauseGame(bool isPaused)
    {
        foreach (GameObject obj in pauseObjects)
        {
            obj.SetActive(!isPaused);   
        }
        var leisureObjects = GameObject.FindGameObjectsWithTag("LeisureObject");
        var studyObjects = GameObject.FindGameObjectsWithTag("StudyObject");
        foreach (GameObject obj in leisureObjects)
        {
            obj.SetActive(!isPaused);
        }
        foreach (GameObject obj in studyObjects)
        {
            obj.SetActive(!isPaused);
        }
    }

}

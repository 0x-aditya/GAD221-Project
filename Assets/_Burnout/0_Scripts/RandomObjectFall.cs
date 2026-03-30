using UnityEngine;
using System.Collections;


public class RandomObjectFall : MonoBehaviour
{
    [SerializeField] private ObjectDetails[] objectDetails;
    [SerializeField] private Transform fallPosX1;
    [SerializeField] private Transform fallPosX2;
    [SerializeField] private float spawnInterval = 0.5f;

    public static float TotalGameTime = 10f;
    
    void Start()
    {
        StartCoroutine(FallObjects());
    }

    private IEnumerator FallObjects()
    {
        float elapsedTime = 0f;
        while (elapsedTime < TotalGameTime)
        {
            foreach (ObjectDetails details in objectDetails)
            {
                if (Random.value <= details.fallChance)
                {
                    int randomIndex = Random.Range(0, details.fallObjectPrefabs.Length);
                    GameObject prefab = details.fallObjectPrefabs[randomIndex];
                    
                    float randomXPos = Random.Range(fallPosX1.position.x, fallPosX2.position.x);
                    Vector3 spawnPos = new Vector3(randomXPos, fallPosX1.position.y, 0f);

                    Instantiate(prefab, spawnPos, Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(spawnInterval);
            elapsedTime += spawnInterval;
        }
    }
}

[System.Serializable]
public class ObjectDetails
{
    public GameObject[] fallObjectPrefabs;
    [Range(0f,1f)] public float fallChance;
}

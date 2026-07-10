using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private GameObject spawnObject;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private float timer;
    [SerializeField] private float timeBetweenSpawns;

    public float speedMultiplier;

    private void Update()
    {
        speedMultiplier += Time.deltaTime * 0.1f;
        timer += Time.deltaTime;

        if(timer > timeBetweenSpawns)
        {
            timer = 0;
            int randomNumber = Random.Range(0, 3);
            Instantiate(spawnObject, spawnPoints[randomNumber].transform.position, Quaternion.identity);
        }
    }
}

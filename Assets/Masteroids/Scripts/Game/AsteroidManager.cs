using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] private int maxAsteroids;
    [SerializeField] public List<Asteroid> Asteroids;
    [SerializeField] private GameObject asteroidPrefab;
    [Range(0,5)]
    [SerializeField] private float averageTimeToSpanAsteroids;
    [Range(0, 20)]
    public int difficulty;
    [Header("Asteroid Spawn Ranges")]
    public float minimumSpawnXRange;
    public float maximumSpawnXRange;
    [Header("Asteroid Settings")]
    [SerializeField] private float maxSpinPerSecond;
    [SerializeField] private float minSpinPerSecond;
    [SerializeField] private float angleRandomness;
    void Start()
    {
        StartCoroutine("RollForAsteroids");
        averageTimeToSpanAsteroids = 3/(float)difficulty;
    }
    private void Update()
    {
        if (GameManager.currentGameState == GameManager.GameState.Game)
        {
            IncreaseDifficulty();
        }
    }
    private void IncreaseDifficulty()
    {
        averageTimeToSpanAsteroids *= 1 - (difficulty * Time.deltaTime * 0.01f);
    }
    public Asteroid MakeAsteroid()
    {
        Vector3 newPosition = new Vector3(Random.Range(minimumSpawnXRange, maximumSpawnXRange), 6, 0);
        Asteroid newAsteroid = Instantiate(asteroidPrefab, newPosition, Quaternion.identity, transform).GetComponent<Asteroid>();
        newAsteroid.spinPerSecond = Random.Range(minSpinPerSecond, maxSpinPerSecond);
        newAsteroid.angle = Random.Range(-3 * angleRandomness, 3 * angleRandomness);
        if ((newPosition.x < -7 && newAsteroid.angle < -1) || (newPosition.x > 7 && newAsteroid.angle < 1))
        {
            newAsteroid.angle *= -1;
        }
        return newAsteroid;
    }
    private IEnumerator RollForAsteroids()
    {
        if (GameManager.currentGameState != GameManager.GameState.Game)
        {
            yield return new WaitForSeconds(0.1f);
        }
        else
        {
            yield return new WaitForSeconds(Random.Range(averageTimeToSpanAsteroids * 0.8f, averageTimeToSpanAsteroids * 0.12f));
            if (Asteroids.Count < maxAsteroids)
            {
                Asteroids.Add(MakeAsteroid());
                if (Asteroids[Asteroids.Count - 1].transform.position.x < -7 && Asteroids[Asteroids.Count - 1])
                {

                }
            }
        }
        StartCoroutine("RollForAsteroids");
    }
}

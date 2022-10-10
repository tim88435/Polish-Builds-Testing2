using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] private int maxAsteroids;
    public int asteroidCount = 0;
    //[SerializeField] public List<Asteroid> Asteroids;
    [SerializeField] private GameObject asteroidPrefab;
    [Range(0.02f,5)]
    [SerializeField] private float timeToSpawnAsteroids;
    [Range(0, 20)]
    public int difficulty;
    private float nextSpawnTime;
    [Header("Asteroid Spawn Ranges")]
    public float minimumSpawnXRange;
    public float maximumSpawnXRange;
    [Header("Asteroid Settings")]
    [SerializeField] private float maxSpinPerSecond;
    [SerializeField] private float minSpinPerSecond;
    [SerializeField] private float angleRandomness;
    void Start()
    {
        timeToSpawnAsteroids = 3/(float)difficulty;
    }
    private void Update()
    {
        if (GameManager.currentGameState == GameManager.GameState.Game)
        {
            IncreaseDifficulty();
            RollForAsteroids();
        }
    }
    private void IncreaseDifficulty()
    {
        timeToSpawnAsteroids *= 1 - (difficulty * Time.deltaTime * 0.01f);
        timeToSpawnAsteroids = Mathf.Clamp(timeToSpawnAsteroids, 0.02f, Mathf.Infinity);
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
    private void RollForAsteroids()
    {
        if (GameManager.currentGameState != GameManager.GameState.Game)
        {
            return;
        }
        if (asteroidCount > maxAsteroids)
        {
            return;
        }
        while (GameManager.gameTime > nextSpawnTime)
        {
            MakeAsteroid();
            asteroidCount++;
            nextSpawnTime += timeToSpawnAsteroids;
        }
        
    }
}

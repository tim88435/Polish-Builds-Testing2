using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TestScript
{
    private GameManager gameManager;
    private PlayerMovement playerMovement;
    private PlayerShoot playerShoot;
    private AsteroidManager asteroidManager;
#if UNITY_EDITOR
    // A Test behaves as an ordinary method
    [SetUp]
    public void SetUp()
    {
        gameManager = new GameObject().AddComponent<GameManager>();
        gameManager.playerprefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Masteroids/Prefabs/Player Prefab.prefab", typeof(GameObject));
        playerShoot = new GameObject().AddComponent<PlayerShoot>();
        asteroidManager = new GameObject().AddComponent<AsteroidManager>();
        gameManager.highScoreTextGradient = new Gradient();
        gameManager.scoreText = new GameObject().AddComponent<UnityEngine.UI.Text>();
        gameManager.highScoreHUDText = new GameObject().AddComponent<UnityEngine.UI.Text>();
        asteroidManager.asteroidPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Masteroids/Prefabs/Asteroid.prefab", typeof(GameObject));
        playerShoot.bulletPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Masteroids/Prefabs/Bullet.prefab", typeof(GameObject));
        playerShoot.asteroidManager = asteroidManager;
        asteroidManager.source = asteroidManager.gameObject.AddComponent<AudioSource>();
    }
    [TearDown]
    public void TearDown()
    {
        Object.Destroy(gameManager.highScoreHUDText.gameObject);
        Object.Destroy(gameManager.scoreText.gameObject);
        Object.Destroy(gameManager.gameObject);
        if (GameManager.player != null)
        {
            Object.Destroy(GameManager.player.gameObject);
        }
        Object.Destroy(playerShoot.gameObject);
        Object.Destroy(asteroidManager.gameObject);
    }
    [Test]
    public void SaveAndLoadHighScores()
    {
        gameManager.instanceHighScoreMax = 10;
        List<GameManager.HighScore> actualScores = new List<GameManager.HighScore>();
        if (System.IO.File.Exists($"{Application.dataPath}/HighScores.txt"))
        {
            gameManager.LoadScores();
            for (int i = 0; i < gameManager.HighScoreList.Count; i++)
            {
                actualScores.Add(gameManager.HighScoreList[i]);
            }
        }
        List<GameManager.HighScore> testScores = new List<GameManager.HighScore>();
        gameManager.ClearHighScores();
        for (int i = 0; i < 5; i++)
        {
            gameManager.NewHighScore("TestScore", -1);
            testScores.Add(new GameManager.HighScore("TestScore", -1));
        }
        bool works = true;
        for (int i = 0; i < 5; i++)
        {
            if (!((gameManager.HighScoreList[i].name == testScores[i].name) || (gameManager.HighScoreList[i].score == testScores[i].score)))
            {
                works = false;
            }
        }
        if (actualScores.Count <= 0)
        {
            gameManager.LoadScores();
            gameManager.HighScoreList = actualScores;
            gameManager.SaveScores();
        }
        Assert.IsTrue(works);
    }
    [UnityTest]
    public IEnumerator MoveRandomDirection()
    {
        yield return null;
        Vector3 currentPosition = new Vector3();
        currentPosition = GameManager.player.transform.position;
        Vector3 direction = new Vector3(Random.Range(1f, 0f), Random.Range(1f, 0f), 0f);
        GameManager.player.MovePlayer(direction);
        Assert.AreEqual(currentPosition + direction.normalized * Time.deltaTime * GameManager.player.speed, GameManager.player.transform.position);
    }
    [UnityTest]
    public IEnumerator ShootBullet()
    {
        yield return null;
        int oldBulletCount = new int();
        oldBulletCount = playerShoot.transform.childCount;
        playerShoot.SpawnBullet();
        Assert.AreEqual(oldBulletCount+1, playerShoot.transform.childCount);
    }
    [UnityTest]
    public IEnumerator BulletHitsEnemy()
    {
        yield return null;
        Bullet bullet = playerShoot.SpawnBullet();
        Asteroid asteroid = asteroidManager.MakeAsteroid();
        asteroid.transform.position = new Vector3(5, 5, 0);
        bullet.transform.position = new Vector3(5, 5, 0);
        float oldScore = GameManager.score;
        int oldAsteroidCount = asteroidManager.transform.childCount;
        int oldBulletCount = playerShoot.transform.childCount;
        yield return new WaitForFixedUpdate();
        //yield return new WaitForFixedUpdate();
        yield return null;
        bool works = true;
        if (bullet != null)
        {
            works = false;
        }
        if (asteroid != null)
        {
            works = false;
        }
        if (oldScore == GameManager.score)
        {
            works = false;
        }
        if (oldAsteroidCount == asteroidManager.transform.childCount)
        {
            works = false;
        }
        if (oldBulletCount == playerShoot.transform.childCount)
        {
            works = false;
        }
        Assert.IsTrue(works);
    }
    [UnityTest]
    public IEnumerator BulletHitsPlayer()
    {
        yield return null;
        Asteroid asteroid = asteroidManager.MakeAsteroid();
        asteroid.transform.position = new Vector3(0, 0, 0);
        int frameNumber = 0;
        bool works = true;
        while (true)
        {
            yield return null;
            frameNumber++;
            if (GameManager.player.CheckForCollision())
            {
                //Debug.Log(frameNumber);
                break;
            }
            else if (frameNumber > 100)
            {
                works = false;
                break;
            }
        }
        Assert.IsTrue(works);
    }
    [UnityTest]
    public IEnumerator RestatsGame()
    {
        yield return null;
        Asteroid asteroid = asteroidManager.MakeAsteroid();
        asteroid.transform.position = new Vector3(0, 0, 0);
        int frameNumber = 0;
        bool works = true;
        while (true)
        {
            yield return null;
            frameNumber++;
            if (GameManager.currentGameState == GameManager.GameState.EndGame)
            {
                //Debug.Log(frameNumber);
                break;
            }
            else if (frameNumber > 100)
            {
                works = false;
                break;
            }
        }
        Assert.IsFalse(!works);
    }
#endif
}

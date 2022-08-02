using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TestScript
{
#if UNITY_EDITOR
    // A Test behaves as an ordinary method
    [Test]
    public void TestScriptSimplePasses()
    {
        
        Assert.IsTrue(true);
        // Use the Assert class to test conditions
    }
    [Test]
    public void SaveAndLoadHighScores()
    {
        GameObject gameObject = new GameObject();
        GameManager gameManager = gameObject.AddComponent<GameManager>();
        gameManager.instanceHighScoreMax = 10;
        gameManager.LoadScores();
        List<GameManager.HighScore> actualScores = new List<GameManager.HighScore>();
        for (int i = 0; i < gameManager.HighScoreList.Count; i++)
        {
            actualScores.Add(gameManager.HighScoreList[i]);
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
        gameManager.LoadScores();
        gameManager.HighScoreList = actualScores;
        gameManager.SaveScores();
        GameObject.Destroy(gameObject);
        Assert.IsTrue(works);
    }
    [UnityTest]
    public IEnumerator MoveRandomDirection()
    {
        //GameObject gameObject = new GameObject();
        //GameManager gameManager = gameObject.AddComponent<GameManager>();
        SceneManager.LoadScene(0);
        yield return null;
        Vector3 currentPosition = new Vector3();
        currentPosition = GameManager.player.transform.position;
        Vector3 direction = new Vector3(Random.Range(1f, 0f), Random.Range(1f, 0f), 0f);
        GameManager.player.MovePlayer(direction);
        Assert.IsTrue(GameManager.player.transform.position == currentPosition + direction.normalized * Time.deltaTime * GameManager.player.speed);
    }
    [UnityTest]
    public IEnumerator ShootBullet()
    {
        SceneManager.LoadScene(0);
        yield return null;
        PlayerShoot bulletHandler = GameObject.Find("Bullets").GetComponent<PlayerShoot>();
        int oldBulletCount = new int();
        oldBulletCount = bulletHandler.transform.childCount;
        bulletHandler.SpawnBullet();
        Assert.IsTrue(oldBulletCount+1 == bulletHandler.transform.childCount);
    }
    [UnityTest]
    public IEnumerator BulletHitsEnemy()
    {
        SceneManager.LoadScene(0);
        yield return null;
        PlayerShoot bulletHandler = GameObject.Find("Bullets").GetComponent<PlayerShoot>();
        Bullet bullet = bulletHandler.SpawnBullet();
        AsteroidManager asteroidHandler = GameObject.Find("Asteroids").GetComponent<AsteroidManager>();
        Asteroid asteroid = asteroidHandler.MakeAsteroid();
        asteroid.transform.position = new Vector3(5, 5, 0);
        bullet.transform.position = new Vector3(5, 5, 0);
        float oldScore = new float();
        int oldAsteroidCount = new int();
        int oldBulletCount = new int();
        oldScore = GameManager.score;
        oldAsteroidCount = asteroidHandler.transform.childCount;
        oldBulletCount = bulletHandler.transform.childCount;
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
        if (oldAsteroidCount == asteroidHandler.transform.childCount)
        {
            works = false;
        }
        if (oldBulletCount == bulletHandler.transform.childCount)
        {
            works = false;
        }
        Assert.IsTrue(works);
    }
    [UnityTest]
    public IEnumerator BulletHitsPlayer()
    {
        SceneManager.LoadScene(0);
        yield return null;
        AsteroidManager asteroidHandler = GameObject.Find("Asteroids").GetComponent<AsteroidManager>();
        Asteroid asteroid = asteroidHandler.MakeAsteroid();
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
        SceneManager.LoadScene(0);
        yield return null;
        AsteroidManager asteroidHandler = GameObject.Find("Asteroids").GetComponent<AsteroidManager>();
        Asteroid asteroid = asteroidHandler.MakeAsteroid();
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
        Assert.IsTrue(works);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
        Assert.IsTrue(true);
    }
#endif
}

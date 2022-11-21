using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float spinPerSecond;
    public float angle;
    private AsteroidManager manager;
    private void Start()
    {
        manager = transform.parent.GetComponent<AsteroidManager>();
    }
    private void Update()
    {
        if (GameManager.currentGameState == GameManager.GameState.Game)
        {
            moveAsteroid();
        }
        SheckIfOutOfBounds();
    }
    private void moveAsteroid()
    {
        transform.Rotate(0, 0, spinPerSecond);
        transform.Translate(angle * Time.deltaTime, -Time.deltaTime, 0, Space.World);
    }
    private void SheckIfOutOfBounds()
    {
        if (transform.position.x > 10)
        {
            transform.Translate(-20, 0, 0, Space.World);
        }
        if (transform.position.x < -10)
        {
            transform.Translate(20, 0, 0, Space.World);
        }
        if (transform.position.y < -6)
        {
            DeleteAsteroid();
        }
    }
    public void DeleteAsteroid()
    {
        //manager.Asteroids.Remove(this);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (manager == null)
        {
            transform.parent.GetComponent<AsteroidManager>().asteroidCount--;
            return;
        }
        manager.asteroidCount--;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private List<Collider2D> collidedObjects = new List<Collider2D>();
    public CircleCollider2D circleCollider2D;
    public float angle;
    public Vector2 direction;
    public float bulletSpeed;
    private PlayerShoot manager;
    private void Start()
    {
        manager = transform.parent.GetComponent<PlayerShoot>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        direction.Normalize();
    }

    private void Update()
    {
        if (GameManager.currentGameState == GameManager.GameState.Game)
        {
            MoveBullet();
            CheckForCollision();
        }
        SheckIfOutOfBounds();
    }
    private void MoveBullet()
    {
        transform.Translate(direction * Time.deltaTime * bulletSpeed, Space.World);
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
        if (transform.position.y > 6)
        {
            manager.Bullets.Remove(this);
            Destroy(gameObject);
        }
    }
    public void CheckForCollision()
    {
        if (Physics2D.OverlapCollider(circleCollider2D, new ContactFilter2D().NoFilter(), collidedObjects) > 0)
        {
            for (int i = 0; i < collidedObjects.Count; i++)
            {
                if (collidedObjects[i].TryGetComponent(out Asteroid asteroid))
                {
                    asteroid.DeleteAsteroid();
                    manager.asteroidManager.PlayAsteroidDestoryed();
                    GameManager.score+= 10;
                    DeleteBullet();
                }
            }
        }
    }
    public void DeleteBullet()
    {
        manager.Bullets.Remove(this);
        Destroy(gameObject);
    }
}

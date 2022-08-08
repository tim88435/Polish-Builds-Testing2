using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float timeSinceLastShot;
    public List<Bullet> Bullets;
    public GameObject bulletPrefab;
    [Range(0.005f, Mathf.Infinity)]
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float bulletSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.currentGameState == GameManager.GameState.Game)
        {
            if ((Input.GetButton("Jump") || Input.GetKey(KeyCode.Mouse0)) && (GameManager.gameTime > (timeBetweenShots + timeSinceLastShot)))
            {
                SpawnBullet();
                timeSinceLastShot = GameManager.gameTime;
            }
        }
    }
    /*public void SpawnBullet()
    {
        GameObject newBulletObject = Instantiate(bulletPrefab, GameManager.player.transform.position, Quaternion.identity, transform);
        Bullet newBullet = newBulletObject.AddComponent<Bullet>();
        newBullet.angle = Input.GetAxisRaw("Horizontal");
        newBullet.bulletSpeed = bulletSpeed;
        Bullets.Add(newBullet);
    }*/
    public Bullet SpawnBullet()
    {
        GameObject newBulletObject = Instantiate(bulletPrefab, GameManager.player.transform.position, Quaternion.identity, transform);
        Bullet newBullet = newBulletObject.AddComponent<Bullet>();
        newBullet.direction = - newBullet.transform.position + Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(/*-(Vector2)newBullet.transform.position + */new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height));
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        newBullet.bulletSpeed = bulletSpeed;
        Bullets.Add(newBullet);
        return newBullet;
    }
}

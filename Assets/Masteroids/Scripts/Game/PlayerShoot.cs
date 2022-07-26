using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private float timeSinceLastShot;
    public List<Bullet> Bullets = new List<Bullet>();
    public GameObject bulletPrefab;
    [Range(0.005f, Mathf.Infinity)]
    [SerializeField] private float timeBetweenShots;
    public AsteroidManager asteroidManager;
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
#if UNITY_EDITOR
            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)) && (GameManager.gameTime > (timeBetweenShots + timeSinceLastShot)))
#else
            if ((Input.GetAxisRaw("P2 Hori") != 0  || Input.GetAxisRaw("P2 Verti") != 0) && (GameManager.gameTime > (timeBetweenShots + timeSinceLastShot)))
#endif
            {
                SpawnBullet();
                timeSinceLastShot = GameManager.gameTime;
            }
        }
    }
    public Bullet SpawnBullet()
    {
        GameObject newBulletObject = Instantiate(bulletPrefab, GameManager.player.transform.position, Quaternion.identity, transform);
        Bullet newBullet = newBulletObject.AddComponent<Bullet>();
#if UNITY_EDITOR
        //newBullet.direction = - newBullet.transform.position + Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newBullet.direction = new Vector2(Input.GetKey(KeyCode.RightArrow) ? 1 : Input.GetKey(KeyCode.LeftArrow) ? -1 : 0, Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0);
#else
        newBullet.direction = new Vector2(Input.GetAxisRaw("P2 Hori"), Input.GetAxisRaw("P2 Verti"));
#endif
        //Debug.Log(/*-(Vector2)newBullet.transform.position + */new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height));
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        newBullet.bulletSpeed = bulletSpeed;
        Bullets.Add(newBullet);
        return newBullet;
    }
}

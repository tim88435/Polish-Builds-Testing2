using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    [SerializeField] List<Collider2D> collidedObjects = new List<Collider2D>();
    private Collider2D playerCollider;
    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.currentGameState == GameManager.GameState.Game)
        {
            MovePlayer(new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0));
        }
        if (CheckForCollision())
        {
            GameManager.currentGameState = GameManager.GameState.EndGame;
        }
        CheckifOutOfBounds();
    }
    private void MovePlayer()
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        transform.Translate(direction.normalized * Time.deltaTime * speed);
    }
    public void MovePlayer(Vector3 direction)
    {
        transform.Translate(direction.normalized * Time.deltaTime * speed);
    }
    public bool CheckForCollision()
    {
        if (Physics2D.OverlapCollider(playerCollider, new ContactFilter2D().NoFilter(), collidedObjects) > 0)
        {
            for (int i = 0; i < collidedObjects.Count; i++)
            {
                if (collidedObjects[i].TryGetComponent<Asteroid>(out Asteroid asteroid))
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void CheckifOutOfBounds()
    {
        if (transform.position.x < -8.5)
        {
            transform.Translate(Time.deltaTime * speed, 0, 0);
        }
        else if (transform.position.x > 8.5)
        {
            transform.Translate(-Time.deltaTime * speed, 0, 0);
        }
        if (transform.position.y < -5)
        {
            transform.Translate(0, Time.deltaTime * speed, 0);
        }
        else if (transform.position.y > 5)
        {
            transform.Translate(0, -Time.deltaTime * speed, 0);
        }
    }
}
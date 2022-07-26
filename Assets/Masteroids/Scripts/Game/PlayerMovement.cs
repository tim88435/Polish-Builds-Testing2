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
            MovePlayer(new Vector3(Input.GetAxisRaw("P1 Hori"), Input.GetAxisRaw("P1 Verti"), 0));
        }
        if (CheckForCollision())
        {
            GameManager.currentGameState = GameManager.GameState.EndGame;
        }
        CheckifOutOfBounds();
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
                if (collidedObjects[i].TryGetComponent(out Asteroid _))
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void CheckifOutOfBounds()
    {
        if (transform.position.x < -10)
        {
            transform.Translate(20, 0, 0, Space.World);
        }
        else if (transform.position.x > 10)
        {
            transform.Translate(-20, 0, 0, Space.World);
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

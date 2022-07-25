using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float time;
    public static float score;

    public static GameObject player;
    void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not assigned");
        }
        time = 0;
    }

    void Update()
    {
        time = time + Time.deltaTime;
        Debug.Log(time);
    }
    void FixedUpdate()
    {
        
    }
}

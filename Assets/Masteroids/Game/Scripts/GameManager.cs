using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float time;
    float score;
    public static GameObject player;
    void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("PLAYER NOT ASSINGED");
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

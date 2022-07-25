using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideAsteroidManager : MonoBehaviour
{
    [SerializeField] bool reverse;
    void Update()
    {
        transform.Rotate(0, 0, (reverse ? -2 : 2) * Time.deltaTime);
    }
}

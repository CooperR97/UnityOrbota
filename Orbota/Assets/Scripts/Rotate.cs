using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    public GameObject windmill;
    public int speed;

    void Update()
    {
        windmill.transform.Rotate(0, 0, speed);
    }
}

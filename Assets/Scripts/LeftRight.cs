using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRight : MonoBehaviour
{
    public float speed = 0.07f;
    public float direction = 1f;
    float time = 1f;

    void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time % 4 >= 2f) transform.Translate(new Vector3(-direction, 0f, 0f).normalized * speed);
        else transform.Translate(new Vector3(direction, 0f, 0f).normalized * speed);
    }
}
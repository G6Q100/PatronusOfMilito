using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : UnityEngine.MonoBehaviour
{
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;
        transform.position = transform.position + Vector3.back * 3;
    }
}

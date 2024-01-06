using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryOnLoad : MonoBehaviour
{
    private bool original = false;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("BackgroundMusic").Length > 1 && original == false)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        original = true;

    }
}

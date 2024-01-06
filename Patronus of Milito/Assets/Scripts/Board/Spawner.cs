using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject king;

    public GameObject[] enemies;

    float speed = 3.5f;

    float spawnCD;

    // Update is called once per frame
    void Update()
    {
        spawnCD += Time.deltaTime;

        if (GameManager.instance.score < 150)
        {
            speed = 3.5f;
        }
        else if (GameManager.instance.score < 250)
        {
            speed = 3.2f;
        }
        else if (GameManager.instance.score < 350)
        {
            speed = 3f;
        }
        else if (GameManager.instance.score < 500)
        {
            speed = 2.8f;
        }
        else if (GameManager.instance.score < 800)
        {
            speed = 2.5f;
        }
        else if (GameManager.instance.score < 1500)
        {
            speed = 2.1f;
        }
        else if (GameManager.instance.score < 2000)
        {
            speed = 2f;
        }
        else if (GameManager.instance.score < 2500)
        {
            speed = 1.8f;
        }
        else if (GameManager.instance.score < 3500)
        {
            speed = 1.5f;
        }
        else if (GameManager.instance.score < 5000)
        {
            speed = 1.3f;
        }
        else
        {
            speed = 1;
        }


        if (spawnCD > speed)
        {
            spawnCD = Random.Range(-0.3f,0); 
            if (king.activeInHierarchy)
            {
                SpawnEnemy();
            }
        }
    }

    public void SpawnEnemy()
    {
        int randPos = Random.Range(1, 5);

        int randEnemy = Random.Range(0, enemies.Length);

        if(GameManager.instance.score < 150)
        {
            randEnemy = 0;
        }
        else if (GameManager.instance.score < 500)
        {
            randEnemy = Mathf.Clamp(randEnemy, 0, 3);
        }
        else if (GameManager.instance.score < 750)
        {
            randEnemy = Mathf.Clamp(randEnemy, 0, 4);
        }
        else if (GameManager.instance.score < 1000)
        {
            randEnemy = Mathf.Clamp(randEnemy, 0, 5);
        }
        else if (GameManager.instance.score < 1250)
        {
            randEnemy = Mathf.Clamp(randEnemy, 0, 6);
        }
        else if (GameManager.instance.score < 1500)
        {
            randEnemy = Mathf.Clamp(randEnemy, 0, 9);
        }
        else if (GameManager.instance.score < 2000)
        {
            randEnemy = Mathf.Clamp(randEnemy, 0, 20);
        }
        else if (GameManager.instance.score < 3000)
        {
            randEnemy = Mathf.Clamp(randEnemy, 0, 22);
        }
        else if (GameManager.instance.score < 4200)
        {
            randEnemy = Mathf.Clamp(randEnemy, 0, 23);
        }

        switch (randPos)
        {
            case 1:
                randPos = Random.Range(-12 , 12);
                Instantiate(enemies[randEnemy], new Vector3(randPos, 8 , -1), Quaternion.identity);
                break;
            case 2:
                randPos = Random.Range(-12, 12);
                Instantiate(enemies[randEnemy], new Vector3(randPos, -8, -1), Quaternion.identity);
                break;
            case 3:
                randPos = Random.Range(-8, 8);
                Instantiate(enemies[randEnemy], new Vector3(12, randPos, -1), Quaternion.identity);
                break;
            case 4:
                randPos = Random.Range(-8, 8);
                Instantiate(enemies[randEnemy], new Vector3(12, randPos, -1), Quaternion.identity);
                break;
        }
    }
}

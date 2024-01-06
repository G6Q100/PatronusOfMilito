using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour
{
    float addCoinCD;

    private void Update()
    {
        addCoinCD += Time.deltaTime;

        if (addCoinCD > 1)
        {
            addCoinCD = 0;
            if(GameManager.instance.score < 150)
                GameManager.instance.gold += 5;
            else if (GameManager.instance.score < 300)
                GameManager.instance.gold += 6;
            else if (GameManager.instance.score < 500)
                GameManager.instance.gold += 8;
            else
                GameManager.instance.gold += 10;

            GameManager.instance.score++;
            GameManager.instance.goldText.text = GameManager.instance.gold.ToString();
            GameManager.instance.scoreText.text = GameManager.instance.score.ToString();
            //goldManager.scoreText.text = "Score: " + goldManager.score;
        }
    }
}

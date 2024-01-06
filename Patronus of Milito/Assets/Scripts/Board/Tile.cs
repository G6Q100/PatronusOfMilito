using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : UnityEngine.MonoBehaviour
{
    public bool isPlaced, canPlaced, isFriend, isKing, isBlocked;

    public int score;

    public SpriteRenderer rend;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (canPlaced)
        {
            if (isPlaced)
            {
                if (!isFriend)
                    rend.color = new Color32(0, 0, 255, 50);
                else
                    rend.color = new Color32(255, 0, 0, 50);
            }
            else
            {
                rend.color = new Color32(0, 0, 0, 50);
            }
        }
        else
        {
            rend.color = new Color32(0, 0, 0, 0);
        }      
    }
}

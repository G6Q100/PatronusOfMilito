using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rider : MonoBehaviour
{
    float moveCD, maxCD = 3f, fastCD = 0.1f;

    public GameObject[] tiles;
    public Tile[] tilesImage;

    float nearestDistance = float.MaxValue;

    bool canMove = true, fastMove = false;

    Tile nearestTile = null;
    GameObject currentTile = null;
    private void Start()
    {
        tiles = GameManager.instance.tiles;
        tilesImage = GameManager.instance.tilesImage;

        nearestTile = tilesImage[0];
        foreach (Tile tile in tilesImage)
        {
            float dist = Vector2.Distance(tile.transform.position, transform.position);

            if (dist < nearestDistance && !tile.isPlaced)
            {
                nearestDistance = dist;
                nearestTile = tile;
            }
        }

        nearestTile.isPlaced = true;
        nearestTile.isFriend = false;
        nearestTile.score = 10;
        nearestDistance = float.MaxValue;
        GameObject toPlaceTile = currentTile;
        foreach (GameObject oTile in tiles)
        {
            float dist = Vector2.Distance(oTile.transform.position, transform.position);

            if (dist < nearestDistance)
            {
                nearestDistance = dist;
                toPlaceTile = oTile;
            }
        }

        currentTile = toPlaceTile;

        transform.position = currentTile.transform.position + Vector3.back * 0.5f;
        transform.parent = currentTile.transform;

    }

    private void Update()
    {
        if (GameManager.instance.king.activeInHierarchy)
        {
            moveCD += Time.deltaTime;

            if (GameManager.instance.score < 1000)
            {
                maxCD = 3.2f;
            }
            else if (GameManager.instance.score < 1500)
            {
                maxCD = 3f;
            }
            else if (GameManager.instance.score < 2000)
            {
                maxCD = 2.8f;
            }
            else if (GameManager.instance.score < 2500)
            {
                maxCD = 2.6f;
            }
            else if (GameManager.instance.score < 3500)
            {
                maxCD = 2.35f;
            }
            else if (GameManager.instance.score < 5000)
            {
                maxCD = 2.25f;
            }
            else
            {
                maxCD = 2;
            }

            if (moveCD > maxCD && fastMove)
            {
                moveCD = Random.Range(-0.2f, 0);
                RandomMove();
                GameManager.instance.CheckIsPlace();
                fastMove = false;
            }
            else if(moveCD > fastCD && !fastMove)
            {
                moveCD = Random.Range(-0.2f, 0);
                RandomMove();
                GameManager.instance.CheckIsPlace();
                fastMove = true;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, currentTile.transform.position, 0.1f);
        transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
    }

    void RandomMove()
    {
        int rand = Random.Range(1, 5);

        if (transform.position.x <= GameManager.instance.king.transform.position.x)
        {
            if (transform.position.x <= GameManager.instance.king.transform.position.x)
            {
                rand = Random.Range(1, 4);
            }
            else
            {
                rand = Random.Range(1, 4);
                if (rand == 1)
                    rand = 4;
            }
        }
        else
        {
            rand = Random.Range(1, 4);
            if (rand == 1)
                rand = 4;
        }

        GameObject toPlaceTile = currentTile;
        switch (rand)
        {
            case 1:
                foreach (Tile tile in tilesImage)
                {
                    float xDist = tile.transform.position.x - transform.position.x;
                    float yDist = tile.transform.position.y - transform.position.y;

                    if (xDist > 1f && xDist < 1.2f && yDist > -1 && yDist < 1)
                    {
                        if (!tile.isFriend && tile.isPlaced)
                            canMove = false;
                        else
                        {
                            nearestTile.isPlaced = false;
                            nearestTile.isFriend = false;
                            nearestTile.score = 0;
                            nearestTile = tile;
                            nearestTile.isFriend = false;
                        }
                    }
                }
                toPlaceTile = currentTile;
                foreach (GameObject oTile in tiles)
                {
                    float xDist = oTile.transform.position.x - transform.position.x;
                    float yDist = oTile.transform.position.y - transform.position.y;

                    if (xDist > 1f && xDist < 1.2f && yDist > -1 && yDist < 1)
                    {
                        toPlaceTile = oTile;
                    }
                }
                break;

            case 2:
                foreach (Tile tile in tilesImage)
                {
                    float xDist = tile.transform.position.x - transform.position.x;
                    float yDist = tile.transform.position.y - transform.position.y;

                    if (yDist > -1.2f && yDist < -1f && xDist > -1 && xDist < 1)
                    {
                        if (!tile.isFriend && tile.isPlaced)
                            canMove = false;
                        else
                        {
                            nearestTile.isPlaced = false;
                            nearestTile.isFriend = false;
                            nearestTile.score = 0;
                            nearestTile = tile;
                            nearestTile.isFriend = false;
                        }
                    }
                }
                toPlaceTile = currentTile;
                foreach (GameObject oTile in tiles)
                {
                    float xDist = oTile.transform.position.x - transform.position.x;
                    float yDist = oTile.transform.position.y - transform.position.y;

                    if (yDist > -1.2f && yDist < -1f && xDist > -1 && xDist < 1)
                    {
                        toPlaceTile = oTile;
                    }
                }
                break;

            case 3:
                foreach (Tile tile in tilesImage)
                {
                    float xDist = tile.transform.position.x - transform.position.x;
                    float yDist = tile.transform.position.y - transform.position.y;

                    if (yDist > 1f && yDist < 1.2f && xDist > -1 && xDist < 1)
                    {
                        if (!tile.isFriend && tile.isPlaced)
                            canMove = false;
                        else
                        {
                            nearestTile.isPlaced = false;
                            nearestTile.isFriend = false;
                            nearestTile.score = 0;
                            nearestTile = tile;
                            nearestTile.isFriend = false;
                        }
                    }
                }
                toPlaceTile = currentTile;
                foreach (GameObject oTile in tiles)
                {
                    float xDist = oTile.transform.position.x - transform.position.x;
                    float yDist = oTile.transform.position.y - transform.position.y;

                    if (yDist > 1f && yDist < 1.2f && xDist > -1 && xDist < 1)
                    {
                        toPlaceTile = oTile;
                    }
                }
                break;

            case 4:
                foreach (Tile tile in tilesImage)
                {
                    float xDist = tile.transform.position.x - transform.position.x;
                    float yDist = tile.transform.position.y - transform.position.y;

                    if (xDist > -1.2f && xDist < -1f && yDist > -1 && yDist < 1)
                    {
                        if (!tile.isFriend && tile.isPlaced)
                            canMove = false;
                        else
                        {
                            nearestTile.isPlaced = false;
                            nearestTile.isFriend = false;
                            nearestTile.score = 0;
                            nearestTile = tile;
                            nearestTile.isFriend = false;
                        }
                    }
                }
                toPlaceTile = currentTile;
                foreach (GameObject oTile in tiles)
                {
                    float xDist = oTile.transform.position.x - transform.position.x;
                    float yDist = oTile.transform.position.y - transform.position.y;

                    if (xDist > -1.2f && xDist < -1f && yDist > -1 && yDist < 1)
                    {
                        toPlaceTile = oTile;
                    }
                }
                break;
        }



        if (canMove)
        {
            currentTile = toPlaceTile;

            if (currentTile.transform.childCount != 0)
            {
                foreach (Transform child in currentTile.transform)
                {
                    if (child.gameObject.name == "King")
                        child.gameObject.SetActive(false);
                    else if (child.gameObject.tag == "Piece")
                        Destroy(child.gameObject);

                    if (child.gameObject.tag == "Piece" || child.gameObject.name == "King")
                    {
                        GameObject move = Instantiate(GameManager.instance.explosion, transform);
                        GameManager.instance.screenShake = 0.2f;
                        Destroy(move, 1);
                    }
                }
            }

            nearestTile.isPlaced = true;
            nearestTile.isFriend = false;
            nearestTile.score = 15;
            transform.parent = currentTile.transform;
        }
        canMove = true;
    }

}

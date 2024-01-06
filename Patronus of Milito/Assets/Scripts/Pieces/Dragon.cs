using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    float moveCD, fastCD = 0.1f;

    public GameObject[] skyTiles, tiles;
    public Tile[] skyTilesImage, tilesImage;

    float nearestDistance = float.MaxValue;

    bool canMove = true, mode2 = false, drop = false, toGround, sky = false;


    Tile nearestSkyTile = null, nearestTile = null;
    GameObject currentSkyTile = null;

    float maxCD = 2.5f;

    int fastMove = 0;

    private void Start()
    {
        skyTiles = GameManager.instance.skyTiles;
        skyTilesImage = GameManager.instance.skyTilesImage;
        tiles = GameManager.instance.tiles;
        tilesImage = GameManager.instance.tilesImage;

        int rand = Random.Range(0, 2);

        if (rand == 0)
        {
            sky = true;

            transform.position += Vector3.up * 20;

            nearestSkyTile = skyTilesImage[0];
            nearestTile = tilesImage[0];
            foreach (Tile tile in skyTilesImage)
            {
                float dist = Vector2.Distance(tile.transform.position, transform.position);

                if (dist < nearestDistance && !nearestSkyTile.isPlaced)
                {
                    nearestDistance = dist;
                    nearestSkyTile = tile;
                }
            }

            nearestSkyTile.isPlaced = true;
            nearestSkyTile.isFriend = false;
            nearestTile.score = 50;

            nearestDistance = float.MaxValue;
            foreach (GameObject oTile in skyTiles)
            {
                float dist = Vector2.Distance(oTile.transform.position, transform.position);

                if (dist < nearestDistance)
                {
                    nearestDistance = dist;
                    currentSkyTile = oTile;
                }
            }

            transform.position = currentSkyTile.transform.position + Vector3.back;
            transform.parent = currentSkyTile.transform;
        }
        else
        {
            nearestSkyTile = skyTilesImage[0];
            nearestTile = tilesImage[0];
            foreach (Tile tile in tilesImage)
            {
                float dist = Vector2.Distance(tile.transform.position, transform.position);

                if (dist < nearestDistance && !nearestSkyTile.isPlaced)
                {
                    nearestDistance = dist;
                    nearestSkyTile = tile;
                }
            }

            nearestSkyTile.isPlaced = true;
            nearestSkyTile.isFriend = false;
            nearestTile.score = 50;

            nearestDistance = float.MaxValue;
            foreach (GameObject oTile in tiles)
            {
                float dist = Vector2.Distance(oTile.transform.position, transform.position);

                if (dist < nearestDistance)
                {
                    nearestDistance = dist;
                    currentSkyTile = oTile;
                }
            }

            transform.position = currentSkyTile.transform.position + Vector3.back;
            transform.parent = currentSkyTile.transform;
        }
    }

    private void Update()
    {
        if (GameManager.instance.king.activeInHierarchy)
        {
            moveCD += Time.deltaTime;

            if (GameManager.instance.score < 1000)
            {
                maxCD = 2.3f;
            }
            else if (GameManager.instance.score < 1500)
            {
                maxCD = 2.1f;
            }
            else if (GameManager.instance.score < 2000)
            {
                maxCD = 2f;
            }
            else if (GameManager.instance.score < 2500)
            {
                maxCD = 1.9f;
            }
            else if (GameManager.instance.score < 3500)
            {
                maxCD = 1.8f;
            }
            else if (GameManager.instance.score < 5000)
            {
                maxCD = 1.6f;
            }
            else
            {
                maxCD = 1.5f;
            }

            if (moveCD > maxCD && fastMove == 0)
            {
                moveCD = Random.Range(-0.2f, 0);

                GameManager.instance.CheckIsPlace();
                if (sky == true)
                {
                    RandomMove();
                    fastMove = 4;
                }
                else
                {
                    RandomMoveOnGround();
                    fastMove = 2;
                }
            }
            else if (moveCD > fastCD && fastMove != 0)
            {
                moveCD = Random.Range(-0.2f, 0);
                if (sky == true)
                    RandomMove();
                else
                    RandomMoveOnGround();
                GameManager.instance.CheckIsPlace();
                fastMove--;
            }
        }
        if (currentSkyTile == null)
            Debug.Log("um");
        currentSkyTile = transform.parent.gameObject;

        if (currentSkyTile != null)
            transform.position = Vector2.MoveTowards(transform.position, currentSkyTile.transform.position, 0.1f);

        transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
    }

    void RandomMoveOnGround()
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

        GameObject toPlaceTile = currentSkyTile;
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
                toPlaceTile = currentSkyTile;
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
                toPlaceTile = currentSkyTile;
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
                toPlaceTile = currentSkyTile;
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
                toPlaceTile = currentSkyTile;
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
            currentSkyTile = toPlaceTile;

            if (currentSkyTile.transform.childCount != 0)
            {
                foreach (Transform child in currentSkyTile.transform)
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
            nearestTile.score = 50;
            transform.parent = currentSkyTile.transform;
        }
        canMove = true;
    }

    void RandomMove()
    {
        if (toGround)
            toGround = false;

        int rand = Random.Range(1, 5);

        Vector2 objectPos = transform.position;

        float minX = 0, maxX = 0, minY = 0, maxY = 0;

        int rand2 = Random.Range(0, 2);

        if (rand2 == 0)
        {
            rand = Random.Range(1, 4);
        }
        else
        {
            rand = Random.Range(1, 4);
            if (rand == 1)
                rand = 4;
        }

        switch (rand)
        {
            case 1:
                minX = 1; maxX = 1.2f; minY = -1; maxY = 1;
                break;
            case 2:
                minX = -1; maxX = 1; minY = -1.2f; maxY = -1f;
                break;
            case 3:
                minX = -1; maxX = 1; minY = 1f; maxY = 1.2f;
                break;
            case 4:
                minX = -1.2f; maxX = -1; minY = -1; maxY = 1;
                break;
        }

        GameObject oNearestSkyTile = currentSkyTile;
        foreach (Tile tile in skyTilesImage)
        {
            float xDist = tile.transform.position.x - currentSkyTile.transform.position.x;
            float yDist = tile.transform.position.y - currentSkyTile.transform.position.y;

            if (xDist > minX && xDist < maxX && yDist > minY && yDist < maxY)
            {
                if (!tile.isFriend && tile.isPlaced)
                    canMove = false;
                else
                {
                    nearestSkyTile.isPlaced = false;
                    nearestSkyTile.isFriend = false;
                    nearestSkyTile.score = 0;
                    nearestSkyTile = tile;
                    nearestSkyTile.isFriend = false;
                }
            }
        }
        foreach (GameObject oTile in skyTiles)
        {
            float xDist = oTile.transform.position.x - currentSkyTile.transform.position.x;
            float yDist = oTile.transform.position.y - currentSkyTile.transform.position.y;

            if (xDist > minX && xDist < maxX && yDist > minY && yDist < maxY)
            {
                oNearestSkyTile = oTile;
            }
        }

        currentSkyTile = oNearestSkyTile;

        if (canMove)
        {
            if (currentSkyTile.transform.childCount != 0)
            {
                foreach (Transform child in currentSkyTile.transform)
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

            nearestSkyTile.isPlaced = true;
            nearestSkyTile.isFriend = false;
            nearestSkyTile.score = 50;

            transform.parent = currentSkyTile.transform;
        }
        canMove = true;
    }
}

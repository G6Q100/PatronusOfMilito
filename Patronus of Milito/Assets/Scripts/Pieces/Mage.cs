using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    float moveCD;

    public GameObject[] skyTiles, tiles;
    public Tile[] skyTilesImage, tilesImage;

    float nearestDistance = float.MaxValue;

    bool canMove = true, mode2 = false, drop = false, toGround;


    Tile nearestSkyTile = null, nearestTile = null;
    GameObject currentSkyTile = null;

    GameObject objectFollow = null;

    float maxCD = 2.5f;

    private void Start()
    {
        transform.position += Vector3.up * 20;
        skyTiles = GameManager.instance.skyTiles;
        skyTilesImage = GameManager.instance.skyTilesImage;
        tiles = GameManager.instance.tiles;
        tilesImage = GameManager.instance.tilesImage;

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
        nearestTile.score = 20;

        GameObject toPlaceTile = objectFollow;
        nearestDistance = float.MaxValue;
        for (int i = 0; i < tiles.Length; i++)
        {
            float dist = Vector2.Distance(tiles[i].transform.position,
                transform.position + Vector3.down * 20);

            if (dist < nearestDistance && tilesImage[i].isKing)
            {
                nearestDistance = dist;
                toPlaceTile = tiles[i];
            }
        }

        objectFollow = toPlaceTile;

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

    private void Update()
    {
        if (GameManager.instance.king.activeInHierarchy)
        {
            moveCD += Time.deltaTime;

            if (GameManager.instance.score < 1000)
            {
                maxCD = 3.8f;
            }
            else if (GameManager.instance.score < 1500)
            {
                maxCD = 3.6f;
            }
            else if (GameManager.instance.score < 2000)
            {
                maxCD = 3.2f;
            }
            else if (GameManager.instance.score < 2500)
            {
                maxCD = 3f;
            }
            else if (GameManager.instance.score < 3500)
            {
                maxCD = 2.8f;
            }
            else if (GameManager.instance.score < 5000)
            {
                maxCD = 2.6f;
            }
            else
            {
                maxCD = 2.5f;
            }

            if (moveCD > maxCD)
            {
                moveCD = Random.Range(-0.2f, 0);
                RandomMove();
                GameManager.instance.CheckIsPlace();
            }
        }
        currentSkyTile = transform.parent.gameObject;

        if (toGround)
            transform.position = Vector2.MoveTowards(transform.position, currentSkyTile.transform.position, 1f);
        else
            transform.position = Vector2.MoveTowards(transform.position, currentSkyTile.transform.position, 0.1f);

        transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
    }

    void RandomMove()
    {
        if (toGround)
            toGround = false;

        int rand = Random.Range(1, 5);

        objectFollow = GameManager.instance.king;

        Vector2 objectPos = transform.position;

        float dist = Vector2.Distance(objectFollow.transform.position,
                objectPos + Vector2.down * 20);

        float minX = 0, maxX = 0, minY = 0, maxY = 0;

        if (transform.position.x <= objectFollow.transform.position.x)
        {
            if (transform.position.x <= objectFollow.transform.position.x)
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
        
        if(!drop && dist < 1)
        {
            toGround = true;
            mode2 = true;
            GameManager.instance.screenShake = 0.2f;
            GameObject move = Instantiate(GameManager.instance.explosion, transform);
            Destroy(move, 1);
            GameManager.instance.king.SetActive(false);
        }

        GameObject oNearestSkyTile = currentSkyTile;
        if (!mode2)
        {
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
                nearestSkyTile.score = 40;

                transform.parent = currentSkyTile.transform;
            }
            canMove = true;
        }
        else
        {
            if (mode2 && !drop)
            {
                drop = true;
                currentSkyTile = objectFollow;

                nearestTile.isPlaced = false;
                nearestTile.isFriend = false;
                nearestTile.score = 0;


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

                transform.parent = currentSkyTile.transform;

                objectFollow = GameManager.instance.king;

                canMove = false;
            }

            foreach (Tile tile in tilesImage)
            {
                float xDist = tile.transform.position.x - currentSkyTile.transform.position.x;
                float yDist = tile.transform.position.y - currentSkyTile.transform.position.y;

                if (xDist > minX && xDist < maxX && yDist > minY && yDist < maxY)
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
            foreach (GameObject oTile in tiles)
            {
                float xDist = oTile.transform.position.x - currentSkyTile.transform.position.x;
                float yDist = oTile.transform.position.y - currentSkyTile.transform.position.y;

                if (xDist > minX && xDist < maxX && yDist > minY && yDist < maxY)
                {
                    oNearestSkyTile = oTile;
                }
            }
            
            if (canMove)
            {

                currentSkyTile = oNearestSkyTile;

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
                nearestTile.score = 40;
                transform.parent = currentSkyTile.transform;
            }
            canMove = true;
        }
    }
}

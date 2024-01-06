using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int gold, goldLost, score, minionType = 1;
    public Text goldText, lostText, scoreText, viewText;
    public Button changeView;

    public bool sky, skyMinionm, moving, started, newMinion, skyEnemy;
    [HideInInspector]
    public Minion minionToPlace, minionMove;

    public GameObject grid, king, gamOverImage, statImage, skyGrid, explosion, moveSound, everything, oStartTile;

    public GameObject[] tiles, skyTiles;
    public Tile[] tilesImage, skyTilesImage;

    public Tile currentTile, startTile;

    [SerializeField]
    GameObject piece,
    warrior, archer, rider, skyMage, tower, assassin, camp, skyDefenceTower;

    public Sprite bigSprite;

    public float screenShake = 0;

    public GameObject miniMap01, miniMap02, mapChange, 
        tutorial, tutorial2, tutorial3, tutorial4, tutorial5;

    public static GameManager instance = null; // original

    private void Awake()
    {
        if (instance == null)      // original check
        {
            instance = this;
        }
    }

    private void Start()
    {
        tutorial.SetActive(true);
        PlayerPrefs.SetInt("played", 0);
        if (PlayerPrefs.GetInt("played") == 0)
        {
            tutorial.SetActive(true);
            Time.timeScale = 0;
            PlayerPrefs.SetInt("played", 1);
        }

        if (PlayerPrefs.GetFloat("StartGold") > 50)
            gold = (int)PlayerPrefs.GetFloat("StartGold");
        startTile = tilesImage[0];
        mapChange.SetActive(false);
    }

    private void Update()
    {
        if(score >= 500 && !skyEnemy)
        {
            mapChange.SetActive(true);
            skyEnemy = true;
        }

        CheckIsPlace();

        Tutroial();

        if (Time.timeScale <= 0)
        {
            if (minionToPlace != null)
            {
                foreach (Tile tile in skyTilesImage)
                {
                    tile.canPlaced = false;
                }
                foreach (Tile tile in tilesImage)
                {
                    tile.canPlaced = false;
                }
                minionToPlace = null;
                minionToPlace = null;
                moving = false;
                newMinion = false;
            }
        }
        else if (king.activeInHierarchy)
        {
            PlaceMinion();
        }
        else
        {
            Time.timeScale = 0.5f;
            GameObject expolsions = Instantiate(explosion, transform);
            Destroy(expolsions, 1);
            gamOverImage.GetComponent<Image>().enabled = true;
            statImage.SetActive(true);
            gameObject.SetActive(false);
        }

        if (screenShake < 0.05f)
            screenShake = 0;
        else
        {
            screenShake -= Time.deltaTime * 5;
        }

        everything.transform.position = new Vector2(Random.Range(-screenShake, screenShake),
            Random.Range(-screenShake, screenShake));

        if (moving == true)
        {
            MoveMinion(minionMove, minionType);
        }

        if (!king.activeInHierarchy || (started && !startTile.isFriend && !newMinion))
        {
            if (grid.activeInHierarchy)
                minionToPlace = null;
            if (!sky)
                grid.SetActive(false);
            else
                skyGrid.SetActive(false);
        }
    }

    void Tutroial()
    {
        if (PlayerPrefs.GetInt("played") == 1 && score > 150)
        {
            tutorial2.SetActive(true);
            Time.timeScale = 0;
            PlayerPrefs.SetInt("played", 2);
        }
        else if (PlayerPrefs.GetInt("played") == 2 && score > 500)
        {
            tutorial3.SetActive(true);
            Time.timeScale = 0;
            PlayerPrefs.SetInt("played", 3);
        }
        else if (PlayerPrefs.GetInt("played") == 3 && score > 1000)
        {
            tutorial4.SetActive(true);
            Time.timeScale = 0;
            PlayerPrefs.SetInt("played", 4);
        }
        else if (PlayerPrefs.GetInt("played") == 4 && score > 1500)
        {
            tutorial5.SetActive(true);
            Time.timeScale = 0;
            PlayerPrefs.SetInt("played", 5);
        }
    }

    public void CheckIsPlace()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].transform.childCount == 0)
            {
                tilesImage[i].isPlaced = false;
                tilesImage[i].isFriend = false;
            }
            else
            {
                tilesImage[i].isPlaced = true;

                if (tiles[i].transform.childCount > 1 && !tilesImage[i].isKing)
                {
                    int childs = 0;
                    foreach (Transform child in tiles[i].transform)
                    {
                        if (childs < tiles[i].transform.childCount - 1)
                            Destroy(child.gameObject);
                        childs++;
                    }
                }
                if (tiles[i].transform.GetChild(0).tag == "Enemy")
                    tilesImage[i].isFriend = false;
                else
                    tilesImage[i].isFriend = true;
            }
        }
        for (int i = 0; i < skyTiles.Length; i++)
        {
            if (skyTiles[i].transform.childCount == 0)
            {
                skyTilesImage[i].isPlaced = false;
                skyTilesImage[i].isFriend = false;
            }
            else
            {
                skyTilesImage[i].isPlaced = true;

                if (skyTiles[i].transform.childCount > 1 && !skyTilesImage[i].isKing)
                {
                    int childs = 0;
                    foreach (Transform child in skyTiles[i].transform)
                    {
                        if (childs < skyTiles[i].transform.childCount - 1)
                            Destroy(child.gameObject);
                        childs++;
                    }
                }

                if (skyTiles[i].transform.GetChild(0).tag == "Enemy")
                    skyTilesImage[i].isFriend = false;
                else
                    skyTilesImage[i].isFriend = true;
            }
        }
    }

    void PlaceMinion()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject move = Instantiate(moveSound, transform);
            Destroy(move, 1);

            if (minionToPlace != null)
            {
                moving = false;
                GameObject nearestTile = null;
                Tile nearestTileImage = null;
                float nearestDistance = float.MaxValue,
                    nearestImageDistance = float.MaxValue;

                if (sky)
                {
                    foreach (GameObject tile in skyTiles)
                    {
                        float dist = Vector2.Distance(tile.transform.position,
                            Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        if (dist < nearestDistance)
                        {
                            nearestDistance = dist;
                            nearestTile = tile;
                        }
                    }

                    foreach (Tile tile in skyTilesImage)
                    {
                        float dist = Vector2.Distance(tile.transform.position,
                            Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        if (dist < nearestImageDistance)
                        {
                            nearestImageDistance = dist;
                            nearestTileImage = tile;
                        }
                    }
                }
                else
                {
                    foreach (GameObject tile in tiles)
                    {
                        float dist = Vector2.Distance(tile.transform.position,
                            Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        if (dist < nearestDistance)
                        {
                            nearestDistance = dist;
                            nearestTile = tile;
                        }
                    }

                    foreach (Tile tile in tilesImage)
                    {
                        float dist = Vector2.Distance(tile.transform.position,
                            Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        if (dist < nearestImageDistance)
                        {
                            nearestImageDistance = dist;
                            nearestTileImage = tile;
                        }
                    }
                }

                if (nearestTileImage.canPlaced == true && nearestTileImage.isFriend != true)
                {
                    started = true;
                    nearestImageDistance = float.MaxValue;
                    if (currentTile != null)
                    {
                        if (sky)
                        {
                            foreach (Tile tile in skyTilesImage)
                            {
                                float dist = Vector2.Distance(tile.transform.position,
                                    Camera.main.ScreenToWorldPoint(currentTile.transform.position));
                                if (dist < nearestImageDistance)
                                {
                                    nearestImageDistance = dist;
                                    startTile = tile;
                                }
                            }
                        }
                        else
                        {
                            foreach (Tile tile in tilesImage)
                            {
                                float dist = Vector2.Distance(tile.transform.position,
                                    Camera.main.ScreenToWorldPoint(currentTile.transform.position));
                                if (dist < nearestImageDistance)
                                {
                                    nearestImageDistance = dist;
                                    startTile = tile;
                                }
                            }
                        }
                        if (!newMinion)
                        {
                            startTile.isFriend = false;
                            startTile.isPlaced = false;
                        }

                        if (nearestTileImage.isPlaced == true)
                        {
                            if (nearestTile.transform.childCount != 0)
                            {
                                foreach (Transform child in nearestTile.transform)
                                {
                                    Destroy(child.gameObject);
                                    screenShake = 0.2f;
                                    move = Instantiate(explosion, transform);
                                    Destroy(move, 1);
                                }
                            }
                        }

                        if (!newMinion)
                        {
                            foreach (Transform child in oStartTile.transform)
                            {
                                Destroy(child.gameObject);
                            }
                        }
                        currentTile.isFriend = false;
                        currentTile.isPlaced = false;
                    }

                    GameObject minionPlace = Instantiate(minionToPlace.gameObject,
                            nearestTile.transform.position + Vector3.back * 0.5f,
                            Quaternion.identity) as GameObject;

                    minionPlace.transform.GetComponent<Minion>().maxCD = 0;

                    minionPlace.transform.parent = nearestTile.transform;

                    oStartTile = nearestTile;
                    startTile = nearestTileImage;
                    currentTile = nearestTileImage;

                    minionToPlace = null;
                    gold -= goldLost;
                    score += goldLost / 10;
                    nearestTileImage.isPlaced = true;
                    nearestTileImage.isFriend = true;
                    score += nearestTileImage.score;
                    //scoreText.text =  "" + score;

                    if (PlayerPrefs.GetFloat("SoulGain") > 1)
                        gold += (int)(nearestTileImage.score * PlayerPrefs.GetFloat("SoulGain"));
                    else
                        gold += nearestTileImage.score;

                    goldText.text = gold.ToString();
                    scoreText.text = score.ToString();

                    nearestTileImage.score = 0;

                    newMinion = false;

                    if (!sky)
                        grid.SetActive(false);
                    else
                        skyGrid.SetActive(false);
                }
                else
                {
                    foreach (Tile tile in skyTilesImage)
                    {
                        tile.canPlaced = false;
                    }
                    foreach (Tile tile in tilesImage)
                    {
                        tile.canPlaced = false;
                    }
                    minionToPlace = null;
                    moving = false;
                    newMinion = false;
                }

                return;
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.transform.tag == "Piece" && hit.transform.name != "Camp(Clone)")
                {
                    if (hit.transform.GetComponent<Minion>().moveCD >=
                        hit.transform.GetComponent<Minion>().maxCD)
                    {
                        GameObject nearestTile = null;
                        Tile nearestTileImage = null;
                        float nearestDistance = float.MaxValue,
                            nearestImageDistance = float.MaxValue;

                        if (sky)
                        {
                            foreach (GameObject tile in skyTiles)
                            {
                                float dist = Vector2.Distance(tile.transform.position,
                                    Camera.main.ScreenToWorldPoint(Input.mousePosition));
                                if (dist < nearestDistance)
                                {
                                    nearestDistance = dist;
                                    nearestTile = tile;
                                }
                            }

                            foreach (Tile tile in skyTilesImage)
                            {
                                float dist = Vector2.Distance(tile.transform.position,
                                    Camera.main.ScreenToWorldPoint(Input.mousePosition));
                                if (dist < nearestImageDistance)
                                {
                                    nearestImageDistance = dist;
                                    nearestTileImage = tile;
                                }
                            }
                        }
                        else
                        {
                            foreach (GameObject tile in tiles)
                            {
                                float dist = Vector2.Distance(tile.transform.position,
                                    Camera.main.ScreenToWorldPoint(Input.mousePosition));
                                if (dist < nearestDistance)
                                {
                                    nearestDistance = dist;
                                    nearestTile = tile;
                                }
                            }

                            foreach (Tile tile in tilesImage)
                            {
                                float dist = Vector2.Distance(tile.transform.position,
                                    Camera.main.ScreenToWorldPoint(Input.mousePosition));
                                if (dist < nearestImageDistance)
                                {
                                    nearestImageDistance = dist;
                                    nearestTileImage = tile;
                                }
                            }
                        }


                        currentTile = nearestTileImage;
                        oStartTile = nearestTile;

                        currentTile.isFriend = true;
                        currentTile.isPlaced = true;

                        startTile = currentTile;

                        moving = true;
                        switch (hit.transform.name)
                        {
                            case "Warrior(Clone)":
                                minionMove = warrior.GetComponent<Minion>();
                                minionType = 1;
                                break;
                            case "Archer(Clone)":
                                minionMove = archer.GetComponent<Minion>();
                                minionType = 2;
                                break;
                            case "Rider(Clone)":
                                minionMove = rider.GetComponent<Minion>();
                                minionType = 3;
                                break;
                            case "SkyMage(Clone)":
                                if (sky)
                                {
                                    minionMove = skyMage.GetComponent<Minion>();
                                    minionType = 6;
                                }
                                else
                                {
                                    minionMove = skyMage.GetComponent<Minion>();
                                    minionType = 4;
                                }
                                break;
                            case "Tower(Clone)":
                                minionMove = tower.GetComponent<Minion>();
                                minionType = 5;
                                break;
                            case "Assassin(Clone)":
                                minionMove = assassin.GetComponent<Minion>();
                                minionType = 7;
                                break;
                            case "SkyDefenceTower(Clone)":
                                if (sky)
                                {
                                    minionMove = skyDefenceTower.GetComponent<Minion>();
                                    minionType = 8;
                                }
                                break;
                        }
                    }
                }
                else
                {
                    foreach (Tile tile in skyTilesImage)
                    {
                        tile.canPlaced = false;
                    }
                    foreach (Tile tile in tilesImage)
                    {
                        tile.canPlaced = false;
                    }
                    minionToPlace = null;
                    moving = false;
                }
            }
        }
    }

    public void SetCost(int cost)
    {
        goldLost = cost;
    }

    public void BuyMinion(Minion minion)
    {
        moving = false;
        if (((sky && minion.gameObject.name.Contains("Sky")) || (!sky && !minion.gameObject.name.Contains("SkyDefence"))) && 
            PlayerPrefs.GetInt(minion.gameObject.name + "Unlock") == 1 && gold >= goldLost)
        {
            newMinion = true;

            if (!sky)
                grid.SetActive(true);
            else
                skyGrid.SetActive(true);

            minionToPlace = minion;

            if (sky)
            {
                foreach (Tile tile in skyTilesImage)
                {
                    float dist = Vector2.Distance(tile.transform.position, new Vector3(-0.4f, 20.3f, 0f));
                    if (dist < 2.5f)
                    {
                        tile.canPlaced = true;
                    }
                    else
                    {
                        tile.canPlaced = false;
                    }
                }
                return;
            }

            foreach (Tile tile in tilesImage)
            {

                float dist = Vector2.Distance(tile.transform.position, new Vector3(-0.4f, 0.3f, 0f));
                if (dist < 2.5f)
                {
                    tile.canPlaced = true;
                }
                else
                {
                    tile.canPlaced = false;
                }

                if (GameObject.FindGameObjectsWithTag("Piece").Length > 0)
                {
                    foreach (GameObject camp in GameObject.FindGameObjectsWithTag("Piece"))
                    {
                        if (camp.name.Contains("Camp"))
                        {
                            dist = Vector2.Distance(tile.transform.position, camp.transform.position);
                            if (dist < 1.2f)
                            {
                                tile.canPlaced = true;
                            }
                        }
                    }
                }

                if (minionToPlace.name.Contains("Camp"))
                    tile.canPlaced = true;
            }
        }
    }

    public void MoveMinion(Minion minion, int moveType)
    {
        switch (moveType)
        {
            case 1:
                foreach (Tile tile in tilesImage)
                {
                    tile.canPlaced = false;
                    float xDist = tile.transform.position.x - currentTile.transform.position.x;
                    float yDist = tile.transform.position.y - currentTile.transform.position.y;
                    // cross move
                    if (((xDist > -2.5f && xDist < 2.5f && yDist > -1 && yDist < 1) ||
                        (yDist > -2.5f && yDist < 2.5f && xDist > -1 && xDist < 1)) && !tile.isBlocked)
                    {
                        tile.canPlaced = true;

                        if (tile.isPlaced)
                        {
                            if (xDist > -1.2f && xDist < -1)
                            {
                                foreach (Tile tiles in tilesImage)
                                {
                                    xDist = tiles.transform.position.x - currentTile.transform.position.x;
                                    yDist = tiles.transform.position.y - currentTile.transform.position.y;

                                    if ((xDist > -2.5f && xDist < -2f && yDist > -1 && yDist < 1))
                                    {
                                        tiles.canPlaced = false;
                                        tiles.isBlocked = true;
                                    }
                                }
                            }
                            else if (xDist > 1 && xDist < 1.2f)
                            {
                                foreach (Tile tiles in tilesImage)
                                {
                                    xDist = tiles.transform.position.x - currentTile.transform.position.x;
                                    yDist = tiles.transform.position.y - currentTile.transform.position.y;

                                    if ((xDist > 2f && xDist < 2.5f && yDist > -1 && yDist < 1))
                                    {
                                        tiles.canPlaced = false;
                                        tiles.isBlocked = true;
                                    }
                                }
                            }
                            else if (yDist > -1.2f && yDist < -1)
                            {
                                foreach (Tile tiles in tilesImage)
                                {
                                    xDist = tiles.transform.position.x - currentTile.transform.position.x;
                                    yDist = tiles.transform.position.y - currentTile.transform.position.y;

                                    if ((yDist > -2.5f && yDist < -2f && xDist > -1 && xDist < 1))
                                    {
                                        tiles.canPlaced = false;
                                        tiles.isBlocked = true;
                                    }
                                }
                            }
                            else if (yDist > 1 && yDist < 1.2f)
                            {
                                foreach (Tile tiles in tilesImage)
                                {
                                    xDist = tiles.transform.position.x - currentTile.transform.position.x;
                                    yDist = tiles.transform.position.y - currentTile.transform.position.y;

                                    if ((yDist > 2f && yDist < 2.5f && xDist > -1 && xDist < 1))
                                    {
                                        tiles.canPlaced = false;
                                        tiles.isBlocked = true;
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case 2:
                foreach (Tile tile in tilesImage)
                {
                    // circle move
                    float dist = Vector2.Distance(tile.transform.position, currentTile.transform.position);
                    if (dist > 1.8f && dist < 2.5f)
                    {
                        tile.canPlaced = true;
                    }
                    else
                    {
                        tile.canPlaced = false;
                    }
                }
                break;

            case 3:
                foreach (Tile tile in tilesImage)
                {
                    float xDist = tile.transform.position.x - currentTile.transform.position.x;
                    float yDist = tile.transform.position.y - currentTile.transform.position.y;

                    //checkerborad move
                    if ((xDist > -1.3f && xDist < -1f && yDist > -1.3f && yDist < -1f) ||   // "X" pattern
                        (xDist > -1.3f && xDist < -1f && yDist > 1 && yDist < 1.3f) ||
                        (xDist > 1f && xDist < 1.3f && yDist > -1.3f && yDist < -1f) ||
                        (xDist > 1f && xDist < 1.3f && yDist > 1 && yDist < 1.3f) ||

                        (xDist > -2.5f && xDist < -2f && yDist > -2.5f && yDist < -2f) ||   // big "X" pattern
                        (xDist > -2.5f && xDist < -2f && yDist > 2f && yDist < 2.5f) ||
                        (xDist > 2f && xDist < 2.5f && yDist > -2.5f && yDist < -2f) ||
                        (xDist > 2f && xDist < 2.5f && yDist > 2f && yDist < 2.5f) ||

                        (xDist > -2.5f && xDist < -2f && yDist > -1f && yDist < 1f) ||   // cross pattern
                        (xDist > 2f && xDist < 2.5f && yDist > -1f && yDist < 1f) ||
                        (xDist > -1f && xDist < 1f && yDist > -2.5f && yDist < -2f) ||
                        (xDist > -1f && xDist < 1f && yDist > 2f && yDist < 2.5f))
                    {
                        tile.canPlaced = true;
                    }
                    else
                    {
                        tile.canPlaced = false;
                    }
                }

                break;
            case 4:
                foreach (Tile tile in tilesImage)
                {
                    float xDist = tile.transform.position.x - currentTile.transform.position.x;
                    float yDist = tile.transform.position.y - currentTile.transform.position.y;

                    //checkerborad move
                    if ((yDist > -1.3f && yDist < -1f) ||   // "X" pattern
                        (yDist > 1 && yDist < 1.3f))
                    {
                        tile.canPlaced = true;
                    }
                    else
                    {
                        tile.canPlaced = false;
                    }
                }
                break;
            case 5:
                foreach (Tile tile in tilesImage)
                {
                    float xDist = tile.transform.position.x - currentTile.transform.position.x;
                    float yDist = tile.transform.position.y - currentTile.transform.position.y;

                    //checkerborad move
                    if ((xDist > -1f && xDist < 1f) || (yDist > -1f && yDist < 1f) ||

                        (xDist > -1.3f && xDist < -1f && yDist > -1.3f && yDist < -1f) ||   // "X" pattern
                        (xDist > -1.3f && xDist < -1f && yDist > 1 && yDist < 1.3f) ||
                        (xDist > 1f && xDist < 1.3f && yDist > -1.3f && yDist < -1f) ||
                        (xDist > 1f && xDist < 1.3f && yDist > 1 && yDist < 1.3f) ||

                        (xDist > -2.5f && xDist < -2f && yDist > -2.5f && yDist < -2f) ||   // big "X" pattern
                        (xDist > -2.5f && xDist < -2f && yDist > 2f && yDist < 2.5f) ||
                        (xDist > 2f && xDist < 2.5f && yDist > -2.5f && yDist < -2f) ||
                        (xDist > 2f && xDist < 2.5f && yDist > 2f && yDist < 2.5f) ||

                        (xDist > -3.5f && xDist < -3f && yDist > -3.5f && yDist < -3f) ||   // "X" pattern2
                        (xDist > -3.5f && xDist < -3f && yDist > 3f && yDist < 3.5f) ||
                        (xDist > 3f && xDist < 3.5f && yDist > -3.5f && yDist < -3f) ||
                        (xDist > 3f && xDist < 3.5f && yDist > 3f && yDist < 3.5f) ||

                        (xDist > -4.6f && xDist < -4f && yDist > -4.6f && yDist < -4f) ||   // big "X" pattern2
                        (xDist > -4.6f && xDist < -4f && yDist > 4f && yDist < 4.6f) ||
                        (xDist > 4f && xDist < 4.6f && yDist > -4.6f && yDist < -4f) ||
                        (xDist > 4f && xDist < 4.6f && yDist > 4f && yDist < 4.6f) ||

                        (xDist > -5.8f && xDist < -5f && yDist > -5.8f && yDist < -5f) ||   // "X" pattern3
                        (xDist > -5.8f && xDist < -5f && yDist > 5f && yDist < 5.8f) ||
                        (xDist > 5f && xDist < 5.8f && yDist > -5.8f && yDist < -5f) ||
                        (xDist > 5f && xDist < 5.8f && yDist > 5f && yDist < 5.8f) ||

                        (xDist > -6.8f && xDist < -6f && yDist > -6.8f && yDist < -6f) ||   // big "X" pattern3
                        (xDist > -6.8f && xDist < -6f && yDist > 6f && yDist < 6.8f) ||
                        (xDist > 6f && xDist < 6.8f && yDist > -6.8f && yDist < -6f) ||
                        (xDist > 6f && xDist < 6.8f && yDist > 6f && yDist < 6.8f))
                    {
                        if (tile.isPlaced)
                            tile.canPlaced = true;
                        else if ((xDist > -2f && xDist < 2f && yDist > -2f && yDist < 2f))
                            tile.canPlaced = true;
                        else
                            tile.canPlaced = false;
                    }
                    else
                    {
                        tile.canPlaced = false;
                    }
                }
                break;
            case 6:
                foreach (Tile tile in skyTilesImage)
                {
                    tile.canPlaced = false;

                    float xDist = tile.transform.position.x - currentTile.transform.position.x;
                    float yDist = tile.transform.position.y - currentTile.transform.position.y;
                    // cross move
                    if (((xDist > -2.5f && xDist < 2.5f && yDist > -1 && yDist < 1) ||
                        (yDist > -2.5f && yDist < 2.5f && xDist > -1 && xDist < 1) ||

                        (xDist > -1.3f && xDist < -1f && yDist > -1.3f && yDist < -1f) ||   // "X" pattern
                        (xDist > -1.3f && xDist < -1f && yDist > 1 && yDist < 1.3f) ||
                        (xDist > 1f && xDist < 1.3f && yDist > -1.3f && yDist < -1f) ||
                        (xDist > 1f && xDist < 1.3f && yDist > 1 && yDist < 1.3f)) && !tile.isBlocked)
                    {
                        tile.canPlaced = true;

                        if (tile.isPlaced)
                        {
                            if (xDist > -1.2f && xDist < -1)
                            {
                                foreach (Tile tiles in skyTilesImage)
                                {
                                    xDist = tiles.transform.position.x - currentTile.transform.position.x;
                                    yDist = tiles.transform.position.y - currentTile.transform.position.y;

                                    if ((xDist > -2.5f && xDist < -2f && yDist > -1 && yDist < 1))
                                    {
                                        tiles.canPlaced = false;
                                        tiles.isBlocked = true;
                                    }
                                }
                            }
                            else if (xDist > 1 && xDist < 1.2f)
                            {
                                foreach (Tile tiles in skyTilesImage)
                                {
                                    xDist = tiles.transform.position.x - currentTile.transform.position.x;
                                    yDist = tiles.transform.position.y - currentTile.transform.position.y;

                                    if ((xDist > 2f && xDist < 2.5f && yDist > -1 && yDist < 1))
                                    {
                                        tiles.canPlaced = false;
                                        tiles.isBlocked = true;
                                    }
                                }
                            }
                            else if (yDist > -1.2f && yDist < -1)
                            {
                                foreach (Tile tiles in skyTilesImage)
                                {
                                    xDist = tiles.transform.position.x - currentTile.transform.position.x;
                                    yDist = tiles.transform.position.y - currentTile.transform.position.y;

                                    if ((yDist > -2.5f && yDist < -2f && xDist > -1 && xDist < 1))
                                    {
                                        tiles.canPlaced = false;
                                        tiles.isBlocked = true;
                                    }
                                }
                            }
                            else if (yDist > 1 && yDist < 1.2f)
                            {
                                foreach (Tile tiles in skyTilesImage)
                                {
                                    xDist = tiles.transform.position.x - currentTile.transform.position.x;
                                    yDist = tiles.transform.position.y - currentTile.transform.position.y;

                                    if ((yDist > 2f && yDist < 2.5f && xDist > -1 && xDist < 1))
                                    {
                                        tiles.canPlaced = false;
                                        tiles.isBlocked = true;
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case 7:
                foreach (Tile tile in tilesImage)
                {
                    tile.canPlaced = false;
                    float xDist = tile.transform.position.x - currentTile.transform.position.x;
                    float yDist = tile.transform.position.y - currentTile.transform.position.y;
                    // cross move
                    if ((xDist > -1.2f && xDist < 1.2f && yDist > -1 && yDist < 1) ||
                        (yDist > -1.2f && yDist < 1.2f && xDist > -1 && xDist < 1))
                    {
                        tile.canPlaced = true;
                    }
                }
                break;
            case 8:
                foreach (Tile tile in skyTilesImage)
                {
                    float xDist = tile.transform.position.x - currentTile.transform.position.x;
                    float yDist = tile.transform.position.y - currentTile.transform.position.y;

                    //checkerborad move
                    if ((xDist > -1f && xDist < 1f) || (yDist > -1f && yDist < 1f))
                    {
                        tile.canPlaced = true;
                    }
                    else
                    {
                        tile.canPlaced = false;
                    }
                }
                break;
        }


        foreach (Tile tile in tilesImage)
        {
            tile.isBlocked = false;
        }
        foreach (Tile tile in skyTilesImage)
        {
            tile.isBlocked = false;
        }


        if (!sky)
            grid.SetActive(true);
        else
            skyGrid.SetActive(true);

        goldLost = 0;
        minionToPlace = minion;
        minion.minionType = moveType;
    }

    public void ChangeView()
    {
        if (minionToPlace != null)
        {
            minionToPlace = null;
            if (!sky)
                grid.SetActive(false);
            else
                skyGrid.SetActive(false);
            //customCursor.gameObject.SetActive(false);
            //Cursor.visible = false;
        }

        mapChange.SetActive(true);

        if (sky)
        {
            sky = false;
            viewText.text = "View: Ground";
            Camera.main.transform.position = new Vector3(0, 0, -10);
            miniMap01.SetActive(true);
            miniMap02.SetActive(false);
        }
        else
        {
            sky = true;
            viewText.text = "View: Sky";
            Camera.main.transform.position = new Vector3(0, 20, -10);
            miniMap01.SetActive(false);
            miniMap02.SetActive(true);
        }

    }
}

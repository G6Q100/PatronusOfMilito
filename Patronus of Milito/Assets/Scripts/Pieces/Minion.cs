using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Minion : MonoBehaviour
{
    public int minionType;

    public float moveCD, maxCD;

    private void Start()
    {
        switch (minionType)
        {
            case 1:
                if (PlayerPrefs.GetFloat("WarriorCD") == 0)
                    PlayerPrefs.SetFloat("WarriorCD", 1.5f);
                else
                    maxCD = PlayerPrefs.GetFloat("WarriorCD");
                break;
            case 2:
                if (PlayerPrefs.GetFloat("ArcherCD") == 0)
                    PlayerPrefs.SetFloat("ArcherCD", 1.8f);
                else
                    maxCD = PlayerPrefs.GetFloat("ArcherCD");
                break;
            case 3:
                if (PlayerPrefs.GetFloat("RiderCD") == 0)
                    PlayerPrefs.SetFloat("RiderCD", 0.75f);
                else
                    maxCD = PlayerPrefs.GetFloat("RiderCD");
                break;
            case 4:
                if (PlayerPrefs.GetFloat("SkyMageCD") == 0)
                    PlayerPrefs.SetFloat("SkyMageCD", 2f);
                else
                    maxCD = PlayerPrefs.GetFloat("SkyMageCD");
                break;
            case 5:
                if (PlayerPrefs.GetFloat("TowerCD") == 0)
                    PlayerPrefs.SetFloat("TowerCD", 2.5f);
                else
                    maxCD = PlayerPrefs.GetFloat("TowerCD");
                break;
            case 6:
                if (PlayerPrefs.GetFloat("SkyMageCD") == 0)
                    PlayerPrefs.SetFloat("SkyMageCD", 2f);
                else
                    maxCD = PlayerPrefs.GetFloat("SkyMageCD");
                break;
            case 7:
                if (PlayerPrefs.GetFloat("AssassinCD") == 0)
                    PlayerPrefs.SetFloat("AssassinCD", 0.3f);
                else
                    maxCD = PlayerPrefs.GetFloat("AssassinCD");
                break;
            case 8:
                if (PlayerPrefs.GetFloat("SkyDefenceTowerCD") == 0)
                    PlayerPrefs.SetFloat("SkyDefenceTowerCD", 2f);
                else
                    maxCD = PlayerPrefs.GetFloat("SkyDefenceTowerCD");
                break;
        }
    }

    private void Update()
    {
        if (moveCD < maxCD)
        {
            moveCD += Time.deltaTime;
            if (GetComponent<SpriteRenderer>() != null)
                GetComponent<SpriteRenderer>().color = new Color32(15, 100, 200, 128);
        }
        else
        {
            if (GetComponent<SpriteRenderer>() != null)
                GetComponent<SpriteRenderer>().color = new Color32(15, 100, 200, 255);
        }
    }
}

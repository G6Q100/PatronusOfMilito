using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceStat : MonoBehaviour
{
    public Button UpgradeButton, UnlockButton;
    public int lv, soulCost1, soulCost2, unlockCost;
    public float minionStat1, minionStat2, minionStat3;
    public Text minionLv, minionCD, minionCost;
    public string nameLv, nameCD, nameUnlock;
    public int unlock;

    void Awake()
    {
        if (PlayerPrefs.GetInt(nameLv) == 0)
        {
            PlayerPrefs.SetInt(nameLv, 1);
        }
        else
        {
            lv = PlayerPrefs.GetInt(nameLv);
        }

        if (PlayerPrefs.GetInt(nameUnlock) == 0)
        {
            PlayerPrefs.SetInt(nameUnlock, unlock);
            UpgradeButton.gameObject.SetActive(false);
            UnlockButton.gameObject.SetActive(true);
            minionCost.text = "X " + unlockCost;
        }
        else
        {
            unlock = PlayerPrefs.GetInt(nameUnlock);
            UpgradeButton.gameObject.SetActive(true);
            UnlockButton.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (PlayerPrefs.GetInt(nameUnlock) == 0)
        {
            UpgradeButton.gameObject.SetActive(false);
            UnlockButton.gameObject.SetActive(true);
            minionCost.text = "X " + unlockCost;

        }
        else
        {
            UpgradeButton.gameObject.SetActive(true);
            UnlockButton.gameObject.SetActive(false);
        }

        if (name.Contains("Camp") && PlayerPrefs.GetInt(nameUnlock) == 0)
        {
            minionLv.text = lv.ToString();
            minionCD.text = "";
            UpgradeButton.enabled = false;
            minionCost.text = "X " + unlockCost;
        }
        else if (name.Contains("Camp") && PlayerPrefs.GetInt(nameUnlock) == 1)
        {
            minionLv.text = lv.ToString();
            minionCD.text = "";
            UpgradeButton.enabled = false;
            minionCost.text = "Max Level";
        }
        else if(lv == 1 && PlayerPrefs.GetInt(nameUnlock) == 1)
        {
            minionLv.text = lv.ToString();
            minionCD.text = minionStat1.ToString();
            minionCost.text = "X " + soulCost1;
        }
        else if (lv == 2 && PlayerPrefs.GetInt(nameUnlock) == 1)
        {
            minionLv.text = lv.ToString();
            minionCD.text = minionStat2.ToString();
            minionCost.text = "X " + soulCost2;
        }
        else if (lv == 3 && PlayerPrefs.GetInt(nameUnlock) == 1)
        {
            minionLv.text = lv.ToString();
            minionCD.text = minionStat3.ToString();
            UpgradeButton.enabled = false;
            minionCost.text = "Max Level";
        }
    }

    public void Upgrade()
    {
        if (lv == 1 && GameManager.instance.score >= soulCost1)
        {
            GameManager.instance.score -= soulCost1;
            GameManager.instance.scoreText.text = GameManager.instance.score.ToString();
            PlayerPrefs.SetInt(nameLv, 2);
            PlayerPrefs.SetFloat(nameCD, minionStat2);
            minionLv.text = lv.ToString();
            lv++;
            minionCD.text = minionStat2.ToString();
            minionCost.text = "X " + soulCost2;
        }
        else if (lv == 2 && GameManager.instance.score >= soulCost2)
        {
            GameManager.instance.score -= soulCost2;
            GameManager.instance.scoreText.text = GameManager.instance.score.ToString();
            PlayerPrefs.SetInt(nameLv, 3);
            PlayerPrefs.SetFloat(nameCD, minionStat3);
            lv++;
            minionLv.text = lv.ToString();
            minionCD.text = minionStat3.ToString();
            UpgradeButton.enabled = false;
            minionCost.text = "Max Level";
        }
        Debug.Log(PlayerPrefs.GetInt(nameLv));
    }

    public void Unlock()
    {
        if (GameManager.instance.score >= unlockCost)
        {
            GameManager.instance.score -= unlockCost;
            GameManager.instance.scoreText.text = GameManager.instance.score.ToString();
            PlayerPrefs.SetInt(nameUnlock, 1);
            if (PlayerPrefs.GetInt(nameUnlock) == 0)
            {
                UpgradeButton.gameObject.SetActive(false);
                UnlockButton.gameObject.SetActive(true);
            }
            else
            {
                UpgradeButton.gameObject.SetActive(true);
                UnlockButton.gameObject.SetActive(false);
            }
            minionCost.text = "X " + soulCost1;
        }
    }
}

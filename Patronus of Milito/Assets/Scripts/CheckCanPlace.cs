using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckCanPlace : MonoBehaviour
{
    void Update()
    {
        if ((GameManager.instance.sky && !name.Contains("Sky")) || PlayerPrefs.GetInt(name + "Unlock") == 0 || (!GameManager.instance.sky && name.Contains("SkyDefence")))
            GetComponent<Image>().color = new Color32(179, 179, 179, 255);
        else
            GetComponent<Image>().color = new Color32(15, 100, 200, 255);
    }
}

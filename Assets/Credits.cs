using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public static int amount;
    public Text amountText;
    public int defaultMoney = 0;
    void Start()
    {
        amount = PlayerPrefs.GetInt("money", defaultMoney);
    }


    public void increaseMoney(int val)
    {
        amount += val;
        SaveMoney(amount);
    }

    public static void SaveMoney(int value)
    {
        PlayerPrefs.SetInt("money", value);
    }


    public void saveKey(string key)
    {
        PlayerPrefs.SetInt(key, 0);
    }

}

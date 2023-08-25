using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class coinControl : MonoBehaviour
{
     public GameObject coinPrefab;
    public Transform player;
    public TMP_Text goldText;

    private int goldCount ;

    private void Start()
{   
    goldCount=PlayerPrefs.GetInt("Gold",goldCount);
    for (int i = 0; i < 200; i++)
    {
        Vector3 randomPosition = new Vector3(Random.Range(-100f, 100f), 0.5f, Random.Range(-100f, 100f));
        GameObject coinObject = Instantiate(coinPrefab, randomPosition, Quaternion.identity);
        coinObject.tag = "Coin";
    }

    UpdateGoldText();
}

    private void UpdateGoldText()
    {
        goldText.text = "Gold: " + goldCount.ToString();
        PlayerPrefs.SetInt("Gold",goldCount);
    }

    public void CollectGold()
    {
        goldCount++;
        UpdateGoldText();
    }
}

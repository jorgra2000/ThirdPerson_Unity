using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI dayFinalText;
    [SerializeField] private Soldier[] enemies;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject finalPanel;
    [SerializeField] private float timeSpawn;
    private int gold = 0;
    private float time = 0;
    private float limitSoldier = 0;
    private int day = 1;

    public int Gold { get => gold; set => gold = value; }

    private void Start()
    {
        StartCoroutine(Waves());
    }

    public void AddCoin() 
    {
        gold++;
        coinsText.text = gold.ToString();
    }

    public void RestCoins(int coins) 
    {
        gold-= coins;
        coinsText.text = gold.ToString();
    }

    public void EndGame() 
    {
        dayFinalText.text = day + " DÍAS";
        finalPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private Transform RandomPositionSpawn()
    {
        return spawnPoints[(int)Random.Range(0, spawnPoints.Length)];
    }

    private Soldier RandomSoldier() 
    {
        return enemies[(int)Random.Range(0, limitSoldier+1)];
    } 

    public void ChangeDay() 
    {
        day++;
        dayText.text = "DAY: " + day.ToString();
        if(timeSpawn >= 0.5f) 
        {
            timeSpawn -= 0.2f;
        }
        if(day % 3 == 0 && limitSoldier <= 3) 
        {
            limitSoldier++;
        }
    }

    IEnumerator Waves() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(timeSpawn);
            Instantiate(RandomSoldier(), RandomPositionSpawn().position, Quaternion.identity);
        }
    }
}

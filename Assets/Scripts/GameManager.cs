using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    private int gold = 0;

    public int Gold { get => gold; set => gold = value; }

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
}

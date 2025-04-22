using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Building : MonoBehaviour, IInteractable
{
    private bool isBuilt = false;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private int cost;
    [SerializeField] private Mesh buildingComplete;
    [SerializeField] private GameObject textCost;
    [SerializeField] private MonoBehaviour scriptReady;

    public void ShowCost() 
    {
        if(!isBuilt) 
        {
            textCost.SetActive(true);
        }
    }

    public void HideCost()
    {
        if(!isBuilt)
        {
            textCost.SetActive(false);
        }
    }

    public void Interact()
    {
        if(gameManager.Gold >= cost)
        {
            gameManager.RestCoins(cost);
            textCost.SetActive(false);
            isBuilt = true;
            GetComponent<MeshFilter>().mesh = buildingComplete;
            scriptReady.enabled = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    [SerializeField] private Image healthImage;
    [SerializeField] private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthImage.fillAmount = currentHealth / maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LooseHealth(float damage) 
    {
        currentHealth -= damage;
        healthImage.fillAmount = currentHealth / maxHealth;
        if (currentHealth <= 0)
            gameManager.EndGame();
      
    }
}

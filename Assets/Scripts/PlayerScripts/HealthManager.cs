using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    private int damage = 5;
    private int maxHealth = 100;
    public int currentHealth;
    public HealthBarScript healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth == 0)
        {
            // EndGame();
        }
    }
    
    public void TakeDamage()
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        // print(currentHealth);
    }

    public void EndGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

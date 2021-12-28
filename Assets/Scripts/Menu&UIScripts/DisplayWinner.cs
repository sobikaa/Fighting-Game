using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DisplayWinner : MonoBehaviour
{
    public GameObject player1Male; 
    public GameObject player2Female; 
    public HealthManager  p1Health; 
    public HealthManager  p2Health; 
    public TMP_Text winText;
    public TMP_Text DrawText;
    public GameObject winTextParent;

    // Start is called before the first frame update
    void Start()
    {
        winTextParent.SetActive(true);
    }

    void Update (){
        if(p1Health.currentHealth <= 0)
        {
            winTextParent.SetActive(true);

            winText.text = "Player 1 wins!";
            player2Female.SetActive(false);
            Invoke("moveToEndGame", 3);
        }
        else if(p2Health.currentHealth <= 0)
        {
            winTextParent.SetActive(true);
            winText.text = "Player 2 wins!";
            player1Male.SetActive(false);
            Invoke("moveToEndGame", 3);
        }
    }

    void moveToEndGame (){
        p1Health.EndGame();
    }
 
}

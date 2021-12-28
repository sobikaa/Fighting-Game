using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Timer : MonoBehaviour
{
    private float roundLength = 45;
    private bool running = false;
    public TMP_Text timerUI; 
    public PauseMenu PauseMenuScript;
    public HealthManager  p1Health; 
    public HealthManager  p2Health; 
    public TMP_Text winText;
    public TMP_Text DrawText;
    public GameObject winTextParent;

    private void Start()
    {
        // Starts the timer automatically
        running = true;
        timerUI.text = roundLength.ToString();
    }

    void Update()
    {
        if(running){
            if (roundLength > 1 ){
                if(PauseMenu.GameIsPaused == false){
                    roundLength -= Time.deltaTime;
                    float displayTime = Mathf.FloorToInt(roundLength);
                    timerUI.text = displayTime.ToString();
                }
            }
            else{
                running = false; 
                determineWinner();
            }
        }
    }

    void determineWinner(){
        if (p1Health.currentHealth == p2Health.currentHealth){
            winTextParent.SetActive(true);
            winText.text = "Match ends in a DRAW!";
            Invoke("moveToEndGame", 3);
        }
        else if (p1Health.currentHealth > p2Health.currentHealth){
            winTextParent.SetActive(true);
            winText.text = "Player 1 wins!";
            Invoke("moveToEndGame", 3);
        } else {
            winTextParent.SetActive(true);
            winText.text = "Player 2 wins!";
            Invoke("moveToEndGame", 3);
        }

    }

    void moveToEndGame (){
        p1Health.EndGame();
    }

}
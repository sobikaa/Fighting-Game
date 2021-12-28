


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateScreen : MonoBehaviour
{
    public HealthManager p1health;
    public HealthManager p2heatlth;
    [SerializeField] TextMeshProUGUI winText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // CheckWinner();
    }

    private void CheckWinner()
    {
        if(p1health.currentHealth > p2heatlth.currentHealth)
        {
            winText.text = "Player 1 Won \n Player 2 lost";
        }

        if(p2heatlth.currentHealth > p1health.currentHealth)
        {
            winText.text = "Player 2 Won \n Player 1 lost";
        }
    }
}

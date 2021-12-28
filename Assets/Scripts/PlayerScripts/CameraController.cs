using System.Collections;
using System; 
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject playerOne;
    public GameObject playerTwo;
    public GameObject arenaCenter; 

    private float offset;
    private Vector3 currentRotation;
    private float yRotation; 
    private float xRotation; 


    void Start()
    {
        offset = transform.position.x - ((playerOne.transform.position.x + playerTwo.transform.position.x)/2);
    }

    void LateUpdate()
    {
        float distancePlayers = Math.Abs(playerOne.transform.position.x - playerTwo.transform.position.x);
        float playerOneDistCenter = Math.Abs(playerOne.transform.position.x - arenaCenter.transform.position.x);
        float playerTwoDistCenter = Math.Abs(playerTwo.transform.position.x - arenaCenter.transform.position.x);
        float midpoint = (playerOne.transform.position.x + playerTwo.transform.position.x)/2; 

        if (playerOneDistCenter > 5 || playerTwoDistCenter > 5){
            //Debug.Log(playerOneDistCenter);

            yRotation =  midpoint*0.25f;
            currentRotation = new Vector3  (0, yRotation, 0);
            transform.eulerAngles = currentRotation; 
        } 
        if (playerOne.transform.position.y > 0.1){
            xRotation = playerOne.transform.position.y*-0.5f;
            currentRotation = new Vector3  (xRotation, yRotation, 0);
            transform.eulerAngles = currentRotation; 
        }
        if (playerTwo.transform.position.y > 0.1){
            xRotation = playerTwo.transform.position.y*-0.5f;
            currentRotation = new Vector3  (xRotation, yRotation, 0);
            transform.eulerAngles = currentRotation; 
        }

        float zValue = -9.5f -(0.25f * distancePlayers); 
        transform.position = new Vector3  (midpoint+offset, 3.5f, zValue);
    }
}
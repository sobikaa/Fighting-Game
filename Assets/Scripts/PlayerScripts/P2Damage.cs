using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2Damage : MonoBehaviour
{
    public LayerMask collisionLayer;
    public float radius = 1f;
    [SerializeField] private AudioSource audioSound;
    [SerializeField] bool isPlayer1;
    [SerializeField] bool isPlayer2;
    public HealthManager health;

    void Start()
    {
    }

    void DetectCollision()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, radius, collisionLayer);
        
        if(hit.Length > 0)
        {
            if(hit[0].gameObject.name == "Male 2" && !isPlayer1)
            {
                audioSound.Play();
                health.TakeDamage();
                //health.TakeDamage();
                //print("Player1");
            }

            else if(hit[0].gameObject.name == "Female_02" && !isPlayer2)
            {
                audioSound.Play();
                health.TakeDamage();
                //health.TakeDamage();
                //print("Player2");
            }
            print("We hit the " + hit[0].gameObject.name);
            gameObject.SetActive(false);
        }

    }
    void Update()
    {
        DetectCollision();
    }
}

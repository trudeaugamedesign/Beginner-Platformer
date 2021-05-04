using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormEnemy : MonoBehaviour
{
    [Header("References")]
    public PlayerController player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private EnemyHealth eh;

    [Header("Settings")]
    public float movementSpeed;
    public float direction = 1;
    public bool hit;

    // Start is called before the first frame update
    void Start()
    {   
        // Set references
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        eh = gameObject.GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set hit variable to always be equal to the health hit variable
        if (eh.hit != hit){
            hit = eh.hit;
        }
        
        // If the worm is not currently being hit, move 
        if (!hit){
            Move();
        }
    }

    // Handles all movement of the worm
    void Move(){
        // If the velocity goes below a certain threshold, flip the direction
        if (Mathf.Abs(rb.velocity.x) < 0.1f){
            direction = -direction;
            sr.flipX = !sr.flipX;
        }
        
        // Move the worm
        rb.velocity = new Vector2(movementSpeed*direction, rb.velocity.y);
    }
}

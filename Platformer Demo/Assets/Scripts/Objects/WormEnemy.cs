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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        eh = gameObject.GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (eh.hit != hit){
            hit = eh.hit;
        }
        
        if (!hit){
            Move();
        }
    }

    void Move(){
        if (Mathf.Abs(rb.velocity.x) < 0.1f){
            direction = -direction;
            sr.flipX = !sr.flipX;
        }
        rb.velocity = new Vector2(movementSpeed*direction, rb.velocity.y);
    }
}

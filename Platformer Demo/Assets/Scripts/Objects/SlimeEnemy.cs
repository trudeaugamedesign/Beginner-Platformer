using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    [Header("References")]
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerController player;
    private BoxCollider2D bc;
    private SpriteRenderer sr;
    private EnemyHealth eh;
    public LayerMask groundLayer;

    [Header("General Settings")]
    public bool hit;
    public bool canMove;
    public float attackDistance;
    public float jumpAnimationTime;

    [Header("Movement Settings")]
    public float movementSpeed;
    public float jumpHeight;
    public float jumpDelay;


    private void Awake() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        bc = gameObject.GetComponent<BoxCollider2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        eh = gameObject.GetComponent<EnemyHealth>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (eh.hit != hit){
            hit = eh.hit;
        }
        if (canMove && IsGrounded() && !hit){
            if (Vector2.Distance(player.transform.position, transform.position) > attackDistance){
                StartCoroutine(Patrol());
            }   else {
                StartCoroutine(Attack());
            }
        }
        Animations();
    }

    void Animations(){
        
        anim.SetBool("IsGrounded", IsGrounded());
        if (rb.velocity.x > 0 && !IsGrounded()){
            sr.flipX = true;
        }   else if (rb.velocity.x < 0 && !IsGrounded()){
            sr.flipX = false;
        }
    }
    
    bool IsGrounded(){
        // Makes a boxcast downwards and returns if the boxcast hit the ground
        RaycastHit2D raycast = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, 0.5f, groundLayer);
        return raycast;
    }


    float PlayerDirection(){
        return Mathf.Sign(player.transform.position.x - transform.position.x);
    }

    IEnumerator Patrol(){
        // Do not allow slime to move while waiting for the jump delay timer
        canMove = false;
        anim.SetTrigger("Jump");
        yield return new WaitForSeconds(jumpAnimationTime);

        // Make the slime jump
        rb.velocity = new Vector2(movementSpeed * (Random.Range(0,2)*2-1), jumpHeight);
        yield return new WaitForSeconds(jumpDelay);

        // Allow the slime to move again
        canMove = true;
    }

    IEnumerator Attack(){
        // Do not allow slime to move while waiting for the jump delay timer
        canMove = false;
        anim.SetTrigger("Jump");
        yield return new WaitForSeconds(jumpAnimationTime);

        // Make the slime jump
        rb.velocity = new Vector2(movementSpeed * PlayerDirection(), jumpHeight);
        yield return new WaitForSeconds(jumpDelay);

        // Allow the slime to move again
        canMove = true;

    }
    
    
}

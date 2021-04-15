using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Component References")]
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D bc;
    
    [Header("Movement Settings")]
    public bool movementLocked;
    public float movementSpeed;
    public float sprintSpeed;
    public float jumpHeight;

    [Header("Attack Settings")]
    public bool attacking;
    public float attackDamage;
    public float attackSpeed;
    public Transform bowPosition;

    [Header("Health Settings")]
    public int health;
    
    [Header("Prefabs")]
    public GameObject arrow;
    
    void Awake() {
        // Referencing components
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        bc = gameObject.GetComponent<BoxCollider2D>();
    }

    // Raycast under the player to check if the player is grounded
    bool isGrounded(){
        // Makes a boxcast downwards and returns if the boxcast hit the ground
        RaycastHit2D raycast = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, 0.5f, groundLayer);
        return raycast;
    }

    // Update is called once per frame
    void Update()
    {
        // Only allow player movement if movement locked is false
        if (!movementLocked){

            // Run player movement function every frame
            PlayerMovement();

            // Run player attacking function every frame
            PlayerAttack();
        }

        // Run player animation function every frame
        Animations();
    }

    // Control all the animations
    void Animations(){
        // Don't allow certain animations if the player is movement locked
        if (!movementLocked){
            // Check if the player is inputing the movement keys
            if (Input.GetAxisRaw("Horizontal") != 0){

                // If the playing is moving, change the direction of the player to the input
                transform.localScale = new Vector2(Input.GetAxisRaw("Horizontal"),1);
            }

            // Check if the player if moving
            if (rb.velocity.x != 0){

                // If the player is moving, set the player animation to running
                //anim.SetBool("isRun", true);
            }
            
        }
    }

    // Control all the player movement
    void PlayerMovement(){
        // Variable to hold desired velocity
        Vector2 movementVelocity;
        
        // Set the horizontal movement of the player
        if (Input.GetKey(KeyCode.LeftShift)){
            // Sprint speed
            movementVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * movementSpeed, rb.velocity.y);
        }   else {
            // Regular movement speed
            movementVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * movementSpeed, rb.velocity.y);
        }
        // Set the horizontal movement of the player
        
        // If the player is on the ground, and he inputs the jump keys
        if (isGrounded() && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))){
            movementVelocity += Vector2.up * jumpHeight;
        }

        // Set velocity to the stored velocity
        rb.velocity = movementVelocity;
    }

    // Controls all player attacking
    void PlayerAttack(){

        // If player inputs the shooting button and the player is not currently shooting
        if (!attacking && (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButton(0))){

            // Start the attack timer
            StartCoroutine(AttackTimer());
        }
    }

    IEnumerator AttackTimer(){
        attacking = true;
        yield return new WaitForSeconds(attackSpeed);
        
        // Instantiate an arrow and set it to the players current direction
        GameObject currentArrow = Instantiate(arrow, bowPosition.position, bowPosition.rotation);
        currentArrow.GetComponent<ArrowController>().direction = transform.localScale.x;
        attacking = false;
    }

    public IEnumerator OnHit(float damage, float knockbackStrength, float knockbackTime){
        yield return new WaitForSeconds(knockbackTime);
    }
}

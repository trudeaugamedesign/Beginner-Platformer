using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Component References")]
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D bc;
    public GameObject healthBar;
    
    [Header("Movement Settings")]
    public bool movementLocked;
    public float movementSpeed;
    public float jumpHeight;
    public float rollSpeed;
    public float rollTime;
    public float lastInput;

    [Header("Attack Settings")]
    public bool attacking;
    public int attackDamage;
    public float attackKnockbackStrength;
    public float attackSpeed;
    public float attackLungeSpeed;
    public BoxCollider2D attackBox;

    [Header("Health Settings")]
    public int health;
    public float knockbackTime;
    public float recoveryTime;
    public float vKnockbackStrength;
    public float hKnockbackStrength;
    public bool invincible;
    
    
    void Awake() {
        // Referencing components
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        bc = gameObject.GetComponent<BoxCollider2D>();
        
    }

    // Raycast under the player to check if the player is grounded
    bool IsGrounded(){
        // Makes a boxcast downwards and returns if the boxcast hit the ground
        RaycastHit2D raycast = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, 0.3f, groundLayer);
        return raycast;
        
    }

    // Update is called once per frame
    void Update()
    {
        // Run player animation function every frame
        Animations();

        // Only allow player movement if movement locked is false
        if (!movementLocked){

            // Run player movement function every frame
            PlayerMovement();

            // Run player attacking function every frame
            PlayerAttack();
        }


        // Check for collisions from enemies
        CheckHit();

        // Manage health
        ManageHealthAnimation();
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

            // Check if the player if moving and if they want to be moving
            if (rb.velocity.x != 0 && Input.GetAxisRaw("Horizontal") != 0){
                // If the player is moving, set the player animation to running
                anim.SetBool("IsRun", true);

                lastInput = Mathf.Sign(rb.velocity.x);
                
            }   else {
                anim.SetBool("IsRun", false);
            }
            
        }

        anim.SetBool("IsGrounded", IsGrounded());
    }

    // Control all the player movement
    void PlayerMovement(){
        // Variable to hold desired velocity
        Vector2 movementVelocity;
        
        // Set the horizontal movement of the player
        movementVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * movementSpeed, rb.velocity.y);
        
        // If the player is on the ground, and he inputs the jump keys
        if (IsGrounded() && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))){
            movementVelocity += Vector2.up * jumpHeight;
            anim.SetTrigger("Jump");
        }

        // Set velocity to the stored velocity
        rb.velocity = movementVelocity;

        // If the player is on the ground, and he inputs roll keys
        if (IsGrounded() && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.G))){
            StartCoroutine(Roll());
        }
    }

    IEnumerator Roll(){
        movementLocked = true;

        // Move player towards rolling direction
        rb.velocity = new Vector2(rollSpeed * lastInput, rb.velocity.y);

        // Set rolling animation
        anim.SetBool("IsRoll", true);
        yield return new WaitForSeconds(rollTime);

        anim.SetBool("IsRoll", false);
        movementLocked = false;
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
        // Stop player movement and prevent more attacks while on cooldown
        attacking = true;
        movementLocked = true;

        // Set animations
        anim.SetBool("IsRun", false);
        anim.SetTrigger("Attack");

        // Detect enemies in range
        RaycastHit2D raycast = Physics2D.BoxCast(attackBox.bounds.center, attackBox.bounds.size, 0f, Vector2.right, 0f, enemyLayer);

        // If enemy is in range, attack them
        if (raycast){
            float enemyDirection = -Mathf.Sign(transform.position.x - raycast.transform.position.x);
            raycast.collider.GetComponent<EnemyHealth>().StopAllCoroutines();
            StartCoroutine(raycast.collider.GetComponent<EnemyHealth>().Hit(attackDamage, attackKnockbackStrength*enemyDirection));
        }
        
        // Move player towards rolling direction
        rb.velocity = new Vector2(attackLungeSpeed * lastInput, rb.velocity.y);
        yield return new WaitForSeconds(attackSpeed);
        movementLocked = false;
        attacking = false;
        
    }

    void CheckHit(){
        
        RaycastHit2D raycast = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, 0, enemyLayer);
        if (raycast && !invincible && !raycast.collider.GetComponent<EnemyHealth>().dead){
            float dir = Mathf.Sign(transform.position.x - raycast.transform.position.x);
            StartCoroutine(OnHit(dir));
        }
    }
    public IEnumerator OnHit(float dir){
        // Apply knockback
        rb.velocity = new Vector2(hKnockbackStrength * dir, vKnockbackStrength);

        // Stop player movmeent and start i frames
        movementLocked = true;
        invincible = true;

        // Start animations
        anim.SetTrigger("Hurt");
        anim.SetBool("IsRun", false);

        // Reduce health 
        health -= 1;

        // Check if dead
        if (health <= 0){
            anim.SetBool("Dead", true);
            rb.velocity = new Vector2(0, 0);    
        }   else {
            // Wait for the knockback time
            yield return new WaitForSeconds(knockbackTime);

            // Allow player movement after knockback time
            movementLocked = false;

            // Wait for recovery time
            yield return new WaitForSeconds(recoveryTime);

            // Allow player to be hit after recovery
            invincible = false;
        }

    }

    void ManageHealthAnimation(){
        if (health == 0){
            healthBar.transform.GetChild(0).GetComponent<Animator>().SetBool("Empty", true);
            healthBar.transform.GetChild(1).GetComponent<Animator>().SetBool("Empty", true);
            healthBar.transform.GetChild(2).GetComponent<Animator>().SetBool("Empty", true);
        }   else if (health == 1){
            healthBar.transform.GetChild(1).GetComponent<Animator>().SetBool("Empty", true);
            healthBar.transform.GetChild(2).GetComponent<Animator>().SetBool("Empty", true);
        }   else if (health == 2){
            healthBar.transform.GetChild(2).GetComponent<Animator>().SetBool("Empty", true);
        }
    }
}

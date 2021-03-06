using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("References")]
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Health Settings")]
    public int health;
    public bool hit;
    public float deathTime;
    public float knockbackTime;
    public bool dead;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }


    // Coroutine that runs when enemy is damaged
    public IEnumerator Hit(int damage, float knockbackStrength){
        // Tell the enemy script that it is currently being hit
        hit = true;

        // Apply knockback
        rb.velocity = new Vector2(knockbackStrength, Mathf.Abs(knockbackStrength));

        // Reduce heath by the attack damage of the arrow
        health -= damage;


        if (health <= 0){
            // Start death sequence
            StartCoroutine(Dead());
        }   else {
                
            // Play hit animation
            anim.SetTrigger("Hurt");
        }

        yield return new WaitForSeconds(knockbackTime);
        hit = false;
    }

    // Coroutine to be called when the enemy dies
    IEnumerator Dead(){
        // Set the enemy to be dead
        dead = true;
        
        // Play the enemy's death animation
        anim.SetBool("Dead", true);

        // Wait for the death animation to finish
        yield return new WaitForSeconds(deathTime);

        // Destroy the enemy
        Destroy(gameObject);
    }

}

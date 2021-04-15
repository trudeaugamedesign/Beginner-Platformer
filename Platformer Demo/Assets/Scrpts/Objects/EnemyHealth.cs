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
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }


    // Coroutine that runs when enemy is damaged
    public IEnumerator Hit(int damage, float knockbackStrength){
        // Tell the enemy script that it is currently being hit
        hit = true;

        // Apply knockback
        rb.velocity = new Vector2(knockbackStrength, rb.velocity.y);
        // Reduce heath by the attack damage of the arrow
        health -= damage;

        if (health <= 0){
            StartCoroutine(Dead());
        }

        yield return new WaitForSeconds(knockbackTime);
        hit = false;
    }

    // Coroutine to be called when the enemy dies
    IEnumerator Dead(){

        yield return new WaitForSeconds(deathTime);
    }

}

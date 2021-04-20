using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnemy : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    private Rigidbody2D rb;
    
    [Header("General Setting")]
    public float movementSpeed;

    [Header("Patrol Settings")]
    public bool patrolling;
    public float patrolDelayTime;
    public float currentTarget;
    public float pointA;
    public float pointB;
    public float threshold;    

    [Header("Attack Settings")]
    public float engageDistance;
    public float attackDamage;
    public float attackSpeed;
    public float chargeDistance;
    public float chargeSpeed;
    public float chargeTime;
    public bool charging;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
        currentTarget = pointA;
    }

    // Update is called once per frame
    void Update()
    {
        float playerDistance = Vector2.Distance(transform.position, player.transform.position);
        // States

        // If the player is in the charge range, charge
        if (!charging && playerDistance < chargeDistance){
            // Start charge coroutine
            StartCoroutine(Charge());
        }
        // If the player is not in charge range, move towards the player   
        else if (!charging && playerDistance < engageDistance){
            MoveTowardsPlayer();
        }
        // If the player is not in range to attack or charge, just patrol
        else if (!charging && !patrolling){
            Patrol();
        }
    }

    IEnumerator Patrol(){
        if(Mathf.Abs(transform.position.x - currentTarget) > threshold){
            rb.velocity = new Vector2(movementSpeed * Mathf.Sign(transform.position.x - currentTarget), rb.velocity.y);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(patrolDelayTime);
        if (currentTarget == pointA){
            currentTarget = pointB;
        }else {
            currentTarget = pointA;
        }
    }   
    IEnumerator bruh(){
        yield return new WaitForSeconds(10);
    }
    

    // Move towards the player
    void MoveTowardsPlayer(){
        float playerDirection = Mathf.Sign(player.transform.position.x - transform.position.x);

        rb.velocity = new Vector2(movementSpeed * playerDirection, rb.velocity.y);
    }

    IEnumerator Charge(){
        charging = true;
        rb.velocity = new Vector2(movementSpeed * Mathf.Sign(transform.position.x - currentTarget), rb.velocity.y);
        yield return new WaitForSeconds(chargeTime);
        charging = false;
    }
}

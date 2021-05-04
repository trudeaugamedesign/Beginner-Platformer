using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{   
    [Header("References")]
    public GameManager gm;
    
    // Setting references
    private void Awake() {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Function is called when a trigger collider is interacted with
    private void OnTriggerEnter2D(Collider2D other) {
        // Check if the collider was a player
        if (other.tag == "Player"){
            // Change the respawn point
            gm.OnChangeRespawnPoint(gameObject.name);
        }
    }
}

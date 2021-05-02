using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public GameManager gm;
    
    
    private void Awake() {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player"){
            gm.OnChangeRespawnPoint(gameObject.name);
        }
    }
}

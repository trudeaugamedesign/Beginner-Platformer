using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScript : MonoBehaviour
{
    [Header("References")]
    public DialogueManager dm;

    [Header("Dialogue Settings")]
    public string[] dialogue;
    public bool playerInRange;
    public bool displayingText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E) && !displayingText){
            dm.dialogue = dialogue;
            StartCoroutine(dm.StartDialogue());
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player"){
            playerInRange = true;
        }
    }   
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player"){
            playerInRange = false;
        }
    }
}

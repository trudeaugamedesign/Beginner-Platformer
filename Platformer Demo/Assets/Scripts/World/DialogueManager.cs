using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("References")]
    public TMP_Text text;
    private Animator anim;
    private GameObject player;

    [Header("General Settings")]
    public bool displayingText;
    public string[] dialogue;
    public int currentDialogueIndex;

    [Header("Animation Settings")]
    public float textStartTime;
    public float textEndTime;
    public float textAnimTime;

    // Start is called before the first frame update
    void Start()
    {
        // Get initial references
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Displaying current dialogue
        DisplayDialogue();
    }

    // Displaying Dialogue
    void DisplayDialogue(){
        // Only display text when needed
        if (displayingText){
            text.text = dialogue[currentDialogueIndex-1];

            // When displaying text, check for input to continue or end text
            if (Input.GetKeyDown(KeyCode.E)){
                if (currentDialogueIndex >= dialogue.Length){
                    StartCoroutine(EndDialogue());
                }   else {
                    StartCoroutine(TextAnimation());
                }
            }

        }   
    }

    // Coroutine that runs to start dialogue
    public IEnumerator StartDialogue(){
        // Run animation to show text
        anim.SetBool("ShowText", true);

        // Set text to display the first set of text
        text.text = dialogue[0];

        // Set the current dialogue index as the first dialogue
        currentDialogueIndex = 1;
    
        // Lock player movement
        player.GetComponent<PlayerController>().movementLocked = true;

        // Wait for tehe time it takes for the animation to run
        yield return new WaitForSeconds(textStartTime);

        // Start displaying text and checking for input
        displayingText = true;
    }

    // Coroutine that runs to end dialogue
    public IEnumerator EndDialogue(){
        // Run animation to hide text
        anim.SetBool("ShowText", false);
        
        // Unlock player movement
        player.GetComponent<PlayerController>().movementLocked = false;

        // Wait for the text animation end time
        yield return new WaitForSeconds(textStartTime);
        displayingText = false;
        text.text = "";
    }
    public IEnumerator TextAnimation(){
        anim.SetTrigger("ChangeText");
        yield return new WaitForSeconds(textAnimTime);
        currentDialogueIndex += 1;
    }


}

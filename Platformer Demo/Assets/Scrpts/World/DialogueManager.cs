using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("References")]
    public TMP_Text text;
    private Animator anim;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisplayDialogue(){
        if (displayingText){
            text.text = dialogue[currentDialogueIndex];
            if (Input.GetKeyDown(KeyCode.E)){
                StartCoroutine(TextAnimation());
            }
        }
    }

    public IEnumerator StartDialogue(){
        anim.SetBool("ShowText", true);
        yield return new WaitForSeconds(textStartTime);
        displayingText = true;
        currentDialogueIndex = 0;
    }

    public IEnumerator EndDialogue(){
        
        anim.SetBool("ShowText", false);
        yield return new WaitForSeconds(textStartTime);
        displayingText = true;
        currentDialogueIndex = 0;
    }
    public IEnumerator TextAnimation(){
        yield return new WaitForSeconds(textAnimTime);
        currentDialogueIndex += 1;
    }


}

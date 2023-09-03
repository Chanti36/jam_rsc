using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCtrl : MonoBehaviour
{
    private DialogueCtrl dialogueCtrl;

    [SerializeField] private float txtSpeed;

    [HideInInspector] public Text nameTxt, dialogueTxt;

    private string[] sentences;
    int dialogueIndex;
    bool onDialogue = false;

    private void Awake()
    {
        dialogueCtrl = GetComponent<DialogueCtrl>();
        onDialogue = false;
    }

    private void Update()
    {
        if (!onDialogue) return;

        if (Input.GetMouseButtonDown(0) && (dialogueTxt.text.Length >= sentences[dialogueIndex].Length))//to pass to new txt need to click
        {
            dialogueIndex++;
            DisplayNextSentence();
        }
        //when hold txt goes fast
        if (Input.GetMouseButton(0)         ||
            Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow)||
            Input.GetKey(KeyCode.UpArrow)   ||
            Input.GetKey(KeyCode.DownArrow))
                                        txtSpeed = 0.1f;  
        else                            txtSpeed = 0.2f;
    }

    //Setup
    public void StartDialogue(Dialogue dialogue, string name)
    {
        //setup
        sentences           = dialogue.sentences;
        nameTxt.text        = name;
        dialogueTxt.text    = "";
        dialogueIndex       = 0;
        //activate
        onDialogue = true;
        //start
        DisplayNextSentence();
    }
    
    //LOOP
    public void DisplayNextSentence()
    {
        if (dialogueIndex >= sentences.Length)
        {
            //end dialogue
            EndDialogue();
            return;
        }
        dialogueTxt.text = "";
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in sentences[dialogueIndex].ToCharArray())
        {
            dialogueTxt.text += c;
            yield return new WaitForSeconds(txtSpeed);
        }
    }

    void EndDialogue()
    {
        StartCoroutine(dialogueCtrl.EndDialogue());
        onDialogue = false;
    }
}
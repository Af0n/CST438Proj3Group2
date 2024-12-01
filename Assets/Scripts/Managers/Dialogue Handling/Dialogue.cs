using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{

    public DialogueManager dialogueManager;

    public void RunDialogue()
    {
        dialogueManager.StartDialogue();
    }

}

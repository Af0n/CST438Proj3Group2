using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{

    // Refence to the dialogue manager
    public DialogueManager dialogueManager;

    // Refences the specific table name a sting asset is in
    public string tableName;

    // Ref to all the possible key names
    public string[] keyNames;

    // Simple function that manages the dialogue
    public void RunDialogue()
    {
        dialogueManager.StartDialogue(tableName,keyNames[Random.Range(0, keyNames.Length)]);
    }

}

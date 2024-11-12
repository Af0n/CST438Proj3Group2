using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Using this tutorial as a base for the dialogue system
// https://youtu.be/8oTYabhj248?si=eKbt5QoFIOFK5mr0

public class Dialogue : MonoBehaviour
{

    // reference to the tmp ugui element
    public TextMeshProUGUI textComponent;

    // ref to the lines that the dialogue meanager needs to show off
    public string[] lines;

    // text speed of the current piece of dialogue
    public float textSpeed;

    // index of the line getting ouputted
    private int index;

    // Start is called before the first frame update
    void Start()
    {

        // makes sure the text in the text component i s empty
        textComponent.text = string.Empty;
        
        // Calls the start dialogue cript to get things goin
        StartDialogue();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartDialogue ()
    {

        // sets the index back to 0
        index = 0;

        // Starts the coroutine for the main type line system
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        // Type each character 1 by 1
        foreach ( char c in lines[index].ToCharArray() )
        {

            // adds the current character to the text component
            textComponent.text += c;

            // waits a moment before adding a new character
            yield return new WaitForSeconds(textSpeed);


        }

    }
}

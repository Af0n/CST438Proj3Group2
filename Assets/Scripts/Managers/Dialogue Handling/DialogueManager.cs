using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using TMPro;

// Using this tutorial as a base for the dialogue system
// https://youtu.be/8oTYabhj248?si=eKbt5QoFIOFK5mr0

public class DialogueManager : MonoBehaviour
{

    // Reference to the whole text box object
    public GameObject textBoxParent;

    // Reference to the actual textbox
    public GameObject textBox;

    // reference to the tmp ugui element
    public TextMeshProUGUI textComponent;

    // ref to the lines that the dialogue meanager needs to show off
    public string[] lines;

    // base text speed the code should refer to
    public float textSpeed;

    // text speed of the current piece of dialogue
    public float currentTextSpeed;

    // index of the line getting ouputted
    private int index;

    // Start is called before the first frame update
    void Start()
    {

        // makes sure the text in the text component i s empty
        textComponent.text = string.Empty;

        // sets the current text speed to the original text speed
        currentTextSpeed = textSpeed;

        // Calls the start dialogue script to get things goin
        // StartDialogue();

    }

    // Update is called once per frame
    void Update()
    {
        // If the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            // If the text component has been completed, then it will move to the next line
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                // Modifies the speed of the how fast text can go
                currentTextSpeed = textSpeed / 2f;
            }
        }

        // Resets the text speed when the mouse buttong gets released
        if (Input.GetMouseButtonUp(0))
        {
            currentTextSpeed = textSpeed;
        }
    }

    public void StartDialogue()
    {

        textBoxParent.SetActive(true);
        isYappingFlip();

        // sets the index back to 0
        index = 0;

        // Starts the coroutine for the main type line system
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        // Type each character 1 by 1
        foreach (char c in lines[index].ToCharArray())
        {

            // adds the current character to the text component
            textComponent.text += c;

            // waits a moment before adding a new character
            yield return new WaitForSeconds(currentTextSpeed);

        }

    }

    // Function that moves onto the next line if there are any
    void NextLine()
    {

        // If there are more lines, then the y will be printed out in the dialogue box
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        // If there are no more line, then the text box will get deactivate
        else
        {
            isYappingFlip();
            textBoxParent.SetActive(false);
        }

    }

    // Handles the flipping of player states when it comes to dialogue
    void isYappingFlip()
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerManager>().flipYapping();
    }

}

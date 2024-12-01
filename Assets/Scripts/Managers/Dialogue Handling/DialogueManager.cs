using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;
using TMPro;

// Using this tutorial as a base for the dialogue system
// https://youtu.be/8oTYabhj248?si=eKbt5QoFIOFK5mr0

public class DialogueManager : MonoBehaviour
{
    // Ref to the player controller
    public PlayerController playerController;

    // Reference to the whole text box object
    public GameObject textBoxParent;

    // Reference to the actual textbox
    public GameObject textBox;

    // reference to the tmp ugui element
    public TextMeshProUGUI textComponent;

    // ref to the lines that the dialogue meanager needs to show off
    private string line;

    // base text speed the code should refer to
    public float textSpeed;

    // text speed of the current piece of dialogue
    public float currentTextSpeed;

    // index of the line getting ouputted
    private int index;
    
    // Bool to track the stae of the dialogue
    private bool startedDialogue = false;

    // Refernce to the table we are currently looking at
    private string tableName = "";

    // Ref to the key in the table we're currently looking at
    private string keyName = "";

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
        // Will only check when the dialogue box is open
        if (startedDialogue)
        {
            // If the left mouse button is pressed
            if (Input.GetMouseButtonDown(0) || playerController.actionTriggered())
            {
                // If the text component has been completed, then it will move to the next line
                if (textComponent.text == line)
                {
                    // Increases the index for later use
                    index++;

                    // Then calls the next line to get started!
                    NextLine();
                }
                else
                {
                    // Modifies the speed of the how fast text can go
                    currentTextSpeed = textSpeed / 2f;
                }
            }

            // Resets the text speed when the mouse buttong gets released
            if (Input.GetMouseButtonUp(0) || playerController.actionTriggered())
            {
                currentTextSpeed = textSpeed;
            }
        }

    }

    public void StartDialogue(string tempTableName, string tempKeyName)
    {
        startedDialogue = true;
        textBoxParent.SetActive(true);
        isYappingFlip();

        // sets the index back to 0
        index = 0;

        // Stores the given variables
        tableName = tempTableName;
        keyName = tempKeyName;

        NextLine();
    }

    IEnumerator TypeLine()
    {
        // Type each character 1 by 1
        foreach (char c in line.ToCharArray())
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

        // Attempts to load a line of dialogue
        StartCoroutine(LoadLocalizedString());

        // If the dialogue is still running, that means a new string has been found
        // therefore, we can load in the new line
        if (startedDialogue)
        {
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
            
        }

    }

    private IEnumerator LoadLocalizedString()
    {

        // first attempts to see if the table exists
        var table = LocalizationSettings.StringDatabase.GetTable(tableName);

        // If the table exists...
        if (table != null)
        {

            // It will then attempt to find the string
            var localizedString = table.GetEntry(keyName + index);

            if (localizedString != null)
            {

                // If a string was found, it will be chucked into the line string, and then will be used later
                line = localizedString.GetLocalizedString();

            }
            else
            {

                // If we don't find anything, and it's the first index, that means that there is some error with the key
                if (index == 0)
                {
                    Debug.Log("The key " + keyName + " was not found!");
                }

                endDialogue();

            }

        }
        else
        {

            Debug.Log("The table " + tableName + " was not found!");
            endDialogue();

        }

        yield return null;
    }

    void endDialogue()
    {

        // If there are no more lines, then the text box will get deactivate
        isYappingFlip();
        startedDialogue = false;
        textBoxParent.SetActive(false);

    }

    // Handles the flipping of player states when it comes to dialogue
    void isYappingFlip()
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerManager>().flipYapping();
    }

}

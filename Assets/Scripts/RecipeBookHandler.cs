using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBookHandler : MonoBehaviour
{

    public GameObject[] allPages;
    public int index = 0;

    public void goRight()
    {

        allPages[index].SetActive(false);

        index++;

        if (index > allPages.Length - 1)
        {
            index = 0;
        }

        allPages[index].SetActive(true);

    }

    public void goLeft()
    {

        allPages[index].SetActive(false);

        index--;

        if (0 > index)
        {
            index = allPages.Length - 1;
        }

        allPages[index].SetActive(true);

    }



}

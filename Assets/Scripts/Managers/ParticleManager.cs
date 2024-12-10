using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartcleManager : MonoBehaviour
{
    [HideInInspector]
    public static PartcleManager instance;
    public enum PartcleType
    {
        DustLeft,
        DustRight,
    }
    public GameObject[] partcles;
    public void Awake()
    {
        // check for any other inst. 
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // Okay then init
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void makePartcleFX(PartcleType type, GameObject parent)
    {
        switch (type)
        {
            case PartcleType.DustLeft:
                Instantiate(partcles[(int)PartcleType.DustLeft], parent.transform);
                break;
            case PartcleType.DustRight:
                Instantiate(partcles[(int)PartcleType.DustRight], parent.transform);
                break;
            default:
                break;
        }
    }
    public void makePartcleFX(PartcleType type, Transform pos)
    {
        makePartcleFX(type, transform.position);
    }
    public void makePartcleFX(PartcleType type, Vector3 pos)
    {
        switch (type)
        {
            case PartcleType.DustLeft:
                Instantiate(partcles[(int)PartcleType.DustLeft], pos, Quaternion.identity);
                break;
            case PartcleType.DustRight:
                Instantiate(partcles[(int)PartcleType.DustRight], pos, Quaternion.identity);
                break;
            default:
                break;
        }
    }
}

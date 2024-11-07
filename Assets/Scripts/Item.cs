using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]

public class Item : MonoBehaviour
{
    public ItemSprites itemSprites;
    public ItemType type;
    public bool isHeld;
    public bool debug;

    private SpriteRenderer spriteRenderer;
    private int typeCode;
    private Rigidbody rb;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();

        ChangeItem(type);
    }

    private void Update()
    {
        // debugging all the sprites
        if (debug)
        {
            ChangeItem(type);
        }
    }

    // not intended to be used as a pickup script.
    // does things to the item when picked up
    public void PickUp()
    {
        isHeld = true;
        rb.isKinematic = true;
    }

    // not intended to be used as a pickup script.
    // does things to the item when dropped
    public void Drop()
    {
        isHeld = false;
        rb.isKinematic = false;
    }

    public void SetSprite(Sprite set)
    {
        spriteRenderer.sprite = set;
    }

    // handles all variables when changing item to a new type
    // use when spawning a new item and set it to the appropriate type
    public void ChangeItem(ItemType newType)
    {
        type = newType;
        typeCode = GetItemTypeCode();
        SetSprite(itemSprites.sprites[typeCode]);
    }

    // gets the numerical value of the item's type
    // an enum doesnt actually equal a number, the value is used for ordering in Unity editor
    public int GetItemTypeCode()
    {
        switch (type)
        {
            case ItemType.BUCKET:
                return 0;
            case ItemType.SALTWATER:
                return 1;
            case ItemType.FRESH_APPLE:
                return 2;
            case ItemType.RAW_RUBY:
                return 3;
            case ItemType.DISORDERED_SPIRIT:
                return 4;
            case ItemType.SPARKLING_APPLE:
                return 5;
            case ItemType.MINOR_SPIRIT:
                return 6;
            case ItemType.PURE_WATER:
                return 7;
            case ItemType.SALT:
                return 8;
            case ItemType.SPARKLING_RUBY:
                return 9;
            case ItemType.GEMSTONE_DUST:
                return 10;
            case ItemType.POTION_CALM:
                return 11;
            case ItemType.POTION_HEAL:
                return 12;
            case ItemType.POTION_MANA:
                return 13;
            default:
                Debug.Log("Could Not Find Type");
                return -1;
        }
    }

    public void ToggleDebug(){
        debug = !debug;
    }
}

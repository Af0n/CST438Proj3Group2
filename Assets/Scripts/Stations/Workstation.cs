using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

[RequireComponent(typeof(SpriteRenderer))]

public class Workstation : MonoBehaviour
{
    [Header("Station Stats")]
    [Tooltip("Determines the type of workstation")]
    public StationType type;
    [Tooltip("Number of ticks it takes to process an ingredient")]
    public int processTime;
    [Tooltip("How strongly the station launches the item when rejected")]
    public float rejectVel;

    [Header("Unity Setup")]
    public StationSprites stationSprites;
    public GameObject prefab;
    public Recipes recipes;
    public StationInput inputs;

    // the child object that determines where the item will be 'held'
    private Transform itemPos;
    private SpriteRenderer spriteRenderer;
    private Brewing brewing;
    private int stationRecipeIndex;

    // type of station
    private int typeCode;

    // used for timing
    private int tickTimer;

    // So Essentaully this is a DP problem!
    private Dictionary<ItemType, int> _price_sheet = new Dictionary<ItemType, int>();
    private ConcurrentQueue<ItemType> _items_price_queue = new ConcurrentQueue<ItemType>(); 

    public int TickTimer
    {
        get { return tickTimer; }
    }

    public bool HasItem
    {
        get { return itemPos.childCount != 0; }
    }

    public bool IsProcessing
    {
        get { return tickTimer != 0; }
    }

    public Transform ProcessingItem
    {
        get
        {
            if (HasItem)
            {
                return itemPos.GetChild(0);
            }
            return null;
        }
    }

    private void OnEnable()
    {
        TickSystem.OnTick += Tick;
    }

    private void OnDisable()
    {
        TickSystem.OnTick -= Tick;
    }


    private void Awake()
    {
        brewing = GetComponent<Brewing>();
        tickTimer = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemPos = transform.GetChild(0);
        SetStation(type);
    }

    public bool SetItem(Transform t)
    {
        // don't accept item if already one
        if (HasItem)
        {
            return false;
        }

        t.GetComponent<PickUp>().Pick(itemPos);
        tickTimer = 0;
        return true;
    }

    // helper fucntion to check all input recipes to see if the item set is processable
    // returns index of found recipe
    // -1 if no match
    private int ValidInput(ItemType t)
    {
        int count = -1;
        foreach (StationRecipe recipe in inputs.recipes)
        {
            count++;
            // checks if the current recipe's input is the same as the given item
            if (recipe.input == t)
            {
                return count;
            }
        }
        //got through all recipes with no match
        return -1;
    }

    public void Tick()
    {
        // dont do anything if no item
        if (!HasItem)
        {
            tickTimer = 0;
            return;
        }

        // yes, i know this next bit runs a foreach every tick, but I wanted the item to successfully
        // teleport before being rejected
        // easiest way to do it would be to let the station check after a tick

        Item itemScript = ProcessingItem.GetComponent<Item>();

        // checking if item is invalid
        stationRecipeIndex = ValidInput(itemScript.Type);
        //Debug.Log(stationRecipeIndex);

        // reject item from station
        if (stationRecipeIndex == -1)
        {
            Reject(ProcessingItem);
            return;
        }

        tickTimer += 1;
        //Debug.Log(tickTimer);

        // Not our time yet.. 
        if (tickTimer < processTime)
        {
            return;
        }

        tickTimer = 0;

        ProcessItem();
    }

    // assumes the item is processable at this station
    // also assumes we HAVE an item to process
    public void ProcessItem()
    {
        if (type == StationType.MIXING)
        {
            // lets boiling script handle things
            return;
        }

        if (type == StationType.SELLING)
        {
            Sell(ProcessingItem.GetComponent<Item>());
            return;
        }

        StationRecipe recipe = inputs.recipes[stationRecipeIndex];
        Destroy(ProcessingItem.gameObject);
        foreach (ItemType t in recipe.output)
        {
            SpawnAtItemPos(t);
        }
    }

    private void Reject(Transform item)
    {
        // rejection launch
        item.GetComponent<PickUp>().Drop();
        Vector2 randDir = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
        randDir.Normalize();
        randDir *= rejectVel;
        item.GetComponent<Rigidbody2D>().AddForce(randDir, ForceMode2D.Impulse);
    }

    private bool Sell(Item item)
    {
        if (!item.CanSell)
        {
            return false;
        }
        if (!_price_sheet.ContainsKey(item.Type))
        {
            _price_sheet.Add(item.Type, item.price);
            Debug.Log("Adding new key");
        }
        else
        {
            float rand = UnityEngine.Random.Range(0f, 1f); // Random value between 0 and 1
            if (rand <= 0.8f) // Adjusted condition
            {
                // Lower the price, ensuring it doesn't go below 1
                _price_sheet[item.Type] = (int)Mathf.Clamp(_price_sheet[item.Type] - 1, 1, float.MaxValue); // Adjusting price
                Debug.Log(_price_sheet[item.Type]);
                _items_price_queue.Enqueue(item.Type);
            }
        }
        if(_items_price_queue.TryPeek(out ItemType result) && result != item.Type) {
            // Don't bother if its the same item.. 
            if(result == item.Type) return item.Sell(_price_sheet[item.Type]);
            // For now.. this might be the easiest way without day night cycle
            float rand = UnityEngine.Random.Range(0f, 1f); // Random value between 0 and 1
            if (rand >= 0.5f) {
                _price_sheet[result]++;
            }
        }
        return item.Sell(_price_sheet[item.Type]);
    }

    // private bool Grind(Item item)
    // {
    //     switch (item.Type)
    //     {
    //         case ItemType.SPARKLING_RUBY:
    //             item.ChangeItem(ItemType.GEMSTONE_DUST);
    //             break;
    //         default:
    //             Debug.Log("Cannot Grind Item");
    //             return false;
    //     }

    //     return true;
    // }

    // private bool Clean(Item item)
    // {
    //     switch (item.Type)
    //     {
    //         case ItemType.FRESH_APPLE:
    //             item.ChangeItem(ItemType.SPARKLING_APPLE);
    //             break;
    //         case ItemType.RAW_RUBY:
    //             item.ChangeItem(ItemType.SPARKLING_RUBY);
    //             break;
    //         case ItemType.SALT:
    //             item.ChangeItem(ItemType.SALTWATER);
    //             break;
    //         default:
    //             Debug.Log("Cannot Clean Item");
    //             return false;
    //     }

    //     return true;
    // }

    // private bool Boil(Item item)
    // {
    //     switch (item.Type)
    //     {
    //         case ItemType.SALTWATER:
    //             item.ChangeItem(ItemType.PURE_WATER);

    //             SpawnAtItemPos(ItemType.SALT);
    //             break;
    //         case ItemType.PURE_WATER:
    //             item.ChangeItem(ItemType.BUCKET);
    //             break;
    //         default:
    //             Debug.Log("Cannot Boil Item");
    //             return false;
    //     }

    //     return true;
    // }

    // private bool Arcane(Item item)
    // {
    //     switch (item.Type)
    //     {
    //         case ItemType.DISORDERED_SPIRIT:
    //             item.ChangeItem(ItemType.MINOR_SPIRIT);
    //             break;
    //         // don't keep this one
    //         // we'll probably get saltwater from the ocean?
    //         case ItemType.BUCKET:
    //             item.ChangeItem(ItemType.SALTWATER);
    //             break;
    //         default:
    //             Debug.Log("Cannot Do Magic With Item");
    //             return false;
    //     }

    //     return true;
    // }

    private void SpawnAtItemPos(ItemType t)
    {
        // spawning item at pos
        // adds a bit of randomness to the horizontal position of the spawn
        GameObject obj = Instantiate(prefab, itemPos.position + UnityEngine.Random.Range(-0.1f, 0.1f) * Vector3.right, Quaternion.identity);
        obj.GetComponent<Item>().ChangeItem(t);
    }

    public void SetStation(StationType t)
    {
        typeCode = GetStationTypeCode(t);
        spriteRenderer.sprite = stationSprites.sprites[typeCode];
    }

    // gets the numerical value for the type of station
    public static int GetStationTypeCode(StationType t)
    {
        switch (t)
        {
            case StationType.GRINDSTONE:
                return 0;
            case StationType.MIXING:
                return 1;
            case StationType.CLEANSING:
                return 2;
            case StationType.BOILING:
                return 3;
            case StationType.CONJURATION:
                return 4;
            case StationType.SELLING:
                return 5;
            default:
                Debug.Log("Could Not Find Type");
                return -1;
        }
    }
}

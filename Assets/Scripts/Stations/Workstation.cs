using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Workstation : MonoBehaviour
{
    [Header("Station Stats")]
    [Tooltip("Determines the type of workstation")]
    public StationType type;
    [Tooltip("Number of ticks it takes to process an ingredient")]
    public int processTime;

    [Header("Unity Setup")]
    public StationSprites stationSprites;
    public GameObject prefab;

    [Header("Testing")]
    [Tooltip("Used for testing while tick system isn't implemented. Set to -1 to disable fake ticks.")]
    public float testTickTime;

    // the child object that determines where the item will be 'held'
    private Transform itemPos;
    private SpriteRenderer spriteRenderer;
    private int typeCode;
    private int tickTimer;

    public bool HasItem
    {
        get { return itemPos.childCount != 0; }
    }

    public bool IsProcessing
    {
        get { return tickTimer != 0; }
    }

    private void Awake()
    {
        tickTimer = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemPos = transform.GetChild(0);
        SetStation(type);
    }

    private void Start()
    {
        // check if we should do test cycling
        if (testTickTime < 0)
        {
            return;
        }

        StartCoroutine("TestProcessCycle");
    }

    private IEnumerator TestProcessCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(testTickTime);
            Tick();
        }
    }

    public void SetItem(Transform t)
    {
        // TODO: DISABLING PICKUP UNTIL DONE
        t.GetComponent<PickUp>().Pick(itemPos);
        tickTimer = processTime;
    }

    public void Tick()
    {
        tickTimer--;

        // dont do anything if no item
        if (!HasItem)
        {
            return;
        }

        // don't do anything if still processing
        if (tickTimer > 0)
        {
            return;
        }

        ProcessItem(itemPos.GetChild(0).GetComponent<Item>());
    }

    // assumes the item is processable at this station
    // also assumes we HAVE an item to process
    public void ProcessItem(Item item)
    {
        switch (type)
        {
            case StationType.GRINDSTONE:
                Grind(item);
                break;
            case StationType.MIXING:
                Mix(item);
                break;
            case StationType.CLEANSING:
                Clean(item);
                break;
            case StationType.BOILING:
                Boil(item);
                break;
            case StationType.CONJURATION:
                Arcane(item);
                break;
            default:
                Debug.Log("Could Not Process Item");
                break;
        }

        item.GetComponent<PickUp>().Drop();
    }

    private void Grind(Item item)
    {
        switch (item.Type)
        {
            case ItemType.SPARKLING_RUBY:
                item.ChangeItem(ItemType.GEMSTONE_DUST);
                return;
            default:
                Debug.Log("Cannot Grind Item");
                return;
        }
    }

    private void Mix(Item item)
    {

    }

    private void Clean(Item item)
    {
        switch (item.Type)
        {
            case ItemType.FRESH_APPLE:
                item.ChangeItem(ItemType.SPARKLING_APPLE);
                return;
            case ItemType.RAW_RUBY:
                item.ChangeItem(ItemType.SPARKLING_RUBY);
                return;
            case ItemType.SALT:
                item.ChangeItem(ItemType.SALTWATER);
                return;
            default:
                Debug.Log("Cannot Clean Item");
                return;
        }
    }

    private void Boil(Item item)
    {
        switch (item.Type)
        {
            case ItemType.SALTWATER:
                item.ChangeItem(ItemType.PURE_WATER);

                SpawnAtItemPos(ItemType.SALT);
                return;
            case ItemType.PURE_WATER:
                item.ChangeItem(ItemType.BUCKET);
                return;
            default:
                Debug.Log("Cannot Boil Item");
                return;
        }
    }

    private void Arcane(Item item)
    {
        switch (item.Type)
        {
            case ItemType.DISORDERED_SPIRIT:
                item.ChangeItem(ItemType.MINOR_SPIRIT);
                return;
            // don't keep this one
            // we'll probably get saltwater from the ocean?
            case ItemType.BUCKET:
                item.ChangeItem(ItemType.SALTWATER);
                return;
            default:
                Debug.Log("Cannot Do Magic With Item");
                return;
        }
    }

    private void SpawnAtItemPos(ItemType t)
    {
        // spawning item at pos
        // adds a bit of randomness to the horizontal position of the spawn
        GameObject obj = Instantiate(prefab, itemPos.position + Random.Range(-0.1f, 0.1f) * Vector3.right, Quaternion.identity);
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
            default:
                Debug.Log("Could Not Find Type");
                return -1;
        }
    }
}

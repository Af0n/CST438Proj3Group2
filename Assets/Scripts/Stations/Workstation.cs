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
    [Tooltip("How strongly the station launches the item when rejected")]
    public float rejectVel;

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
        bool success;
        switch (type)
        {
            case StationType.GRINDSTONE:
                success = Grind(item);
                break;
            case StationType.MIXING:
                success = Mix(item);
                break;
            case StationType.CLEANSING:
                success = Clean(item);
                break;
            case StationType.BOILING:
                success = Boil(item);
                break;
            case StationType.CONJURATION:
                success = Arcane(item);
                break;
            default:
                Debug.Log("Could Not Process Item");
                success = false;
                break;
        }

        item.GetComponent<PickUp>().Drop();

        if(success){
            return;
        }

        Vector2 randDir = new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f));
        randDir.Normalize();
        randDir *= rejectVel;
        item.GetComponent<Rigidbody2D>().AddForce(randDir, ForceMode2D.Impulse);
    }

    private bool Grind(Item item)
    {
        switch (item.Type)
        {
            case ItemType.SPARKLING_RUBY:
                item.ChangeItem(ItemType.GEMSTONE_DUST);
                break;
            default:
                Debug.Log("Cannot Grind Item");
                return false;
        }

        return true;
    }

    private bool Mix(Item item)
    {
        return false;
    }

    private bool Clean(Item item)
    {
        switch (item.Type)
        {
            case ItemType.FRESH_APPLE:
                item.ChangeItem(ItemType.SPARKLING_APPLE);
                break;
            case ItemType.RAW_RUBY:
                item.ChangeItem(ItemType.SPARKLING_RUBY);
                break;
            case ItemType.SALT:
                item.ChangeItem(ItemType.SALTWATER);
                break;
            default:
                Debug.Log("Cannot Clean Item");
                return false;
        }

        return true;
    }

    private bool Boil(Item item)
    {
        switch (item.Type)
        {
            case ItemType.SALTWATER:
                item.ChangeItem(ItemType.PURE_WATER);

                SpawnAtItemPos(ItemType.SALT);
                break;
            case ItemType.PURE_WATER:
                item.ChangeItem(ItemType.BUCKET);
                break;
            default:
                Debug.Log("Cannot Boil Item");
                return false;
        }

        return true;
    }

    private bool Arcane(Item item)
    {
        switch (item.Type)
        {
            case ItemType.DISORDERED_SPIRIT:
                item.ChangeItem(ItemType.MINOR_SPIRIT);
                break;
            // don't keep this one
            // we'll probably get saltwater from the ocean?
            case ItemType.BUCKET:
                item.ChangeItem(ItemType.SALTWATER);
                break;
            default:
                Debug.Log("Cannot Do Magic With Item");
                return false;
        }

        return true;
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

using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]

public class Item : MonoBehaviour
{
    [Header("Item info")]
    [Tooltip("Enum to determine what kind of item this is.\nDo not set directly, call ChangeItem")]
    [SerializeField]
    private ItemType type;
    public bool isHeld;
    public int price;

    [Header("Unity Set Up")]
    public ItemSprites itemSprites;
    public PlayerStats stats;
    public Prices prices;
    [Header("Debugging")]
    public bool debug;

    private SpriteRenderer spriteRenderer;
    private int typeCode;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private ResourceNode maker;
    private bool isNoded;

    public bool IsNoded{
        get { return isNoded; }
        set { isNoded = value; }
    }

    public ItemType Type{
        get{ return type; }
    }

    public bool CanSell{
        get{
            return type == ItemType.POTION_CALM ||
                    type == ItemType.POTION_HEAL ||
                    type == ItemType.POTION_MANA;
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        ChangeItem(type);
    }

    public void SetMaker(ResourceNode m){
        maker = m;
        IsNoded = true;
    }

    /*
    private void Update()
    {
        // debugging all the sprites
        if (debug)
        {
            ChangeItem(type);
        }
    }
    */

    private int GetPrice(){
        switch (type)
        {
            case ItemType.POTION_CALM:
                return prices.calmPrice;
            case ItemType.POTION_HEAL:
                return prices.healPrice;
            case ItemType.POTION_MANA:
                return prices.manaPrice;
            default:
                return 0;
        }
    }

    public bool Sell(){
        if(!CanSell){
            Debug.Log("Cannot sell");
            return false;
        }

        if (debug)
        {
            Debug.Log("Selling " + name + " for " + price);
        }

        stats.money += price;
        Destroy(gameObject);
        return true;
    }

    // For Buying and selling.. 
    public bool Sell(int newPrice){
        if(!CanSell){
            Debug.Log("Cannot sell");
            return false;
        }

        if (debug)
        {
            Debug.Log("Selling " + name + " for " + newPrice);
        }

        stats.money += newPrice;
        Destroy(gameObject);
        return true;
    }

    // not intended to be used as a pickup script.
    // does things to the item when picked up
    public void PickUp()
    {
        isHeld = true;
        col.enabled = false;
        rb.isKinematic = true;

        if(IsNoded){
            maker.LoseOne();
            IsNoded = false;
            maker = null;
        }

        if (debug)
        {
            Debug.Log("Picking up" + name);
        }
    }

    // not intended to be used as a pickup script.
    // does things to the item when dropped
    public void Drop()
    {
        isHeld = false;
        col.enabled = true;
        rb.isKinematic = false;

        if (debug)
        {
            Debug.Log(transform.name + " has been dropped");
        }
    }

    public void SetSprite(Sprite set)
    {
        spriteRenderer.sprite = set;

        if (debug)
        {
            Debug.Log(transform.name + " has new sprite: " + set.ToString());
        }
    }

    // handles all variables when changing item to a new type
    // use when spawning a new item and set it to the appropriate type
    public void ChangeItem(ItemType newType)
    {
        type = newType;
        typeCode = GetItemTypeCode(type);
        SetSprite(itemSprites.sprites[typeCode]);
        price = GetPrice();

        if (debug)
        {
            Debug.Log(transform.name + " has been changed to a " + newType);
        }
    }

    // gets the numerical value of the item's type
    // an enum doesnt actually equal a number, the value is used for ordering in Unity editor
    public static int GetItemTypeCode(ItemType t)
    {
        switch (t)
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

    public void ToggleDebug()
    {
        debug = !debug;
    }
}

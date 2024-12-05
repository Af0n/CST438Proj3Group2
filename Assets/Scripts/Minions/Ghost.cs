using NUnit.Framework;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float speed;
    public float radius;

    public Transform player;
    public Transform node;
    public LayerMask pickupMask;
    public LayerMask nodeMask;

    private Transform target;
    private Rigidbody2D rb;
    private bool pickedUp;

    public bool HasItem{
        get { return transform.childCount > 0; }
    }

    private void OnEnable() {
        TickSystem.OnTick += Tick;
    }

    private void OnDisable() {
        TickSystem.OnTick -= Tick;
    }

    private void Awake() {
        target = transform;
    }

    public void SetTargets(Transform p, Transform n){
        player = p;
        node = n;
    }

    private void Update() {
        // dont try and move while picked up
        if(pickedUp){
            return;
        }

        target = DetermineTarget();

        NavigateTowardTarget();
    }

    private void Tick(){
        if(pickedUp){
            return;
        }

        // don't move if no actiual target
        if(target == transform){
            return;
        }

        TrySwitch();
    }

    public void PickUp(){
        rb.isKinematic = true;
        pickedUp = true;
    }

    public void Drop(Transform p){
        rb.isKinematic = false;
        pickedUp = false;
        player = p;
        TryFindNode();
    }

    private bool TryFindNode(){
        Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, radius * 3, nodeMask);

        // if there are no nearby nodes, quit
        if(objs.Length == 0){
            Debug.Log("Didnt find anything");
            return false;
        }

        // default value that will guarantee at least one selected station
        float dist = 100;
        Collider2D closestItem = null;

        // finding closest station to PLAYER
        foreach (Collider2D obj in objs)
        {
            if(Vector2.Distance(transform.position, obj.transform.position) < dist){
                closestItem = obj;
            }
        }

        node = closestItem.transform;

        return true;
    }

    private Transform DetermineTarget(){
        // sanity check for if something happens to the player or node
        if(player == null || target == null){
            return transform;
        }

        if(HasItem){
            return player;
        }

        return node;
    }

    // TODO: UTILIZE PATHFINDING SCRIPT
    private void NavigateTowardTarget(){
        // getting normalized direction to target
        Vector2 direction = target.position - transform.position;
        direction.Normalize();

        transform.Translate(speed * Time.deltaTime * direction);
    }

    private void TrySwitch(){
        // if not close enough, don't do anything
        if(radius < Vector2.Distance(transform.position, target.position)){
            return;
        }

        // here, we will be close enough to the player to drop the item
        if(HasItem){
            ItemDrop();
            return;
        }

        // here, we'll be close enough to the node to try and pick up an item
        TryItemPickup();
    }

    private bool TryItemPickup(){
        Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, radius * 3 , pickupMask);

        // if there are no nearby items to pickup, quit
        if(objs.Length == 0){
            return false;
        }

        // default value that will guarantee at least one selected item
        float dist = 100;
        Collider2D closestItem = null;

        // finding closest item to PLAYER
        foreach (Collider2D obj in objs)
        {
            if(Vector2.Distance(transform.position, obj.transform.position) < dist){
                closestItem = obj;
            }
        }

        // we have found the closest item
        // call pickup on that item
        closestItem.GetComponent<PickUp>().Pick(transform);

        return true;
    }

    private void ItemDrop(){
        transform.GetChild(0).GetComponent<PickUp>().Drop(player);
    }
}

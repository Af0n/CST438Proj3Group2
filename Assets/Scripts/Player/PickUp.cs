using UnityEngine;

public class PickUp : MonoBehaviour
{
    public void Pick(Transform p){
        transform.parent = p;
        transform.localPosition = Vector3.zero;

        if(transform.CompareTag("Item"))
        {
            GetComponent<Item>().PickUp();
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }else if(transform.CompareTag("Ghost")){
            GetComponent<Ghost>().PickUp();
        }

        Debug.Log(transform.name + " has been picked up by " + p.name);
    }

    public void Drop(Transform player){
        transform.parent = null;

        if(transform.CompareTag("Item"))
        {
            GetComponent<Item>().Drop();
        }else if(transform.CompareTag("Ghost")){
            GetComponent<Ghost>().Drop(player);
        }

        Debug.Log(transform.name + " has been dropped");
    }

    public void Drop(){
        transform.parent = null;

        if(transform.CompareTag("Item"))
        {
            GetComponent<Item>().Drop();
        }else if(transform.CompareTag("Ghost")){
            GetComponent<Ghost>().Drop();
        }

        Debug.Log(transform.name + " has been dropped");
    }
}

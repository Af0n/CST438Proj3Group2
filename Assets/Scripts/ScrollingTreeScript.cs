using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTreeScript : MonoBehaviour
{
    public float moveSpeed = 2f;
    public GameObject objectToSpawn;
    public Transform spawner;
    void Update()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ScrollingMenu"))
        {
            if (objectToSpawn != null && spawner != null)
            {
                Instantiate(objectToSpawn, spawner.position, spawner.rotation);
            }
            Destroy(gameObject);
        }
    }
}

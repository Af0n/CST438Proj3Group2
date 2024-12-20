using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public ItemType type;
    [Tooltip("number of ticks between spawn waves\nset to 0 to spawn every tick")]
    public int spawnDelay;
    [Tooltip("Minimum range to spawn items within")]
    public float spawnRangeMin;
    [Tooltip("Maximum range to spawn items within")]
    public float spawnRangeMax;
    [Tooltip("Number of items this node can spawn per wave.")]
    public int numSpawns;
    [Tooltip("Max number of items this node can spawn.")]
    public int maxSpawns;

    [Header("Unity Setup")]
    [Tooltip("Item prefab to instantiate\nDO NOT CHANGE")]
    public GameObject prefab;

    private int numItems;
    private int tickTimer;

    private void Awake()
    {
        numItems = 0;
        tickTimer = spawnDelay;
    }

    private void OnEnable() {
        TickSystem.OnTick += Tick;
    }

    private void OnDisable() {
        TickSystem.OnTick -= Tick;
    }

    public void Tick()
    {
        // don't spawn or count if too many items
        if (numItems >= maxSpawns)
        {
            return;
        }

        tickTimer--;
        tickTimer = Mathf.Max(0, tickTimer);

        // don't spawn if still cooling down
        if (tickTimer > 0)
        {
            return;
        }

        SpawnWave();

        tickTimer = spawnDelay;
    }

    private void SpawnWave()
    {
        for (int i = 0; i < numSpawns; i++)
        {
            SpawnItem();
        }
    }

    public void SpawnItem()
    {
        // random position around node
        Vector2 randPos = RandomPointInRadii(spawnRangeMin, spawnRangeMax);
        // instantiation
        GameObject obj = Instantiate(prefab, randPos, Quaternion.identity);
        // changing object to the proper type
        obj.GetComponent<Item>().ChangeItem(type);
        obj.GetComponent<Item>().SetMaker(this);
        obj.GetComponent<Rigidbody2D>().isKinematic = true;

        numItems++;
    }

    private Vector2 RandomPointInRadii(float minRange, float maxRange)
    {
        // random direction vector
        Vector2 randDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        // normalize
        randDir.Normalize();
        // give it a random length between ranges
        randDir *= Random.Range(minRange, maxRange);
        return transform.position + (Vector3)randDir;
    }

    // called whenever a resource is picked up from a node
    public void LoseOne()
    {
        numItems--;
    }
}

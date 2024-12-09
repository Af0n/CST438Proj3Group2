using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResizeCollider : MonoBehaviour
{
    public PolygonCollider2D polygon;
    public Tilemap tilemap;
    private CinemachineConfiner2D confiner2D;
    private void Start() {
        confiner2D = GameObject.FindGameObjectWithTag("Confiner").GetComponent<CinemachineConfiner2D>();
    }
    public void resizeCollider()
    {
        // Set the polygon to the bounds
        Bounds tilemapBounds = tilemap.localBounds;
        Vector2[] boxColliderPoints = new Vector2[4];
        boxColliderPoints[0] = new Vector2(tilemapBounds.min.x, tilemapBounds.min.y);
        boxColliderPoints[1] = new Vector2(tilemapBounds.max.x, tilemapBounds.min.y);
        boxColliderPoints[2] = new Vector2(tilemapBounds.max.x, tilemapBounds.max.y);
        boxColliderPoints[3] = new Vector2(tilemapBounds.min.x, tilemapBounds.max.y);
        polygon.SetPath(0, boxColliderPoints);
        // Invalidate the cache.. to reset the polygon
        confiner2D.InvalidateCache();
    }
}

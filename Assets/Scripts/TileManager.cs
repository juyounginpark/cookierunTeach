using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TileManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject groundPrefab;

    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 5f;
    [SerializeField] private int tileCount = 12;
    [SerializeField] private float startX = -8f;
    [SerializeField] private float recycleX = -12f;

    private readonly List<Transform> tiles = new List<Transform>();
    private float tileWidth;
    private float groundY;

    private void OnEnable()
    {
        BuildTiles();
    }

    private void OnDisable()
    {
        ClearTiles();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!isActiveAndEnabled) return;
        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (this == null) return;
            BuildTiles();
        };
    }
#endif

    private void Update()
    {
        if (!Application.isPlaying) return;

        float delta = scrollSpeed * Time.deltaTime;

        for (int i = 0; i < tiles.Count; i++)
        {
            Transform t = tiles[i];
            if (t == null) continue;
            t.position += Vector3.left * delta;

            if (t.position.x <= recycleX)
            {
                float rightmostX = GetRightmostX();
                t.position = new Vector3(rightmostX + tileWidth, groundY, t.position.z);
            }
        }
    }

    private void BuildTiles()
    {
        ClearTiles();

        if (tilePrefab == null || groundPrefab == null) return;

        groundY = groundPrefab.transform.position.y;
        tileWidth = GetTileWidth(tilePrefab);
        if (tileWidth <= 0f) return;

        float x = startX;
        for (int i = 0; i < tileCount; i++)
        {
            GameObject tile = Instantiate(tilePrefab, new Vector3(x, groundY, 0f), Quaternion.identity, transform);
            tile.name = $"{tilePrefab.name}_{i}";
            tile.hideFlags = Application.isPlaying ? HideFlags.None : HideFlags.DontSave;
            tiles.Add(tile.transform);
            x += tileWidth;
        }
    }

    private void ClearTiles()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (Application.isPlaying) Destroy(child);
            else DestroyImmediate(child);
        }
        tiles.Clear();
    }

    private float GetRightmostX()
    {
        float max = float.NegativeInfinity;
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i] != null && tiles[i].position.x > max) max = tiles[i].position.x;
        }
        return max;
    }

    private static float GetTileWidth(GameObject prefab)
    {
        SpriteRenderer sr = prefab.GetComponentInChildren<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            return sr.sprite.bounds.size.x * prefab.transform.lossyScale.x;
        }

        Renderer r = prefab.GetComponentInChildren<Renderer>();
        if (r != null)
        {
            return r.bounds.size.x;
        }

        return 0f;
    }
}

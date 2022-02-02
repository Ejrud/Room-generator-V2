using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildRoom : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] private Tile[] _tiles; // 0-topLeft, 1-top, 2-topRight, 3-left, 4-right, 5-bottomLeft, 6-bottom, 7-bottomRight, 8-cornerTopLeft,
                                            // 9-cornerTopRight, 10-cornerBottomLeft, 11-cornerBottomRight, 12-floor, 13 - top/bottom door, 14 - right/left door

    private Tilemap _groundTilemap;
    private Tilemap _wallTilemap;

    private int[] _roomRadius;

    public int[] GetRoomRadius()
    { 
        return _roomRadius;
    }

    public Tile[] GetTiles()
    {
        return _tiles;
    }

    public void SetParameters(Tilemap groundTilemap, Tilemap wallTilemap)
    { 
        _groundTilemap = groundTilemap;
        _wallTilemap = wallTilemap;
    }

    public void Build(Vector3Int[] roomPositions, int minRoomRadius, int maxRoomRadius) // main
    {
        int count = roomPositions.Length;
        _roomRadius = new int[count];

        for (int i = 0; i < count; i++)
        { 
            _roomRadius[i] = Random.Range(minRoomRadius, maxRoomRadius);
            int currerntRadius = _roomRadius[i];

            BuildGround(roomPositions[i], currerntRadius);
        }
    }

    private void BuildGround(Vector3Int roomPosition, int currerntRadius)
    {
        for (int y = -currerntRadius; y <= currerntRadius; y++)
        {
            for (int x = -currerntRadius; x <= currerntRadius; x++)
            {
                Vector3Int offset = new Vector3Int(x, y, 0);
                Vector3Int tilePosition = offset + roomPosition;
                _groundTilemap.SetTile(tilePosition, _tiles[12]);

                BuildWall(tilePosition, offset, currerntRadius);
            }
        }
    }

    private void BuildWall(Vector3Int tilePosition, Vector3Int offset, int currentRadius)
    {
        int tileIndex;

        if (offset.y == -currentRadius && offset.x == -currentRadius)
        {
            tileIndex = 5;
        }
        else if (offset.y == -currentRadius && offset.x == currentRadius)
        {
            tileIndex = 7;
        }
        else if (offset.y == currentRadius && offset.x == -currentRadius)
        {
            tileIndex = 0;
        }
        else if (offset.y == currentRadius && offset.x == currentRadius)
        {
            tileIndex = 2;
        }
        else if (offset.y == -currentRadius)
        {
            tileIndex = 6;
        }
        else if (offset.y == currentRadius)
        {
            tileIndex = 1;
        }
        else if (offset.x == -currentRadius)
        {
            tileIndex = 3;
        }
        else if (offset.x == currentRadius)
        {
            tileIndex = 4;
        }
        else
        {
            tileIndex = -1;
        }

        if (tileIndex != -1)
        {
            _wallTilemap.SetTile(tilePosition, _tiles[tileIndex]);
        }
    }

}

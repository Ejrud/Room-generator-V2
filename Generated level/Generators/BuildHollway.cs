using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildHollway : MonoBehaviour
{
    private Tile[] _tiles; // 0-topLeft, 1-top, 2-topRight, 3-left, 4-right, 5-bottomLeft, 6-bottom, 7-bottomRight, 8-cornerTopLeft,
                           // 9-cornerTopRight, 10-cornerBottomLeft, 11-cornerBottomRight, 12-floor, 13 - top/bottom door, 14 - right/left door

    private Tilemap _groundTilemap;
    private Tilemap _wallTilemap;
    private Tilemap _doorTilemap;

    private Vector3Int[] _roomPositions;
    private int[] _roomRadius;

    private int _roomDistance;

    private int _topLeft, _topRight, _bottomLeft, _bottomRight;
    private int _left, _right;
    private int _doorSide;

    public void SetParameters(Vector3Int[] roomPositions, int[] roomRadius, Tilemap groundTilemap, Tilemap wallTilemap, Tilemap doorIilemap, Tile[] tiles)
    {
        _roomPositions = roomPositions;
        _roomRadius = roomRadius;
        _groundTilemap = groundTilemap;
        _wallTilemap = wallTilemap;
        _doorTilemap = doorIilemap;
        _tiles = tiles;
    }

    public void Build(int maxRoomRadius)
    {
        _roomDistance = maxRoomRadius * 2;

        for (int i = 0; i < _roomPositions.Length; i++)
        {
            for (int j = 1; j <= 4; j++) // 1 - top, 2 - right, 3 - bottom, 4 - left
            {
                switch (j)
                {
                    case 1:
                        _topLeft = 10; _topRight = 11;
                        _bottomLeft = 8; _bottomRight = 9;
                        _left = 3; _right = 4;
                        _doorSide = 13;
                        BuildGround(new Vector3Int(0, 1, 0), i);
                        break;
                    case 2:
                        _topLeft = 10; _topRight = 8;
                        _bottomLeft = 11; _bottomRight = 9;
                        _left = 6; _right = 1;
                        _doorSide = 14;
                        BuildGround(new Vector3Int(1, 0, 0), i); // right
                        break;
                    case 3:
                        _topLeft = 9; _topRight = 8;
                        _bottomLeft = 11; _bottomRight = 10;
                        _left = 4; _right = 3;
                        _doorSide = 13;
                        BuildGround(new Vector3Int(0, -1, 0), i);
                        break;
                    case 4:
                        _topLeft = 9; _topRight = 11;
                        _bottomLeft = 8; _bottomRight = 10;
                        _left = 1; _right = 6;
                        _doorSide = 14;
                        BuildGround(new Vector3Int(-1, 0, 0), i);
                        break;
                }
            }
        }
    }

    private void BuildGround(Vector3Int direction, int roomIndex)
    {
        Vector3Int suposedRoomPosition = _roomPositions[roomIndex] + direction * _roomDistance;
        Vector3Int currentPosition;
        Vector3Int endPosition;

        int closeRoomIndex = -1;

        for (int i = 0; i < _roomPositions.Length; i++)
        {
            if (_roomPositions[i] == suposedRoomPosition)
            {
                closeRoomIndex = i;
            }
        }

        if (closeRoomIndex != -1)
        {
            currentPosition = _roomPositions[roomIndex] + direction * _roomRadius[roomIndex];
            endPosition = _roomPositions[closeRoomIndex] + (-direction * _roomRadius[closeRoomIndex]);

            BuildDoors(currentPosition, endPosition);

            int loopCount = _roomDistance - (_roomRadius[roomIndex] + _roomRadius[closeRoomIndex]) + 1;

            for (int i = 0; i < loopCount; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Vector3Int tileCursor = currentPosition + new Vector3Int(j * direction.y, j * direction.x, 0);

                    _groundTilemap.SetTile(tileCursor, _tiles[12]);

                    if (i == 0 && j == -1)
                    {
                        BuildWall(tileCursor, _tiles[_bottomLeft]);
                    }
                    else if (i == 0 && j == 1)
                    {
                        BuildWall(tileCursor, _tiles[_bottomRight]);
                    }
                    else if (i == loopCount - 1 && j == -1)
                    {
                        BuildWall(tileCursor, _tiles[_topLeft]);
                    }
                    else if (i == loopCount - 1 && j == 1)
                    {
                        BuildWall(tileCursor, _tiles[_topRight]);
                    }
                    else if (j == -1)
                    {
                        BuildWall(tileCursor, _tiles[_left]);
                    }
                    else if (j == 1)
                    {
                        BuildWall(tileCursor, _tiles[_right]);
                    }
                    else
                    {
                        BuildWall(tileCursor, null);
                    }
                }

                currentPosition += direction;
            }
        }
    }

    private void BuildWall(Vector3Int position, Tile tile)
    {
        _wallTilemap.SetTile(position, null);
        _wallTilemap.SetTile(position, tile);
    }

    private void BuildDoors(Vector3Int startPos, Vector3Int endPos)
    {
        _doorTilemap.SetTile(startPos, _tiles[_doorSide]);
        _doorTilemap.SetTile(endPos, _tiles[_doorSide]);
    }
}

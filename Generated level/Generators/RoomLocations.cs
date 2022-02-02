using System.Collections.Generic;
using UnityEngine;

public class RoomLocations : MonoBehaviour
{
    private Vector3Int[] _positions;
    private Vector3Int _previousPosition;

    private int _maxRoomRadius = 0;

    public void CreatePositions(int roomCount, int maxRoomRadius)
    {
        _positions = new Vector3Int[roomCount];

        _maxRoomRadius = maxRoomRadius;

        for (int i = 0; i < roomCount; i++)
        {
            Vector3Int position = (i == 0) ? new Vector3Int(0, 0, 0) : ProducePosition();
            _previousPosition = position;
            _positions[i] = position;
        }
    }

    public Vector3Int[] GetPositions()
    { 
        return _positions;
    }

    private Vector3Int ProducePosition()
    { 
        Vector3Int direction;
        Vector3Int nextPosition;

        int indexDirection = Random.Range(0, 5); // 1 - left, 2 - top, 3 - right, 4 - bottom

        switch (indexDirection)
        { 
            case 0:
                direction = new Vector3Int(-1, 0, 0);
                break;
            case 1:
                direction = new Vector3Int(0, 1, 0);
                break;
            case 2:
                direction = new Vector3Int(1, 0, 0);
                break;
            case 3:
                direction = new Vector3Int(0, -1, 0);
                break;
            default:
                direction = new Vector3Int(1, 0, 0);
                break;
        }

        nextPosition = _previousPosition + ((direction * _maxRoomRadius) * 2);


        for (int i = 0; i < _positions.Length; i++)
        {
            if (nextPosition == _positions[i])
            {
                nextPosition += ((new Vector3Int(direction.y, direction.x, 0) * _maxRoomRadius) * 2);
                i = 0;
            }
        }
            
        return nextPosition;
    }
}

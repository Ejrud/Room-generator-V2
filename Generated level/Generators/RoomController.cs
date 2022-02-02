using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class RoomController : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private AiManager _aiManager;

    [Header("Links")]
    [SerializeField] private Grid _grid;
    [SerializeField] private GameObject _groundTilemapObj;
    [SerializeField] private GameObject _wallTilemapObj;
    [SerializeField] private GameObject _doorTilemapObj;
    private Tilemap _groundTilemap;
    private Tilemap _wallTilemap;
    private Tilemap _doorTilemap;

    [Header("Settings")]
    [SerializeField] private int _roomCount;
    [SerializeField] private int _maxRoomRadius;
    [SerializeField] private int _minRoomRadius;

    private RoomLocations _roomLocations;
    private BuildHollway _buildHollway;
    private BuildRoom _buildRoom;

    private void Start()
    {
        _roomLocations = GetComponent<RoomLocations>();
        _buildHollway = GetComponent<BuildHollway>();
        _buildRoom = GetComponent<BuildRoom>();

        _groundTilemap = _groundTilemapObj.GetComponent<Tilemap>();
        _wallTilemap = _wallTilemapObj.GetComponent<Tilemap>();
        _doorTilemap = _doorTilemapObj.GetComponent<Tilemap>();

        GenerateDungeon();

        _wallTilemapObj.SetActive(false);
        _wallTilemapObj.SetActive(true);

        _doorTilemapObj.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void GenerateDungeon()
    {
        _roomLocations.CreatePositions(_roomCount, _maxRoomRadius);

        _buildRoom.SetParameters(_groundTilemap, _wallTilemap);
        _buildRoom.Build(_roomLocations.GetPositions(), _minRoomRadius, _maxRoomRadius);

        _buildHollway.SetParameters(_roomLocations.GetPositions(), _buildRoom.GetRoomRadius(), _groundTilemap, _wallTilemap, _doorTilemap, _buildRoom.GetTiles());
        _buildHollway.Build(_maxRoomRadius);

        _aiManager.SpawnCreaturesArea(_roomLocations.GetPositions(), _buildRoom.GetRoomRadius());
    }
}

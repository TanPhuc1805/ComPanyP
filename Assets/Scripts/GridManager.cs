using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab; // Tile mặc định

    [Header("Wall Prefabs")]
    [SerializeField] private GameObject _wallTop;  // Tường trên cùng
    [SerializeField] private GameObject _cornerTopLeft, _cornerTopRight; // Góc trên
    [SerializeField] private GameObject _cornerBottomLeft, _cornerBottomRight; // Góc dưới
    [SerializeField] private GameObject _horizontalEdge, _verticalEdge; // Cạnh ngang & dọc

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles; // Lưu danh sách tile
    private Transform _tileParent, _decoParent; // Parent objects để gộp tile & deco

    void Start()
    {
        _tileParent = new GameObject("Tiles").transform;  // Parent cho Tiles
        _decoParent = new GameObject("Deco").transform;   // Parent cho Deco

        GenerateGrid(); // Spawn map trước
        GenerateDeco(); // Thêm deco sau
    }

    // 🔹 Spawn map trước
    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity, _tileParent);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        // Căn giữa camera
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    // 🔹 Thêm tường & deco sau khi map đã được tạo
    void GenerateDeco()
    {
        // 🟢 Spawn WallTop trên hàng trên cùng
        for (int x = 0; x < _width; x++)
        {
            Vector3 position = new Vector3(x, _height, 0);
            Instantiate(_wallTop, position, Quaternion.identity, _decoParent).name = $"WallTop {x} {_height}";
        }

        // 🟢 Bao quanh WallTop + Grid bằng Edge & Corner
        for (int x = -1; x <= _width; x++)
        {
            for (int y = -1; y <= _height + 1; y++)
            {
                Vector3 position = new Vector3(x, y, 0);

                // Nếu vị trí nằm ngoài khu vực map & WallTop
                if (y == _height + 1) // Hàng trên cùng (ngoài WallTop)
                {
                    if (x == -1)
                        Instantiate(_cornerTopLeft, position, Quaternion.identity, _decoParent).name = "Corner Top Left";
                    else if (x == _width)
                        Instantiate(_cornerTopRight, position, Quaternion.identity, _decoParent).name = "Corner Top Right";
                    else
                        Instantiate(_horizontalEdge, position, Quaternion.identity, _decoParent).name = $"Horizontal Edge {x} {y}";
                }
                else if (y == -1) // Hàng dưới cùng (ngoài Grid)
                {
                    if (x == -1)
                        Instantiate(_cornerBottomLeft, position, Quaternion.identity, _decoParent).name = "Corner Bottom Left";
                    else if (x == _width)
                        Instantiate(_cornerBottomRight, position, Quaternion.identity, _decoParent).name = "Corner Bottom Right";
                    else
                        Instantiate(_horizontalEdge, position, Quaternion.identity, _decoParent).name = $"Horizontal Edge {x} {y}";
                }
                else if (x == -1) // Cạnh trái
                {
                    Instantiate(_verticalEdge, position, Quaternion.identity, _decoParent).name = $"Vertical Edge {x} {y}";
                }
                else if (x == _width) // Cạnh phải
                {
                    Instantiate(_verticalEdge, position, Quaternion.identity, _decoParent).name = $"Vertical Edge {x} {y}";
                }
            }
        }

        Debug.Log("WallTop, Corner & Edge đã được spawn xong!");
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}

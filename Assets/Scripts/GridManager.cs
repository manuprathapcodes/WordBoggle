using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int width = 4, height = 4;
    public Tile[,] grid;

    public void GenerateGrid(bool endlessMode, LevelData level = null)
    {
        if (level != null)
        {
            width = level.gridSize.x;
            height = level.gridSize.y;
        }

        grid = new Tile[width, height];

        // Configure layout
        var layout = GetComponent<GridLayoutGroup>();
        layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = width;

        // Clean up existing tiles
        foreach (Transform child in transform) Destroy(child.gameObject);

        // Spawn grid
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var go = Instantiate(tilePrefab, transform);
                var tile = go.GetComponent<Tile>();
                char letter;
                TileType type = TileType.Normal;

                if (level != null)
                {
                    int idx = x + y * width;
                    letter = level.gridData[idx].letter[0];
                    type = (TileType)level.gridData[idx].tileType;
                }
                else
                {
                    letter = RandomLetter();
                }

                tile.Init(letter, type, new Vector2Int(x, y));
                grid[x, y] = tile;
                go.name = $"Tile_{x}_{y}";
            }
        }
    }

    char RandomLetter()
    {
        string letters = "ETAOINSHRDLUCMWGFYPBVKJXQZ";
        return letters[Random.Range(0, letters.Length)];
    }

    public void RemoveTiles(List<Tile> path)
    {
        foreach (var t in path)
        {
            int x = t.coords.x, y = t.coords.y;
            Destroy(grid[x, y].gameObject);
            for (int yy = y; yy < height - 1; yy++)
                grid[x, yy] = grid[x, yy + 1];
            var go = Instantiate(tilePrefab, transform);
            var nt = go.GetComponent<Tile>();
            nt.Init(RandomLetter(), TileType.Normal, new Vector2Int(x, height - 1));
            grid[x, height - 1] = nt;
        }
    }

    public bool IsAdjacent(Tile a, Tile b) =>
        Mathf.Abs(a.coords.x - b.coords.x) <= 1 && Mathf.Abs(a.coords.y - b.coords.y) <= 1;
    }
using System.Collections.Generic;
using System.Text;

/// <summary>
/// This script contains all the data that is needed for serializing the level data. 
/// </summary>

[System.Serializable]
public class TileInfo
{
    public int tileType;
    public string letter;

    public override string ToString()
    {
        return $"[{letter}:{tileType}]";
    }
}

[System.Serializable]
public class LevelDataList
{
    public List<LevelData> data;
}

[System.Serializable]
public class LevelData
{
    public int bugCount, wordCount, timeSec, totalScore;
    public SerializableVector2 gridSize;
    public List<TileInfo> gridData;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Grid Size: {gridSize.x} x {gridSize.y}");
        sb.AppendLine($"Bug Count: {bugCount}, Word Count: {wordCount}, Time: {timeSec}, Score: {totalScore}");
        sb.AppendLine("Grid Data:");

        int width = gridSize.x;
        for (int i = 0; i < gridData.Count; i++)
        {
            sb.Append(gridData[i].ToString() + " ");
            if ((i + 1) % width == 0)
                sb.AppendLine();
        }

        return sb.ToString();
    }
}

[System.Serializable]
public struct SerializableVector2
{
    public int x, y;

    public override string ToString()
    {
        return $"({x}, {y})";
    }
}

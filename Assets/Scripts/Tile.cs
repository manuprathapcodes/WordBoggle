using UnityEngine;
using TMPro;

public enum TileType { Normal, Bug, Rock }

public class Tile : MonoBehaviour
{
    public Vector2Int coords;
    public char letter;
    public TileType tileType;

    [Header("References")]
    public TextMeshProUGUI letterTMP;
    public GameObject bonusGO;  // Assign ScoreIndicators > Bonus
    public GameObject rockGO;   // Assign ScoreIndicators > Block

    public bool isSelected = false;

    public void Init(char c, TileType type, Vector2Int pos)
    {
        letter = c;
        tileType = type;
        coords = pos;

        letterTMP.text = c.ToString();

        // Show/hide bonus/rock based on tile type
        bonusGO.SetActive(tileType == TileType.Bug);
        rockGO.SetActive(tileType == TileType.Rock);
    }

    public void Highlight(bool on)
    {
        isSelected = on;
        GetComponent<UnityEngine.UI.Image>().color = on ? Color.yellow : Color.white;
    }
}

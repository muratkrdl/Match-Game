using UnityEngine;
using UnityEngine.EventSystems;

public class BGTile : MonoBehaviour
{
    private bool isInaGroup = false;
    private VisualTile currentVisualTile;
    private Vector2Int coordinate;

    private int sortingOrderValue;

    public bool IsInaGroup
    { 
        get => isInaGroup;
        set => isInaGroup = value;
    }
    public VisualTile CurrentVisualTile 
    { 
        get => currentVisualTile; 
        set
        {
            currentVisualTile = value;
            if(currentVisualTile != null)
                currentVisualTile.BGTile = this;
        }
    }

    public Vector2Int GetCoordinate { get => coordinate; }
    public int GetSortingOrderValue { get => sortingOrderValue; }

    public void Initialize(Vector2Int coordinate, int sort)
    {
        this.coordinate = coordinate;
        name = "(" + coordinate.x + "," + coordinate.y + ")";

        sortingOrderValue = sort * 10;
    }

}

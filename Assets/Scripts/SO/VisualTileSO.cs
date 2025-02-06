using UnityEngine;

[CreateAssetMenu(menuName = "TileSO", fileName = "NewTile")]
public class VisualTileSO : ScriptableObject
{
    [SerializeField] private Sprite[] sprites;
    public Sprite GetTileByGroupCount(int count)
    {
        return sprites[count];
    }
}

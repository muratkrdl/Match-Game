using UnityEngine;

public class VisualTileSOKeeper : MonoBehaviour
{
    public static VisualTileSOKeeper Instance;

    [SerializeField] private VisualTileSO[] tileSO;

    private void Awake() 
    {
        Instance = this;
    }

    public VisualTileSO GetRandomTileSO()
    {
        return tileSO[Random.Range(0, BoardInfo.Instance.GetColorAmount)];
    }

}

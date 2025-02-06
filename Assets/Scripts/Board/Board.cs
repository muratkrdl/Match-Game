using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance;

    [SerializeField] private float xDistance;
    [SerializeField] private float yDistance;

    [SerializeField] private BGTile bgTilePrefab;
    [SerializeField] private VisualTile visualTilePrefab;

    [SerializeField] private Transform bgTileParent;
    [SerializeField] private Transform visualTileParent;

    [SerializeField] private Transform bgBoard;
    [SerializeField] private Transform bgImage;

    private CancellationTokenSource cts = new();

    private BGTile[,] tiles;
    
    private int width;
    private int height;
    private float realHeight;

    public BGTile[,] GetTiles
    {
        get => tiles;
    }
    public float GetRealHeight
    {
        get => realHeight;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        width = BoardInfo.Instance.GetBoardSize.x;
        height = BoardInfo.Instance.GetBoardSize.y;
        tiles = new BGTile[width, height];

        // for virtual camera follow group
        bgBoard.localScale = new(width * xDistance, height * yDistance);
        bgBoard.position = new((float)(width-1)/2 * xDistance, (float)(height-1)/2 * yDistance);

        SetUpTile().Forget();
    }

    private async UniTaskVoid SetUpTile()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(.5f));
        transform.GetChild(2).gameObject.SetActive(true);

        //background image
        float useComplier = height;
        if(width > height)
            useComplier = width;
        bgImage.localScale = new(useComplier * bgImage.localScale.x, useComplier * bgImage.localScale.y);
        bgImage.position = bgBoard.transform.position;

        // set active board frame
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                BGTile newBGTile = Instantiate(bgTilePrefab, new(i * xDistance, j * yDistance), Quaternion.identity, bgTileParent);
                newBGTile.Initialize(new(i, j), j);
                tiles[i,j] = newBGTile;

                VisualTile visualTile = Instantiate(visualTilePrefab, new(newBGTile.transform.position.x, height*2), Quaternion.identity, visualTileParent);
                visualTile.Initialize(new(newBGTile.transform.position.x, height*2));

                newBGTile.CurrentVisualTile = visualTile;
            }
        }

        await UniTask.WaitUntil(AllTileFit);

        realHeight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height)).y;
        TileGroupInfoKeeper.Instance.SetGroupInfos();
    }

    public async UniTaskVoid BlastingTiles()
    {
        HashSet<int> blastedTilesColumnValues = new(); // keep blast column values
        List<VisualTile> blastedVisualTiles = new();

        foreach(BGTile item in TileGroupInfoKeeper.Instance.BlastedTiles)
        {
            blastedTilesColumnValues.Add(item.GetCoordinate.x);
            blastedVisualTiles.Add(item.CurrentVisualTile);
            item.CurrentVisualTile = null;
        }

        await UniTask.Delay(TimeSpan.FromSeconds(.6f), cancellationToken: cts.Token);

        GameStateManager.Instance.OnGameStateChange?.Invoke(new TileFallingState());

        foreach(BGTile item in tiles)
        {
            item.IsInaGroup = false;

            if(blastedTilesColumnValues.Contains(item.GetCoordinate.x) && item.GetCoordinate.y != 0 && item.CurrentVisualTile != null)
            {
                BGTile lowestTile = GetLowestColumn(item);
                if(lowestTile == item) continue;
                
                (lowestTile.CurrentVisualTile, item.CurrentVisualTile) = (item.CurrentVisualTile, lowestTile.CurrentVisualTile);
            }
        }

        await UniTask.WaitUntil(() => AllTileFit(), cancellationToken: cts.Token);

        foreach(VisualTile item in blastedVisualTiles)
        {
            BGTile lowestTile = GetLowestColumn(tiles[item.BGTile.GetCoordinate.x, height-1]);
            lowestTile.CurrentVisualTile = item;
        
            item.Initialize(new(lowestTile.transform.position.x, realHeight+2));
            item.GetAnimator.SetTrigger(ConstStrings.VISUAL_TILE_RESET);
        }

        await UniTask.WaitUntil(() => AllTileFit(), cancellationToken: cts.Token);

        TileGroupInfoKeeper.Instance.SetGroupInfos();
    }

    BGTile GetLowestColumn(BGTile bGTile)
    {
        BGTile lowestTile = GetTileByCoordinate(bGTile.GetCoordinate + Vector2Int.down);
        if(lowestTile != null && lowestTile.CurrentVisualTile == null)
        {
            return GetLowestColumn(lowestTile);
        }

        return bGTile;
    }

    public BGTile GetTileByCoordinate(Vector2Int lookCoordinate)
    {
        if(lookCoordinate.x <= width-1 && lookCoordinate.y <= height-1 && lookCoordinate.x != -1 && lookCoordinate.y != -1)
        {
            return tiles[lookCoordinate.x, lookCoordinate.y];
        }
        else
        {
            return null;
        }
    }

    public bool AllTileFit()
    {
        foreach(var item in tiles)
        {
            if(item.CurrentVisualTile == null) continue;
            if(item.CurrentVisualTile.transform.position != item.transform.position)
                return false;
        }

        return true;
    }

    private void OnDestroy() 
    {
        cts.Cancel();
    }

}

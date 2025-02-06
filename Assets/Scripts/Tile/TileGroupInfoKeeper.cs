using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileGroupInfoKeeper : MonoBehaviour
{
    public static TileGroupInfoKeeper Instance;

    private List<HashSet<BGTile>> groupInfos = new();

    private HashSet<BGTile> blastedTiles;
    private Sprite blastedTileSprite;

    private CancellationTokenSource cts = new();

    private readonly List<Vector2Int> lookVectors = new()
    {
        Vector2Int.right,
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left
    };

    public HashSet<BGTile> BlastedTiles
    {
        get => blastedTiles;
        set => blastedTiles = value;
    }
    public Sprite BlastedTileSprite
    {
        get => blastedTileSprite;
        set => blastedTileSprite = value;
    }

    private void Awake() 
    {
        Instance = this;
    }

    async UniTaskVoid SetTilesVisualByGroupCount()
    {
        foreach(HashSet<BGTile> item in groupInfos)
        {
            int index;
            
            if(item.Count <= BoardInfo.Instance.GetConditions[0])
                index = 0;
            else if(item.Count <= BoardInfo.Instance.GetConditions[1])
                index = 1;
            else if(item.Count <= BoardInfo.Instance.GetConditions[2])
                index = 2;
            else
                index = 3;

            foreach(BGTile bgTile in item)
            {
                bgTile.CurrentVisualTile.SetSprite(bgTile.CurrentVisualTile.GetVisualTileSO.GetTileByGroupCount(index)).Forget();
            }
        }

        foreach(BGTile bgTile in Board.Instance.GetTiles)
        {
            if(!bgTile.IsInaGroup)
            {
                bgTile.CurrentVisualTile.SetSprite(bgTile.CurrentVisualTile.GetVisualTileSO.GetTileByGroupCount(0)).Forget();
            }
        }

        await UniTask.Delay(TimeSpan.FromSeconds(.4f), cancellationToken: cts.Token);
        IPlayerState newState = GoalMovesBoard.Instance.IsGameOver() ? new GameOverState() : new PlayerReadyState();
        GameStateManager.Instance.OnGameStateChange?.Invoke(newState);
    }

    public void SetGroupInfos()
    {
        groupInfos.Clear();

        foreach(BGTile item in Board.Instance.GetTiles)
        {
            if(item.IsInaGroup) continue;

            HashSet<BGTile> groupTiles = CheckGroup(new(), new() { item } );
            if(groupTiles.Count > 1)
            {
                groupInfos.Add(groupTiles);
            }
        }

        if(CheckHaveAnyGroup() || GoalMovesBoard.Instance.GetCurrentMoves <= 0)
        {
            SetTilesVisualByGroupCount().Forget();
        }
        else // no Blastable group
        {
            ChangeOneTile().Forget();
        }
    }

    public HashSet<BGTile> CheckGroup(HashSet<BGTile> oldList, HashSet<BGTile> newCheck)
    {
        HashSet<BGTile> newCheckList = new();

        foreach(BGTile item in newCheck)
        {
            foreach(Vector2Int lookVector in lookVectors)
            {
                newCheckList = HasSameColorType(newCheckList, item, lookVector);
            }
        }

        if(newCheckList.Count == 0)
        {
            return oldList;
        }

        foreach(BGTile item in newCheck)
        {
            oldList.Add(item);
            item.IsInaGroup = true;
        }

        return CheckGroup(oldList, newCheckList);
    }

    async UniTaskVoid ChangeOneTile()
    {
        // No blastable Group

        await UniTask.WaitUntil(() => Board.Instance.AllTileFit());

        MessageUI.Instance.CallMessageOnUI(ConstStrings.MESSAGE_NO_BLASTABLE, 1f, .2f);

        await UniTask.Delay(TimeSpan.FromSeconds(1.2));

        BGTile firstTile = Board.Instance.GetTiles[Random.Range(0, BoardInfo.Instance.GetBoardSize.x), Random.Range(0, BoardInfo.Instance.GetBoardSize.y)];
        BGTile secondTile = null;

        while(secondTile == null)
        {
            Vector2Int newVector;
            if(Random.value <= .5f)
                newVector = Vector2Int.left;
            else
                newVector = Vector2Int.right;

            secondTile = Board.Instance.GetTileByCoordinate(firstTile.GetCoordinate + newVector);
        }

        secondTile.CurrentVisualTile.SetVisualTileSO(firstTile.CurrentVisualTile.GetVisualTileSO);

        SetGroupInfos();
    }

    HashSet<BGTile> HasSameColorType(HashSet<BGTile> newList, BGTile bGTile, Vector2Int direction)
    {
        if(bGTile.IsInaGroup) return newList;
        Vector2Int lookCoordinate = bGTile.GetCoordinate + direction;

        BGTile secondBGTile = Board.Instance.GetTileByCoordinate(lookCoordinate);
        if(secondBGTile == null || bGTile.CurrentVisualTile.GetVisualTileSO != secondBGTile.CurrentVisualTile.GetVisualTileSO)
            return newList;
        
        newList.Add(secondBGTile);
        return newList;
    }

    public HashSet<BGTile> IsInAnyGroup(BGTile bGTile)
    {
        foreach(HashSet<BGTile> item in groupInfos)
        {
            if(item.Contains(bGTile))
            {
                return item;
            }
        }

        return null;
    }

    bool CheckHaveAnyGroup()
    {
        return groupInfos.Count switch
        {
            0 => false,
            _ => true,
        };
    }

    private void OnDestroy() 
    {
        cts.Cancel();
    }

}

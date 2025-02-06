using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MenuBaseClass
{
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;

    CancellationTokenSource cts = new();

    protected override void OnOpenMyMenu()
    {
        SetGameOver();
    }

    private void SetGameOver()
    {
        bool value = GoalMovesBoard.Instance.CheckWin();
        string sfxName = value ? ConstStrings.SFX_WIN : ConstStrings.SFX_LOSE;
        
        winMenu.SetActive(value);
        loseMenu.SetActive(!value);

        SoundManager.Instance.PlaySound2D(sfxName);
    }

    public void OnClick_ReturnMainMenu()
    {
        FadeImage.Instance.SetFadeImage(ConstStrings.FADE_IMAGE_FADE);
        Invoke(nameof(InvokeReloadScene), MenuManager.Instance.GetFadeImageDelay);
    }

    private void InvokeReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClick_Restart(bool value)
    {
        if(!value && GameStateManager.Instance.IsGameOver) return;
        MenuManager.Instance.ChangeMenuWithDelay(MenuManager.Instance.GetMenuByName(ConstStrings.MENU_GOAL_MOVE)).Forget();
        InitializeAllTile().Forget();
    }

    async UniTaskVoid InitializeAllTile()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(MenuManager.Instance.GetFadeImageDelay), cancellationToken: cts.Token);

        //initialize
        foreach(BGTile item in Board.Instance.GetTiles)
        {
            item.IsInaGroup = false;
            item.CurrentVisualTile.Initialize(new(item.transform.position.x, Board.Instance.GetRealHeight*2));
            item.CurrentVisualTile.BGTile = item;
        }

        //set moves and goal
        GoalMovesBoard.Instance.OnClickedRestartGame();

        await UniTask.WaitUntil(() => Board.Instance.AllTileFit(), cancellationToken: cts.Token);

        TileGroupInfoKeeper.Instance.SetGroupInfos();

        await UniTask.Delay(TimeSpan.FromSeconds(.4f), cancellationToken: cts.Token);
        //set playerreadystate
        GameStateManager.Instance.OnGameStateChange?.Invoke(new PlayerReadyState());

        cts.Cancel();
        cts = new();
    }

    private void OnDestroy()
    {
        cts.Cancel();
    }

}

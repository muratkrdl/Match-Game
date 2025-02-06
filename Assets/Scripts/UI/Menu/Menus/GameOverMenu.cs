using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MenuBaseClass
{
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;

    [SerializeField] Image gameOverMenuGoalSprite;
    [SerializeField] TextMeshProUGUI gameOverMenuGoal;

    CancellationTokenSource cts = new();

    protected override void OnOpenMyMenu()
    {
        SetGameOver();
    }

    void SetGameLoseGoal(Sprite sprite, int goalAmount)
    {
        gameOverMenuGoalSprite.sprite = sprite;
        gameOverMenuGoal.text = goalAmount.ToString();
    }

    private void SetGameOver()
    {
        bool value = GoalMovesBoard.Instance.CheckWin();
        string sfxName = ConstStrings.SFX_WIN;
        
        if(!value)
        {
            sfxName = ConstStrings.SFX_LOSE;
            SetGameLoseGoal(GoalMovesBoard.Instance.GetGoalSprite, GoalMovesBoard.Instance.GetCurrentGoal);
        }
        
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

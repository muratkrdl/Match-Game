using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class GameOverState : IPlayerState
{
    public void EnterState()
    {
        GameStateManager.Instance.IsGameOver = true;
        MessageUI.Instance.CallMessageOnUI(ConstStrings.MESSAGE_GAME_OVER, 1f);

        DelayGameOverMenu().Forget();
    }

    public void ExitState()
    {
        GameStateManager.Instance.IsGameOver = false;
        cts.Cancel();
    }

    public void UpdateState()
    {

    }

    CancellationTokenSource cts = new();
    async UniTaskVoid DelayGameOverMenu()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cts.Token);
        if(!GameStateManager.Instance.IsGameOver) return;
        MenuManager.Instance.ChangeMenuWithDelay(MenuManager.Instance.GetMenuByName(ConstStrings.MENU_GAME_OVER)).Forget();
    }

    private void OnDestroy() 
    {
        cts.Cancel();
    }
    
}

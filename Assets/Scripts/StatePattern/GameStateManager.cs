using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public Action<IPlayerState> OnGameStateChange;

    private IPlayerState currentState;

    private bool canPlayerInteract = false;
    private bool isGameOver = false;

    public bool CanPlayerInteract
    {
        get => canPlayerInteract;
    }
    public bool IsGameOver
    {
        get => isGameOver;
        set => isGameOver = value;
    }

    private void Awake()
    {
        Instance = this;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
    }

    private void Start() 
    {
        OnGameStateChange += GameStateManager_OnGameStateChange;
        currentState = new OnStartState();
    }

    private void Update() 
    {
        currentState.UpdateState();
    }

    private void GameStateManager_OnGameStateChange(IPlayerState newState)
    {
        ChangeState(newState);
    }

    private void ChangeState(IPlayerState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public void SetCanPlayerInteract(bool value)
    {
        canPlayerInteract = value;
    }

    private void OnDestroy() 
    {
        OnGameStateChange -= GameStateManager_OnGameStateChange;
    }
}

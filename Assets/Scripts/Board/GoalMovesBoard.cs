using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalMovesBoard : MonoBehaviour
{
    public static GoalMovesBoard Instance;

    [SerializeField] private Image goalSprite;
    [SerializeField] private TextMeshProUGUI goalText;

    [SerializeField] private TextMeshProUGUI movesText;

    private int initialGoal;
    private int initialMoves;

    private int currentGoal;
    private int currentMoves;

    public int GetCurrentMoves
    {
        get => currentMoves;
    }

    private void Awake() 
    {
        Instance = this;
    }

    public void SetGoalMove(Sprite sprite, int goal, int moves)
    {
        goalSprite.sprite = sprite;

        initialGoal = goal;
        initialMoves = moves;

        currentGoal = initialGoal;
        currentMoves = initialMoves;

        goalText.text = currentGoal.ToString();
        movesText.text = currentMoves.ToString();
    }

    public void CheckBlastedTile()
    {
        currentMoves--;
        if(currentMoves <= 0) currentMoves = 0;
        movesText.text = currentMoves.ToString();
        if(TileGroupInfoKeeper.Instance.BlastedTileSprite == goalSprite.sprite)
        {
            currentGoal -= TileGroupInfoKeeper.Instance.BlastedTiles.Count;
            if(currentGoal <= 0) currentGoal = 0;
            goalText.text = currentGoal.ToString();
        }
    }

    public void OnClickedRestartGame()
    {
        currentGoal = initialGoal;
        currentMoves = initialMoves;

        goalText.text = currentGoal.ToString();
        movesText.text = currentMoves.ToString();
    }

    public bool IsGameOver()
    {
        return currentGoal <= 0 || currentMoves <= 0;
    }

    public bool CheckWin()
    {
        return currentGoal <= 0;
    }

}

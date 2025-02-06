using UnityEngine;

public class BoardInfo : MonoBehaviour
{
    public static BoardInfo Instance;

    private Vector2Int boardSize;
    private int colorAmount;
    private int[] conditions = new int[3];

    public Vector2Int GetBoardSize
    {
        get => boardSize;
    }
    public int GetColorAmount
    {
        get => colorAmount;
    }
    public int[] GetConditions
    {
        get => conditions;
    }

    private void Awake() 
    {
        Instance = this;
    }

    void Start() 
    {
        MenuManager.Instance.OnClickPlay += MenuManager_OnClickPlay;
    }

    private void MenuManager_OnClickPlay(Vector2Int boardSize, int ColorAmount, Sprite choosedSprite, int choosedScore, int choosedMove, int[] conditionsArray)
    {
        SetBoardValues(boardSize, ColorAmount, choosedSprite, choosedScore, choosedMove, conditionsArray);
    }

    void SetBoardValues(Vector2Int boardSize, int colorAmount, Sprite choosedSprite, int choosedScore, int choosedMove, int[] conditions)
    {
        this.boardSize = boardSize;
        this.colorAmount = colorAmount;
        this.conditions = conditions;

        Board.Instance.StartGame();
        GoalMovesBoard.Instance.SetGoalMove(choosedSprite, choosedScore, choosedMove);
    }

}

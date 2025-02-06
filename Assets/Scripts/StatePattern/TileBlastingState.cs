public class TileBlastingState : IPlayerState
{
    public void EnterState()
    {
        GoalMovesBoard.Instance.CheckBlastedTile();
        Board.Instance.BlastingTiles().Forget();
        SoundManager.Instance.PlaySound2D(ConstStrings.SFX_TILE_BLAST);
    }

    public void ExitState()
    {

    }

    public void UpdateState()
    {

    }
}

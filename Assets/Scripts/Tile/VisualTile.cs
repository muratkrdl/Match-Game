using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class VisualTile : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Animator visualTileAnimator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private ParticleSystem myParticleSystem;

    [SerializeField] private float moveSpeed;

    private VisualTileSO currentVisualTileSO;

    private CancellationTokenSource cts = new();

    private BGTile currentBGTile;

    public Animator GetAnimator
    {
        get => visualTileAnimator;
    }
    public VisualTileSO GetVisualTileSO
    {
        get => currentVisualTileSO;
    }
    public BGTile BGTile 
    { 
        get => currentBGTile;
        set
        {
            currentBGTile = value;
            cts.Cancel();
            cts = new();
            spriteRenderer.sortingOrder = currentBGTile.GetSortingOrderValue;
            MoveVisualTile().Forget();
        }
    }

    public void Initialize(Vector3 spawnPos)
    {
        transform.position = spawnPos;
        
        currentVisualTileSO = VisualTileSOKeeper.Instance.GetRandomTileSO();
        spriteRenderer.sprite = currentVisualTileSO.GetTileByGroupCount(0);
    }

    public void SetVisualTileSO(VisualTileSO so)
    {
        currentVisualTileSO = so;
        SetSprite(currentVisualTileSO.GetTileByGroupCount(0)).Forget();
    }

    public async UniTaskVoid SetSprite(Sprite sprite)
    {
        if(spriteRenderer.sprite == sprite) return;
        // particle
        myParticleSystem.Play();
        SoundManager.Instance.PlaySound2DVolume(ConstStrings.SFX_TILE_CHANGE_SPRITE, .1f);
        await UniTask.Delay(TimeSpan.FromSeconds(.4f), cancellationToken: cts.Token);
        spriteRenderer.sprite = sprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!GameStateManager.Instance.CanPlayerInteract) return;
        else if(!currentBGTile.IsInaGroup)
        {
            UnBlastAnim().Forget();
            return;
        }

        // find its group
        HashSet<BGTile> itsGroup = TileGroupInfoKeeper.Instance.IsInAnyGroup(BGTile);
        TileGroupInfoKeeper.Instance.BlastedTiles = itsGroup;
        TileGroupInfoKeeper.Instance.BlastedTileSprite = currentVisualTileSO.GetTileByGroupCount(0);

        // blast its group
        foreach(BGTile item in itsGroup)
        {
            item.CurrentVisualTile.GetAnimator.SetTrigger(ConstStrings.VISUAL_TILE_ANIM);
        }

        // change game state
        GameStateManager.Instance.OnGameStateChange?.Invoke(new TileBlastingState());
    }

    async UniTaskVoid MoveVisualTile()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(((float)currentBGTile.GetCoordinate.y) / 20));
        
        while(transform.position != currentBGTile.transform.position)
        {
            await UniTask.Yield(cancellationToken: cts.Token);
            Vector2 moveDir = (currentBGTile.transform.position - transform.position).normalized;
            Vector2 newPos = transform.position;
            newPos += moveSpeed * Time.deltaTime * moveDir;
            transform.position = newPos;

            if(Vector2.Distance(transform.position, currentBGTile.transform.position) <= .15f ||
            transform.position.y < currentBGTile.transform.position.y)
            {
                transform.position = currentBGTile.transform.position;
            }
        }

        CancelCTS();
        cts = new();
    }

    async UniTaskVoid UnBlastAnim()
    {
        spriteRenderer.sortingOrder = currentBGTile.GetSortingOrderValue;
        spriteRenderer.sortingOrder += 5;
        visualTileAnimator.SetTrigger(ConstStrings.VISUAL_TILE_UNBLAST);
        SoundManager.Instance.PlaySound2DVolume(ConstStrings.SFX_TILE_UNBLAST, .25f);
        await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cts.Token);
        spriteRenderer.sortingOrder = currentBGTile.GetSortingOrderValue;
    }

    void CancelCTS()
    {
        cts.Cancel();
    }

    private void OnDestroy() 
    {
        CancelCTS();
    }

}

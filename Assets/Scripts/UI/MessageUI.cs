using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class MessageUI : MonoBehaviour
{
    public static MessageUI Instance;

    [SerializeField] Animator myAnimator;
    [SerializeField] TextMeshProUGUI messageText;

    CancellationTokenSource cts = new();

    private void Awake() 
    {
        Instance = this;
    }

    public void CallMessageOnUI(string message, float duration, float delay = default)
    {
        AppearMessage(message, duration, delay).Forget();
    }

    async UniTaskVoid AppearMessage(string message, float duration, float delay = default)
    {
        messageText.text = message;
        await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: cts.Token);
        myAnimator.SetTrigger(ConstStrings.MESSAGEUI_SHOW);
        await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: cts.Token);
        myAnimator.SetTrigger(ConstStrings.MESSAGEUI_HIDE);
        
        cts.Cancel();
        cts = new();
    }

    public void PlaySFX()
    {
        SoundManager.Instance.PlaySound2D(ConstStrings.SFX_MESSAGE_UI);
    }

    void OnDestroy() 
    {
        cts.Cancel();
    }

}

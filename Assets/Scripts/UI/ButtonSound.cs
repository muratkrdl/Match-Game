using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void Play2DSound(AnimationEvent animationEvent)
    {
        SoundManager.Instance.PlaySound2DVolume(animationEvent.stringParameter, animationEvent.floatParameter);
    }
}

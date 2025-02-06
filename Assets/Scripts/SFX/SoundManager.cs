using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
 
    [SerializeField] SoundLibrary sfxLibrary;
    [SerializeField] AudioSource sfx2DSource;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlaySound2D(string clipName)
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(clipName));
    }

    public void PlaySound2DVolume(string clipName, float volumeScale)
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(clipName), volumeScale);
    }
}

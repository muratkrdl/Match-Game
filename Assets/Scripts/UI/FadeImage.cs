using UnityEngine;

public class FadeImage : MonoBehaviour
{
    public static FadeImage Instance;

    [SerializeField] private Animator animator;

    private void Awake() 
    {
        Instance = this;
    }

    public void SetFadeImage(string str)
    {
        animator.SetTrigger(str);
    }

}

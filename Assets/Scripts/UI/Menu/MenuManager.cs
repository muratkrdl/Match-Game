using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public Action<Vector2Int,int,Sprite,int,int,int[]> OnClickPlay;

    [SerializeField] private MenuBaseClass[] menus;

    [SerializeField] private float delay;

    public float GetFadeImageDelay
    {
        get => delay;
    }

    private void Awake() 
    {
        Instance = this;
    }

    void Start() 
    {
        OpenMenu(menus[0]);
    }

    public void OnClick_OpenMyMenu(MenuBaseClass menu)
    {
        ChangeMenuWithDelay(menu).Forget();
    }

    public void OnClick_CloseMyAllMenu()
    {
        ChangeMenuWithDelay(null).Forget();
    }

    public async UniTaskVoid ChangeMenuWithDelay(MenuBaseClass menu)
    {
        FadeImage.Instance.SetFadeImage(ConstStrings.FADE_IMAGE_FADE);
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        FadeImage.Instance.SetFadeImage(ConstStrings.FADE_IMAGE_UNFADE);

        if(menu != null)
            OpenMenu(menu);
        else
            CloseAllMenu();
    }

    void OpenMenu(MenuBaseClass menu)
    {
        foreach(MenuBaseClass item in menus)
        {
            if(item == menu)
            {
                item.Open();
                continue;
            }
            else if(item.GetOpen)
            {
                item.Close();
            }
        }
    }

    public MenuBaseClass GetMenuByName(string name)
    {
        foreach (var item in menus)
        {
            if(item.name == name)
                return item;
        }
        
        return null;
    }

    void CloseAllMenu()
    {
        foreach(var item in menus)
        {
            item.Close();
        }
    }

    public void QuitGame()
    {
        QuitWithDelay().Forget();
    }

    async UniTaskVoid QuitWithDelay()
    {
        FadeImage.Instance.SetFadeImage(ConstStrings.FADE_IMAGE_FADE);
        await UniTask.Delay(TimeSpan.FromSeconds(delay*2));
        Application.Quit();
    }

}

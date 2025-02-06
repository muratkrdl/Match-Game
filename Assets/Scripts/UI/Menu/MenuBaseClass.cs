using UnityEngine;

public abstract class MenuBaseClass : MonoBehaviour
{
    [SerializeField] private string menuName;
    private bool open = false;

    public string GetMenuName
    {
        get => menuName;
    }
    public bool GetOpen
    {
        get => open;
    }

    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
        OnOpenMyMenu();
    }

    public void Close()
    {
        OnCloseMyMenu();
        open = false;
        gameObject.SetActive(false);
    }

    protected virtual void OnOpenMyMenu()
    {
        /* */
    }
    protected virtual void OnCloseMyMenu()
    {
        /* */
    }
}

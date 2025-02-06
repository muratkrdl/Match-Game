using UnityEngine;

public class ESCMenu : MenuBaseClass
{
    [SerializeField] GameObject offPanel;

    protected override void OnOpenMyMenu()
    {
        if(!GameStateManager.Instance.CanPlayerInteract)
        {
            Close();
            return;
        }
        offPanel.SetActive(false);
    }

    protected override void OnCloseMyMenu()
    {
        offPanel.SetActive(true);
    }

}
